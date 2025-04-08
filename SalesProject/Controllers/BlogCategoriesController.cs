using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SalesProject.Interface;
using SalesProject.Models.Domain;
using SalesProject.Models.DTOs;
using SalesProject.Models.DTOs.Request;
using SalesProject.Models.DTOs.Response;
//using SalesProject.Services;

[ApiController]
[Route("api/[controller]")]
public class BlogCategoriesController : ControllerBase
{
    private readonly IBlogCategoryService _service;
    private readonly IMapper _mapper;

    public BlogCategoriesController(IBlogCategoryService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _service.GetAllAsync();
        var responseDtos = _mapper.Map<List<BlogCategoryResponseDto>>(categories);
        return Ok(responseDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await _service.GetByIdAsync(id);
        if (category == null) return NotFound();

        var responseDto = _mapper.Map<BlogCategoryResponseDto>(category);
        return Ok(responseDto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] BlogCategoryRequestDto requestDto)
    {
        var category = _mapper.Map<BlogCategory>(requestDto);
        var created = await _service.CreateAsync(category);
        var responseDto = _mapper.Map<BlogCategoryResponseDto>(created);

        return CreatedAtAction(nameof(GetById), new { id = responseDto.Id }, responseDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] BlogCategoryRequestDto requestDto)
    {
        var updatedModel = _mapper.Map<BlogCategory>(requestDto);
        var updated = await _service.UpdateAsync(id, updatedModel);
        if (updated == null) return NotFound();

        var responseDto = _mapper.Map<BlogCategoryResponseDto>(updated);
        return Ok(responseDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.DeleteAsync(id);
        return result ? NoContent() : NotFound();
    }
}
