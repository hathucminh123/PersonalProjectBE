using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesProject.Dtos;
using SalesProject.Interface;
using SalesProject.Models.Domain;
using SalesProject.Models.DTOs.Request;
using SalesProject.Models.DTOs.Response;

namespace SalesProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersRepository ordersRepository;
        private readonly IMapper mapper;

        public OrdersController(IOrdersRepository ordersRepository ,IMapper mapper)
        {
            this.ordersRepository = ordersRepository;
            this.mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var order = await ordersRepository.GetOrdersHistory();


            //convert domain to dto 
            var orderDto = mapper.Map<List<OrderDto>>(order);

            return Ok(orderDto);
        }


        [HttpGet]
        [Route("{userId:Guid}")]
        public async Task<IActionResult> GetAllOrdersbyUserId([FromRoute]Guid userId )
        {
            var order = await ordersRepository.GetOrderHistoryByUserIdAsync(userId);


            if (order == null)
            {
                return BadRequest(string.Empty);
            }

            //convert domain to dto 
            var orderDto = mapper.Map<List<OrderDto>>(order);

            return Ok(orderDto);
        }


        [HttpPost]
        
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            try
            {
                var shippingAddressInput = mapper.Map<Address>(request.shippingAddressInput);

                var order = await ordersRepository.CreateOrderAsync(request.userId, shippingAddressInput, request.PaymentMethod, request.DiscountCode);


                var orderDto =  mapper.Map< OrderDto > (order);

                //return Ok(new
                //{
                //    order.Id,
                //    order.UserId,
                //    order.TotalPrice,
                //    order.DiscountAmount,
                //    order.Status
                //});
                return Ok(orderDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
