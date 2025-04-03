using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesProject.Interface;
using SalesProject.Models.Domain;
using SalesProject.Models.DTO;
using SalesProject.Models.DTOs;
using SalesProject.Models.DTOs.Request;
using SalesProject.Models.DTOs.Response;

namespace SalesProject.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserRepository userRepository;
    private readonly IMapper mapper;

    public UsersController(IUserRepository userRepository ,IMapper mapper)
    {
        this.userRepository = userRepository;
        this.mapper = mapper;
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetUserById([FromRoute] Guid id)
    {
        var user = await userRepository.GetUsersById(id);

        if (user == null)
        {
            return NotFound();
        }

        var userDto = new UserDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            GoogleId = user.GoogleId,
            AvatarUrl = user.AvatarUrl,
            Phone = user.Phone,
            Address = user.Address,
            DateOfBirth = user.DateOfBirth,
            Role = user.Role,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,

            // Ánh xạ Orders
            Orders = user.Orders?.Select(order => new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                TotalPrice = order.TotalPrice, // ✅ Khớp với TotalAmount trong DTO
                DiscountAmount = order.DiscountAmount,
                ShippingFee = order.ShippingFee,
                Status = order.Status,
                CreatedAt = order.CreatedAt,
                OrderDetails = order.OrderDetails?.Select(detail => new OrderDetailsDto
                {
                    Id = detail.Id,
                    ProductName = detail.Product?.Name ?? string.Empty,
                    Quantity = detail.Quantity,
                    Price = detail.UnitPrice,
                    TotalPrice = detail.TotalPrice // ✅ Gán từ entity nếu đã thêm set
                }).ToList(),
             Discounts = order.OrderDiscounts?
            .Select(discount => new DiscountDto
            {
                Code = discount.Discount.Code,
                DiscountAmount = discount.Discount.DiscountAmount
            })
            .ToList()
            .ToList(),
            }).ToList(),

            // Ánh xạ CartItems
            CartItems = user.CartItems?.Select(cart => new CartDTO
            {
                Id = cart.Id,
                Product = cart.Product != null ? new ProductDto
                {
                    Id = cart.Product.Id,
                    Name = cart.Product.Name,
                    //Price = cart.Product.Price,
                    ImageUrl = cart.Product.ImageUrl
                } : null,
                Quantity = cart.Quantity,
               
            }).ToList(),

            // Ánh xạ Reviews
            Reviews = user.Reviews?.Select(review => new ReviewDto
            {
                Id = review.Id,
                Product = review.Product != null ? new ProductDto
                {
                    Id = review.Product.Id,
                    Name = review.Product.Name,
                    //Price = review.Product.Price,
                    ImageUrl = review.Product.ImageUrl
                } : null,
                Rating = review.Rating
            }).ToList(),
            Addresses = user.Addresses?.Select(address =>new AddressResponse
            {
                Id = address.Id,
                FullName =address.FullName,
                Phone =address.Phone,
                Province =address.Province,
                District= address.District,
                StreetAddress=address.StreetAddress,
                Ward= address.Ward,
                Email=address.Email,
                IsDefault =address.IsDefault,
                CreatedAt =address.CreatedAt,

            }).ToList(),


        };

        return Ok(userDto);
    }

    [HttpPut]
    [Route("{id:Guid}")]

    public async Task<IActionResult> UpdateUser([FromBody] UserRequest userRequest, [FromRoute] Guid id)
    {

        //Convert Dto to domain
        var user = mapper.Map<Users>(userRequest);

        user = await userRepository.UpdateUser(id, user);


        //convert domain to dto
        var   userDto = mapper.Map<UserDto>(user);

        return Ok(userDto);

    }


}
