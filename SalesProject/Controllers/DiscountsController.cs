using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesProject.DTOs;
using SalesProject.Interface;
using SalesProject.Models.Domain;
using SalesProject.Models.DTOs.Response;

namespace SalesProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IDiscountRepository discountRepository;

        public DiscountsController(IMapper mapper, IDiscountRepository discountRepository)
        {
            this.mapper = mapper;
            this.discountRepository = discountRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var discounts = await discountRepository.GetDiscounts();

            // convert domain to dto 

            var discountsDto = mapper.Map<List<DiscountResponseDto>>(discounts);

            return Ok(discountsDto);
        }





        [HttpPost]

        public async Task<IActionResult> CreateDiscount([FromBody] DiscountRequestDto discountRequestDto)
        {
            //convert dto to domain 
            var discount = mapper.Map<Discounts>(discountRequestDto);

            discount = await discountRepository.CreateDiscount(discount);

            //map domain to dto 

            var discountDto = mapper.Map<DiscountResponseDto>(discount);

            return Ok(discountDto);





        }


        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteDiscount([FromRoute] Guid id)
        {
            var discount = await discountRepository.DeleteDiscount(id);
            var discountDto = mapper.Map<DiscountResponseDto>(discount);

            return Ok(discountDto);

        }
    }
}
