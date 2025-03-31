using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesProject.Data;
using SalesProject.Interface;
using SalesProject.Models.Domain;
using SalesProject.Models.DTO;
using SalesProject.Models.DTOs;
using SalesProject.Models.DTOs.Request;
using SalesProject.Repositories;

namespace SalesProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly SalesDbContext salesDbContext;
        private readonly ICartRepository cartRepository;

        public CartController(SalesDbContext salesDbContext, ICartRepository cartRepository)
        {
            this.salesDbContext = salesDbContext;
            this.cartRepository = cartRepository;
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetCartById([FromRoute] Guid id)
        {
            var cart = await cartRepository.GetCartById(id);

            if (cart == null)
            {
                return NotFound();
            }

            // Use AutoMapper or manual mapping
            var cartDto = new CartDTO
            {
                Id = cart.Id,
                UserId = cart.UserId,
                UserName = cart.User?.FullName ?? "Unknown User",
                ProductId = cart.ProductId,
                ProductName = cart.Product?.Name ?? "Unknown Product",
                Quantity = cart.Quantity,
                CreatedAt = cart.CreatedAt,
                IsActive = cart.IsActive,
                UpdatedAt = cart.UpdatedAt
            };

            return Ok(cartDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCart([FromBody] CartRequest cartRequest)
        {
            // Convert DTO request to domain model
            var cartDomain = new Cart
            {
                UserId = cartRequest.UserId,
                ProductId = cartRequest.ProductId,
                Quantity = cartRequest.Quantity,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Create cart in repository
            cartDomain = await cartRepository.CreateCartforUser(cartDomain);

            // Map domain model to DTO
            var cartDto = new CartDTO
            {
                UserId = cartDomain.UserId,
                ProductId = cartDomain.ProductId,
                Quantity = cartDomain.Quantity,
                CreatedAt = cartDomain.CreatedAt,
                IsActive = cartDomain.IsActive,
                UpdatedAt = cartDomain.UpdatedAt
            };

            return CreatedAtAction(nameof(GetCartById), new { id = cartDomain.Id }, cartDto);
        }


        [HttpPut]
        public async Task<IActionResult> UpdateCart([FromBody] CartRequest cartRequest)
        {
            // Validate số lượng
            if (cartRequest.Quantity < 1)
            {
                return BadRequest("Số lượng sản phẩm phải lớn hơn hoặc bằng 1.");
            }

            // Chuyển DTO sang Domain Model
            var cart = new Cart
            {
                ProductId = cartRequest.ProductId,
                UserId = cartRequest.UserId,
                Quantity = cartRequest.Quantity
            };

            var updatedCart = await cartRepository.UpdateCartForUser(cart);

            if (updatedCart == null)
            {
                return NotFound("Không tìm thấy sản phẩm trong giỏ hàng của người dùng.");
            }

            // Chuyển Domain Model sang DTO để trả về
            var cartDto = new CartDTO
            {
                UserId = updatedCart.UserId,
                ProductId = updatedCart.ProductId,
                Quantity = updatedCart.Quantity
            };

            return Ok(cartDto);
        }



        [HttpDelete]
        [Route("{productId:Guid}/{userId:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid productId, [FromRoute] Guid userId)
        {
            var cart = await cartRepository.DeleteProductFromCart(userId, productId);

            if (cart == null)
            {
                return NotFound(); // Trả về HTTP 404 nếu không tìm thấy
            }

            // Convert domain model to DTO (nếu cần)
            var cartDto = new CartDTO
            {
                Id = cart.Id,
                UserId = cart.UserId,
                ProductId = cart.ProductId,
                Quantity = cart.Quantity,
                CreatedAt = cart.CreatedAt,
                IsActive = cart.IsActive,
                UpdatedAt = cart.UpdatedAt
            };

            // ✅ Trả về HTTP 200 với dữ liệu
            return Ok(cartDto);
        }


        [HttpDelete]
        [Route("{userId:Guid}")]
        public async Task<IActionResult> DeleteAll([FromRoute] Guid userId)
        {
            var deletedCount = await cartRepository.ClearCartByUserId(userId);
            if (deletedCount > 0)
            {
                return NoContent(); // HTTP 204 - Xóa thành công
            }

            return NotFound($"No cart items found for user with ID {userId}"); // HTTP 404


        }



        [HttpGet]
        [Route("user/{id:Guid}")]

        public async Task<IActionResult> GetCartByUsrId([FromRoute] Guid id)
        {
            var carts = await cartRepository.GetCartsByUserId(id);

            if (carts == null || !carts.Any())
            {
                return NotFound();
            }

            var cartDtos = carts.Select(item => new CartDTO
            {
                Id = item.Id,
                UserId = item.UserId,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                CreatedAt = item.CreatedAt,
                UpdatedAt = item.UpdatedAt,
                IsActive = item.IsActive,
                Product = item.Product != null ? new ProductDto
                {
                    Id = item.Product.Id,
                    Name = item.Product.Name,
                    Description = item.Product.Description,
                    OriginalPrice = item.Product.OriginalPrice,
                    DiscountAmount = item.Product.DiscountAmount,
                    FinalPrice = item.Product.FinalPrice,
                    ImageUrl = item.Product.ImageUrl,
                    DiscountPercentage= item.Product.DiscountPercentage,
                    Stock = item.Product.Stock,
                    // Các trường khác nếu cần
                } : null
            }).ToList();
            return Ok(cartDtos);
        }



        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddOrUpdateCart([FromBody] CartRequest cartRequest)
        {
            if (cartRequest.Quantity < 1)
            {
                return BadRequest("Số lượng phải >= 1.");
            }

            var cartDomain = new Cart
            {
                UserId = cartRequest.UserId,
                ProductId = cartRequest.ProductId,
                Quantity = cartRequest.Quantity,
                IsActive = true
            };

            var result = await cartRepository.CreateOrUpdateCart(cartDomain);

            var cartDto = new CartDTO
            {
                Id = result.Id,
                UserId = result.UserId,
                ProductId = result.ProductId,
                Quantity = result.Quantity,
                CreatedAt = result.CreatedAt,
                UpdatedAt = result.UpdatedAt,
                IsActive = result.IsActive,
                Product = result.Product != null ? new ProductDto
                {
                    Id = result.Product.Id,
                    Name = result.Product.Name,
                    FinalPrice = result.Product.FinalPrice,
                    OriginalPrice = result.Product.OriginalPrice,
                    DiscountAmount = result.Product.DiscountAmount,
                    DiscountPercentage = result.Product.DiscountPercentage,
                    ImageUrl = result.Product.ImageUrl,
                    Stock = result.Product.Stock
                } : null
            };

            return Ok(cartDto);
        }


    }
}
