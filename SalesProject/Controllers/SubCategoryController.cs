using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesProject.Interface;
using SalesProject.Models.Domain;
using SalesProject.Models.DTOs.Request;
using SalesProject.Models.DTOs.Response;

namespace SalesProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoryController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ISubCategoryRepository subCategoryRepository;

        public SubCategoryController(IMapper mapper,ISubCategoryRepository subCategoryRepository)
        {
            this.mapper = mapper;
            this.subCategoryRepository = subCategoryRepository;
        }



        [HttpGet]


        public async Task<IActionResult> GetAll()
        {
            var subcategories = await subCategoryRepository.GetAllAsync();

            //convert domain to dto
            var subcategoriesDto = mapper.Map<List<SubCategoryResponse>>(subcategories);

            return Ok(subcategoriesDto);
        }


        [HttpGet]
        [Route("{id:Guid}")]

        public async Task<IActionResult> GetCategorybyId([FromRoute] Guid id)
        {
            var subcategory = await subCategoryRepository.GetSubCategorybyIdAsync(id);
            if (subcategory == null)
            {
                return NotFound();
            }
            var subcategoriesDto = mapper.Map<SubCategoryResponse>(subcategory);

            return Ok(subcategoriesDto);

        }



        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] SubCategoryCreateRequest subCategoryCreateRequest)
        {
            //conert dto to domain
            var subcategory = mapper.Map<SubCategory>(subCategoryCreateRequest);

            subcategory = await subCategoryRepository.CreateSubCategoryAsync(subcategory);


            //convert domain to dto 
            var subcategoryDto = mapper.Map<SubCategoryResponse>(subcategory);
            return Ok(subcategoryDto);
        }


        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateCategorybyId([FromRoute] Guid id, [FromBody] SubCategoryUpdateRequest subCategoryUpdateRequest)
        {

            //conert dto to domain
            var subcategory = mapper.Map<SubCategory>(subCategoryUpdateRequest);
             subcategory = await subCategoryRepository.UpdateSubCategoryAsync(id , subcategory);
            if (subcategory == null)
            {
                return NotFound();
            }
            var subcategoriesDto = mapper.Map<SubCategoryResponse>(subcategory);

            return Ok(subcategoriesDto);

        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteCategorybyId(Guid id)
        {
            var subcategory = await subCategoryRepository.DeleteSubCategoryAsync(id);
            if (subcategory == null)
            {
                return NotFound();
            }
            var subcategoriesDto = mapper.Map<SubCategoryResponse>(subcategory);

            return Ok(subcategoriesDto);
        }
    }
}
