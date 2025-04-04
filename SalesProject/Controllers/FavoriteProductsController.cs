using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesProject.Interface;
using SalesProject.Models.DTOs;
using SalesProject.Models.DTOs.Request;
using SalesProject.Models.DTOs.Response;

namespace SalesProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteProductsController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly IFavoriteProduct _favoriteProduct;
        public FavoriteProductsController(IMapper mapper , IFavoriteProduct favoriteProduct)
        {

            _mapper = mapper;
            _favoriteProduct = favoriteProduct;
        }



        [HttpPost]
        public async Task<IActionResult> AddFavoriteProduct([FromBody] AddFavoriteProducts addFavoriteProducts)
        {
            var result = await _favoriteProduct.AddFavoriteProductAsync(addFavoriteProducts.ProductId, addFavoriteProducts.UserId);

            if (result == null)
            {
                return BadRequest("Failed to add favorite product.");
            }

            //convert domain to DTO

            var resultDto = _mapper.Map<AddFavoriteProductsRespone>(result);

            return Ok(resultDto);
        }



        [HttpGet("{userId:Guid}")]
        public async Task<IActionResult> GetAllFavoriteProductsByUserId([FromRoute] Guid userId)
        {
            var result = await _favoriteProduct.GetAllFavoriteProductsAsyncByUserId(userId);
            if (result == null)
            {
                return NotFound("No favorite products found for this user.");
            }
            //convert domain to DTO
            var resultDto = _mapper.Map<List<ProductDto>>(result);
            return Ok(resultDto);
        }


        [HttpDelete]
        [Route("{productId:Guid}/{userId:Guid}")]
        public async Task<IActionResult> RemoveFavoriteProduct([FromRoute] Guid productId, [FromRoute] Guid userId)
        {
            var result = await _favoriteProduct.RemoveFavoriteProductAsync(productId, userId);
            if (result == null)
            {
                return NotFound("No favorite products found for this user.");
            }
            //convert domain to DTO
            var resultDto = _mapper.Map<AddFavoriteProductsRespone>(result);
            return Ok(resultDto);
        }
    }
}
