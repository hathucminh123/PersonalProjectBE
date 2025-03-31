using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesProject.Data;
using SalesProject.Interface;
using SalesProject.Models.Domain;
using SalesProject.Models.DTOs;
using SalesProject.Models.DTOs.Request;
using SalesProject.Models.DTOs.Response;

namespace SalesProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly SalesDbContext salesDbContext;
        private readonly ICategory category;
        private readonly IMapper mapper;

        public CategoriesController(SalesDbContext salesDbContext ,ICategory category , IMapper mapper)
        {
            this.salesDbContext = salesDbContext;
            this.category = category;
            this.mapper = mapper;
        }


        [HttpGet]

        public async Task<IActionResult> GetAll()
        { 
            var Categories = await category.GetAsync();


            //convert domain to dto 

            var CategoriesDto = mapper.Map<List< CategoryResponse >>(Categories);
            return Ok(CategoriesDto);

            

        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetCateGorybyId([FromRoute] Guid id)
        {

            var Catgory = await category.GetCategorybyIdAsync(id);

            if (Catgory == null)
            {
                return NotFound();
            }
            var CategoriesDto = mapper.Map<CategoryResponse>(Catgory);



            //convert to Dto 


            return Ok(CategoriesDto);

        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CategoryCreateRequest addCategoriesDto)
        {
            //convert dto to domain
            var categorydomain = mapper.Map<Category>(addCategoriesDto);

            categorydomain = await category.CreateCategoryAsync(categorydomain);



            var CategoryDto = mapper.Map<CategoryResponse>(categorydomain);

            return CreatedAtAction(nameof(GetCateGorybyId), new { id = CategoryDto.Id }, CategoryDto);


        }

        [HttpPut]
        [Route("{id:Guid}")]


        public async Task<IActionResult> Update([FromBody] CategoryUpdateRequest updateCategoriesDto, [FromRoute] Guid id)
        {
            //convert dto to domain
            var categorydomain = mapper.Map<Category>(updateCategoriesDto);

            categorydomain = await category.UpdateCategoryAsync(id, categorydomain);

            if (categorydomain == null)
            {
                return NotFound();
            }



            //covert Domain Model to DTO
            var CategoryDto = mapper.Map<CategoryResponse>(categorydomain);

            return Ok(CategoryDto);


        }


        [HttpDelete]
        [Route("{id:Guid}")]

        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
           var categorydomain = await category.DeleteCategoryAsync(id);

            if (categorydomain == null)
            {
                return NotFound();
            }





            var CategoryDto = mapper.Map<CategoryResponse>(categorydomain);

            return Ok(CategoryDto);



        }
    }
}
