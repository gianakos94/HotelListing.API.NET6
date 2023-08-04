using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListingAPI.Data;
using HotelListingAPI.Contracts;
using AutoMapper;
using HotelListingAPI.Models.Hotel;
using Microsoft.AspNetCore.Authorization;
using HotelListingAPI.Models;
using HotelListingAPI.Models.Country;
using HotelListingAPI.Repository;

namespace HotelListingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Adminstrator")]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelsRepository _hotelsRepository;
        private readonly IMapper _mapper;


        public HotelsController(IHotelsRepository hotelsRepository, IMapper mapper)
        {
            _hotelsRepository = hotelsRepository;
            this._mapper = mapper;
        }

        // GET: api/Hotels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HotelDto>>> Gethotels()
        {
            //var hotels = await _hotelsRepository.GetAllAsync();
            //return Ok(_mapper.Map<List<HotelDto>>(hotels));    

            var hotels = await _hotelsRepository.GetAllAsync<HotelDto>();
            return Ok(hotels);
        }

        // GET: api/Hotels?StartIndex=0&pagesize=25&PageNumber=1
        [HttpGet ("HotelPaging")]
        public async Task<ActionResult<PagedResult<HotelDto>>> HotelPagingTest([FromQuery] QueryParameters queryParameters)
        {
   
            var pagedhotels = await _hotelsRepository.GetAllAsync<HotelDto>(queryParameters);

           return Ok(pagedhotels);
        }

        // GET: api/Hotels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HotelDto>> GetHotel(int id)
        {
            var hotel = await _hotelsRepository.GetAsync(id);
            return Ok(hotel);

            //if (hotel == null)
            //{
            //    return NotFound();
            //}

            //return Ok(_mapper.Map<HotelDto>(hotel));
        }

        // PUT: api/Hotels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHotel(int id, HotelDto hotelDto)
        {
            if (id != hotelDto.Id)
            {
                return BadRequest();
            }

            //var hotel = await _hotelsRepository.GetAsync(id);
            //if (hotel == null)
            //{
            //    return NotFound();
            //}

            //_mapper.Map(hotelDto, hotel);

            try
            {
                await _hotelsRepository.UpdateAsync(id, hotelDto);
            }


            catch (DbUpdateConcurrencyException)
            {
                if (!await HotelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Hotels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Hotel>> PostHotel(CreateHotelDto CreateHotelDto)
        {
            //var hotel = _mapper.Map<Hotel>(CreateHotelDto);
            //await _hotelsRepository.AddAsync(hotel);

            //return CreatedAtAction("GetHotel", new { id = hotel.Id }, hotel);

            var hotel = await _hotelsRepository.AddAsync<CreateHotelDto, HotelDto> (CreateHotelDto);
            return CreatedAtAction(nameof(GetHotel), new { id = hotel.Id }, hotel);
        }

        // DELETE: api/Hotels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            //var hotel = await _hotelsRepository.GetAsync(id);
            //if (hotel == null)
            //{
            //    return NotFound();
            //}

            await _hotelsRepository.DeleteAsync(id);
            return NoContent();
        }

        private async Task<bool> HotelExists(int id)
        {
            return await _hotelsRepository.Exists(id);
        }
    }
}
