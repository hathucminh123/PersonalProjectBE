using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesProject.Interface;
using SalesProject.Models.Domain;
using SalesProject.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProductRepository productRepository;

        public ProductsController(IMapper mapper, IProductRepository productRepository)
        {
            _mapper = mapper;
            this.productRepository = productRepository;
        }

        // GET: api/products
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await productRepository.GetAllProduct();
            var productsDto = _mapper.Map<List<ProductDto>>(products);
            return Ok(productsDto);
        }

        // GET: api/products/{id}
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var product = await productRepository.GetProductById(id);

            if (product == null)
            {
                return NotFound($"Product with ID {id} not found");
            }

            var productDto = _mapper.Map<ProductDto>(product);
            return Ok(productDto);
        }

        // POST: api/products
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddProductDto addProductDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = _mapper.Map<Products>(addProductDto);
            product = await productRepository.AddProduct(product);

            var productDto = _mapper.Map<ProductDto>(product);

            return CreatedAtAction(nameof(GetById), new { id = productDto.Id }, productDto);
        }

        // PUT: api/products/{id}
        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] Guid id, [FromBody] UpdateProductRequestDto updateProductDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = _mapper.Map<Products>(updateProductDto);
            product = await productRepository.UpdateProduct(id, product);

            if (product == null)
            {
                return NotFound($"Product with ID {id} not found");
            }

            var productDto = _mapper.Map<ProductDto>(product);
            return Ok(productDto);
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
        {
            var product = await productRepository.DeleteProduct(id);

            if (product == null)
            {
                return NotFound($"Product with ID {id} not found");
            }

            var productDto = _mapper.Map<ProductDto>(product);
            return Ok(productDto);
        }
    }
}
