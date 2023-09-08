using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListingAPI.Data;
using HotelListingAPI.Models.Country;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using HotelListingAPI.Contracts;
using Microsoft.AspNetCore.Authorization;
using HotelListingAPI.Exceptions;
using HotelListingAPI.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics.Metrics;

namespace HotelListingAPI.Controllers
{
    
    [Route("api/v{version:apiVersion}countries")]
    [ApiController]
    [ApiVersion("1.0", Deprecated = true)]
    
    public class CountriesController : ControllerBase
    {
        
        private readonly IMapper _mapper;
        private readonly ICountriesRepository _countriesRepository;
        private readonly ILogger<CountriesController> _logger;

        public CountriesController (IMapper mapper, ICountriesRepository countriesRepository,ILogger<CountriesController> logger)
        { 
            this._mapper = mapper;
            this._countriesRepository = countriesRepository;
            this._logger = logger;
        }
        // GET: api/Countries/GetAll
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<GetCountryDro>>> GetCountries()
        {
            Console.WriteLine("bhka controller");
            //Take the info from database
           // var countries = await _countriesRepository.GetAllAsync();

            //Mapping to CountryDto and return
            //var records = _mapper.Map<List<GetCountryDro>>(countries);
            //return Ok(records);

            //Refactor because the mapping is on Repo
            var countries = await _countriesRepository.GetAllAsync<GetCountryDro>();
            Console.WriteLine("gyrisa return apo generic repo");
            return Ok(countries);
        }

        

        // GET: api/Countries/?StartIndex=0&pagesize=25&PageNumber=1
        [HttpGet]
        public async Task<ActionResult<PagedResult<GetCountryDro>>> GetPagedCountries([FromQuery] QueryParameters queryParameters)
        {

            //Take the info from database and we do the maping on the Repository
            var pagedCountryResult = await _countriesRepository.GetAllAsync<GetCountryDro>(queryParameters);

            //Mapping to CountryDto and return
            //var records = _mapper.Map<List<GetCountryDro>>(countries);

            return Ok(pagedCountryResult);
        }


        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDto>> GetCountry(int id)
        {
            //var country = await _countriesRepository.GetDetails(id);

            //if (country == null)
            //{
            //    throw new NotFoundException(nameof(GetCountry), id);
            //}

            //var countryDto = _mapper.Map<CountryDto>(country);
            //return Ok(countryDto);

            var country = await _countriesRepository.GetDetails(id);
            return Ok(country);
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutCountry(int id, UpdateCountryDto updateCountryDto)
        {
            if (id != updateCountryDto.Id)
            {
                return BadRequest("Invalid Record Id");
            }
            //Old version Refactor
            //    var country = await _countriesRepository.GetAsync(id);
            //    if (country == null)
            //    {
            //        throw new NotFoundException(nameof(GetCountry), id);
            //    }

            //    _mapper.Map(updateCountryDto, country);

            try
            {
                await _countriesRepository.UpdateAsync(id, updateCountryDto);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CountryExists(id))
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

        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Country>> PostCountry(CreateCountryDto createCountryDto)
        {

            //var country = _mapper.Map<Country>(createCountryDto);
            //return CreatedAtAction("GetCountry", new { id = country.Id }, country);
            //await _countriesRepository.AddAsync(country);


            var country = await _countriesRepository.AddAsync<CreateCountryDto, Country>(createCountryDto);
            return CreatedAtAction(nameof(GetCountry), new {id = country.Id}, country);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Adminstrator")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            //var country = await _countriesRepository.GetAsync(id);
            //if (country == null)
            //{
            //    throw new NotFoundException(nameof(GetCountry), id);
            //}

            await _countriesRepository.DeleteAsync(id);
            return NoContent();
        }

        private async Task<bool> CountryExists(int id)
        {
            return await _countriesRepository.Exists(id);
        }
    }
}
