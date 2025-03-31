using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SalesProject.Interface;
using SalesProject.Models.DTOs;
namespace SalesProject.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CompareController : ControllerBase
{
    private readonly ICompareRepository _compareRepository;
    private readonly IMapper _mapper;

    public CompareController(ICompareRepository compareRepository,IMapper mapper)
    {
        _compareRepository = compareRepository;
        _mapper = mapper;
    }

    // ✅ Thêm vào danh sách so sánh
    [HttpPost("add/{userId:Guid}/{productId:Guid}")]
    public async Task<IActionResult> AddToCompare(Guid userId, Guid productId)
    {

        var result = await _compareRepository.AddToCompare(userId, productId);
        if (result)
        return Ok("Sản phẩm đã được thêm vào danh sách so sánh.");
        return BadRequest("Sản phẩm đã tồn tại trong danh sách.");
    }

    // ✅ Xoá khỏi danh sách so sánh
    [HttpDelete("remove/{userId:Guid}/{productId:Guid}")]
    public async Task<IActionResult> RemoveFromCompare(Guid userId, Guid productId)
    {
        var result = await _compareRepository.RemoveFromCompare(userId, productId);
        if (result)
            return Ok("Đã xoá sản phẩm khỏi danh sách so sánh.");
        return NotFound("Không tìm thấy sản phẩm trong danh sách so sánh.");
    }

     //✅ Lấy danh sách sản phẩm đã được thêm vào để so sánh
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetCompareList(Guid userId)
    {
        var products = await _compareRepository.GetCompareList(userId);
        return Ok(products);
    }


    [HttpGet("compareProduct/{userId}")]
    public async Task<IActionResult> CompareProducts(Guid userId)
    {
        var products = await _compareRepository.GetCompareList(userId);

        if (products == null || products.Count < 2)
        {
            return BadRequest("Cần ít nhất 2 sản phẩm để so sánh.");
        }

        // ✅ Dùng AutoMapper để ánh xạ từ Products → ProductDto
        var result = _mapper.Map<List<ProductDto>>(products);

        return Ok(result);
    }


}
