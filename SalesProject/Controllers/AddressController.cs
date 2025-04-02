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
    public class AddressController : ControllerBase
    {
        private readonly IAddressRepository _address;
        private readonly IMapper _mapper;

        public AddressController(IAddressRepository address, IMapper mapper)
        {
            _address = address;
            _mapper = mapper;
        }


        [HttpPost]
        public async Task<IActionResult> CreateAddress([FromBody] CreateAddressRequestDto addressRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var addressDomain = _mapper.Map<Address>(addressRequestDto);

            addressDomain = await _address.CreateAddress(addressDomain);

            var addressDto = _mapper.Map<AddressResponse>(addressDomain);

            return Ok(addressDto);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAddress([FromBody] UpdateAddressRequestDto updateAddressRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var addressDomain = _mapper.Map<Address>(updateAddressRequestDto);

            addressDomain = await _address.UpdateAddress(addressDomain);

            if (addressDomain == null)
            {
                return NotFound("Address not found or does not belong to the user.");
            }

            var addressDto = _mapper.Map<AddressResponse>(addressDomain);

            return Ok(addressDto);
        }


        [HttpGet]
        [Route("{id:Guid}")]

        public async Task<IActionResult> GetAddressByUserId([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           var addressDomain = await _address.GetAddressByUserId(id);


            if(addressDomain == null)
            {
                return NotFound("User not found");
            }

            var addressDto = _mapper.Map<List<AddressResponse>>(addressDomain);

            return Ok(addressDto);


        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAddressUser([FromBody] DeleteAddressUser deleteAddressUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var addressDomain = _mapper.Map<Address>(deleteAddressUser);

            addressDomain = await _address.DeleteAddress(addressDomain);

            if(addressDomain == null)
            {
                return NotFound();
            }

            //convert domain to dto
            var addressDto = _mapper.Map<AddressResponse>(addressDomain);

            return Ok(addressDto);


        }

        [HttpDelete("Admin/{id:Guid}")]
        
        public async Task<IActionResult> DeleteAddress([FromRoute] Guid id)
        {
            var  addressDomain = await _address.DeleteByAdmin(id);


            if(addressDomain == null)
            {
                return NotFound();
            }

            //convert domain to dto
            var addressDto = _mapper.Map<AddressResponse>(addressDomain);

            return Ok(addressDto);
        }

    }
}
