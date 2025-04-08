using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SalesProject.Interface;
using SalesProject.Models.Domain;
using SalesProject.Models.DTOs.Request;
using SalesProject.Models.DTOs.Response;

namespace SalesProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostService _service;
        private readonly IMapper _mapper;

        public BlogPostsController(IBlogPostService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var posts = await _service.GetAllAsync();
            var responseDtos = _mapper.Map<List<BlogPostResponseDto>>(posts);
            return Ok(responseDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var post = await _service.GetByIdAsync(id);
            if (post == null) return NotFound();

            var responseDto = _mapper.Map<BlogPostResponseDto>(post);
            return Ok(responseDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BlogPostRequestDto requestDto)
        {
            var entity = _mapper.Map<BlogPost>(requestDto);
            var created = await _service.CreateAsync(entity);
            var responseDto = _mapper.Map<BlogPostResponseDto>(created);

            return CreatedAtAction(nameof(GetById), new { id = responseDto.Id }, responseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BlogPostRequestDto requestDto)
        {
            var entity = _mapper.Map<BlogPost>(requestDto);
            var updated = await _service.UpdateAsync(id, entity);
            if (updated == null) return NotFound();

            var responseDto = _mapper.Map<BlogPostResponseDto>(updated);
            return Ok(responseDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}
