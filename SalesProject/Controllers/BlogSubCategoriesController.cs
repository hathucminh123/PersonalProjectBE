using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesProject.Interface;
using SalesProject.Models.Domain;
using SalesProject.Models.DTOs.Request;
using SalesProject.Models.DTOs.Response;

namespace SalesProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogSubCategoriesController : ControllerBase
    {
        private readonly IBlogSubCategoryService _service;
        private readonly IMapper _mapper;

        public BlogSubCategoriesController(IBlogSubCategoryService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var subCategories = await _service.GetAllAsync();
            var responseDtos = _mapper.Map<List<BlogSubCategoryResponseDto>>(subCategories);
            return Ok(responseDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var subCategory = await _service.GetByIdAsync(id);
            if (subCategory == null) return NotFound();

            var responseDto = _mapper.Map<BlogSubCategoryResponseDto>(subCategory);
            return Ok(responseDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BlogSubCategoryRequestDto requestDto)
        {
            var entity = _mapper.Map<BlogSubCategory>(requestDto);
            var created = await _service.CreateAsync(entity);
            var responseDto = _mapper.Map<BlogSubCategoryResponseDto>(created);

            return CreatedAtAction(nameof(GetById), new { id = responseDto.Id }, responseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BlogSubCategoryRequestDto requestDto)
        {
            var entity = _mapper.Map<BlogSubCategory>(requestDto);
            var updated = await _service.UpdateAsync(id, entity);
            if (updated == null) return NotFound();

            var responseDto = _mapper.Map<BlogSubCategoryResponseDto>(updated);
            return Ok(responseDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }

        [HttpGet("by-name")]
        public async Task<IActionResult> GetByName([FromQuery] string name)
        {
            var subCategory = await _service.GetByNameAsync(name);
            if (subCategory == null) return NotFound();

            var response = _mapper.Map<BlogSubCategoryResponseDto>(subCategory);
            return Ok(response);
        }

    }

}
