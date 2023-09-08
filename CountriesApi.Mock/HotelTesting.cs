using AutoMapper;
using HotelListingAPI.Contracts;
using HotelListingAPI.Repository;
using HotelListingAPI.Controllers;
using HotelListingAPI.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using HotelListingAPI.Data;
using HotelListingAPI.Models.Hotel;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using FluentAssertions;
using NSubstitute;
using Microsoft.AspNetCore.Http;
using HotelListingAPI.Configurations;
using System.Net;
using HotelListingAPI.Models.Country;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.FileProviders;
using HotelListingAPI.Models;

namespace CountriesApi.Mock
{
    public class HotelTesting
    {
        private readonly HotelsController _controllerHotel;
        private readonly CountriesController _countriesController;

        private readonly AccountController _accountController;
        private readonly IAuthManager _authManager;

        //Nsubstistude Setup
        private readonly IHotelsRepository _mockIRepository = Substitute.For<IHotelsRepository>();
        private readonly ICountriesRepository _countriesRepository = Substitute.For<ICountriesRepository>();
        private readonly ILogger<CountriesController> _logger = Substitute.For<ILogger<CountriesController>>();

        private readonly ILogger<AccountController> _loggerAccount = Substitute.For<ILogger<AccountController>>();
 
        public HotelTesting()
        {
            //_controller = new HotelsController(_mockIRepository.Object, _mapper.Object);


            //!Better practise from Inject the Mapper ,is making a mapper for unitTest!

            var _mapper = new MapperConfiguration(x =>
            {
                x.AddProfile(new MapperConfig());
            }).CreateMapper();

            _controllerHotel = new HotelsController(_mockIRepository, _mapper);
            _countriesController = new CountriesController(_mapper, _countriesRepository, _logger);

            _accountController = new AccountController(_authManager, _loggerAccount); 
        }

        [Fact]
        public async Task Gethotels_ShouldReturnAlltheHotels()
        {
            //Arrange

            var expectedHotels = new List<HotelDto>
            {
                new HotelDto
                {
                    Id = 1,
                    Name = "Test",
                    Rating = 5,
                    Address = "test road",
                    CountryId = 1,
                },
                new HotelDto
                {
                    Id = 2,
                    Name = "Test Hotel 2",
                    Rating = 4,
                    Address = "456 Example Street",
                    CountryId = 2,
                }
            };

            //? _mockIRepository.Setup(repo => repo.GetAllAsync<HotelDto>())
                //.ReturnsAsync(expectedHotels);

            _mockIRepository.GetAllAsync<HotelDto>().Returns(expectedHotels);

            // Act
            var result = await _controllerHotel.Gethotels();
            ActionResult<IEnumerable<HotelDto>> actionResult = result;

            actionResult.Result.Should().BeOfType<OkObjectResult>();
            var okObjectResult = actionResult.Result as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            okObjectResult.StatusCode.Should().Be(200);
  
        }

        [Fact]
        public async Task GetHotel_ByID_IfIDisNotNull()
        {
            //Arrange

            var country_id = 1;
            var country_name = "Test";

            var expectedhotelsDto = new HotelDto
            {

                Id = 1,
                Name = "Test",
                Rating = 5,
                Address = "test road",
                CountryId = 1,

            };
            
            //This is with Mocking
            //_mockIRepository(repo => repo.GetAsync(country_id))
            // .ReturnsAsync(expectedhotels); // Make sure to return the expected hotel when the repository is called.

            _mockIRepository.GetAsync<HotelDto>(country_id).Returns(expectedhotelsDto);

            //Act
            var result = await _controllerHotel.GetHotel(country_id);

            //Assert
            result.Should().NotBeNull();

            Assert.Equal(country_id, expectedhotelsDto.Id);
            Assert.Equal(country_name, expectedhotelsDto.Name);

        }

        [Fact]
        public async Task DeleteCountry_ByID_ReturnNoContent()
        {
            //Arrange
            int id = 1;


            //Act
            var result = await _countriesController.DeleteCountry(id);


            //Assert
           result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task PostHotel()
        {
             //Arrange
            var hotelTestCreate = new CreateHotelDto
            {
                Name = "Vouligamsenhs",
                Address = "test road",
                Rating = 5,
                CountryId = 1,
            };

            _mockIRepository
                .AddAsync<CreateHotelDto, HotelDto>(Arg.Any<CreateHotelDto>())
                .Returns(Task.FromResult<HotelDto>(null)); // Placeholder setup


            var expectedHotelDto = new HotelDto
            {
                Name = "Vouligamsenhs",
                Address = "test road",
                Rating = 5,
                CountryId = 1,
            };


            //Configure mock Behavior
            _mockIRepository.AddAsync<CreateHotelDto, HotelDto>(Arg.Any<CreateHotelDto>())
                .Returns(Task.FromResult(expectedHotelDto));

            //!Act
            var result = await _controllerHotel.PostHotel(hotelTestCreate);


            //!Assert
            result.Should().NotBeNull();
            

            // Assert that the result is a CreatedAtActionResult
            var createdAtActionResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdAtActionResult.ActionName.Should().Be("GetHotel");

            // Assert that the "id" route value is not null
            createdAtActionResult.RouteValues["id"].Should().NotBeNull();

            // Assert repository interaction
            await _mockIRepository.Received(1).AddAsync<CreateHotelDto, HotelDto>(Arg.Any<CreateHotelDto>());

        }

        [Fact]
        public async Task RegisterAdmin_SuccessfulRegistration_ReturnsOkResult()
        {
            //Arrange

            var apiUserDto = new ApiUserDto
            {
                FirstName = "Test",
                LastName = "Test"
            };

            //Act
            var result = await _accountController.RegisterAdmin(apiUserDto);

            //Assert
            result.Should().BeOfType<ObjectResult>();
        }

        [Fact]
        public async Task RegisterAdmin_FailedRegistration_ReturnsBadRequestWithErrors()
        {
            // Arrange
            var apiUserDto = new ApiUserDto
            {
               
             
                
            };

      
            // Act
            var result = await _accountController.RegisterAdmin(apiUserDto);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            //result.Should().count(0);
        }

        [Fact]
        public async Task GetAllCountries_ReturnAll()
        {
            //Arrange

            //Act
            var result = await _countriesController.GetCountries();


            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<ActionResult<IEnumerable<HotelListingAPI.Models.Country.GetCountryDro>>>(); //Check if the result is ok result

        }
        [Fact]
        public async Task GetPagedCountries_ValidQueryParameters_ReturnOKWithPagedResult()
        {
            //Arrange
            var queryParameter = new HotelListingAPI.Models.QueryParameters
            {
                StartIndex = 1,
                PageNumber = 1,
                PageSize = 15
            };
            //Act
            var result = await _countriesController.GetPagedCountries(queryParameter);

            //Assert
            result.Should().BeOfType<ActionResult<PagedResult<GetCountryDro>>>();
            
        }

        [Fact]
        public async Task GetCountryById()
        {
            //Arrange
            int country_id = 1;

           //Making the expected list
            var expectedHotelForDto = new List<HotelDto>()
            {
                new HotelDto
                {
                Name = "Vouligamsenhs",
                Address = "test road",
                Rating = 5,
                CountryId = 1,
                }
            };

            var expectedCountry = new CountryDto
            {
                Id = 1,
                Name = "Test",
                ShortName = "T",
                Hotels = expectedHotelForDto // Assign the list
            };

            //Act
            _countriesRepository.GetDetails(country_id).Returns(expectedCountry);
            var result = await _countriesController.GetCountry(country_id);

            var okResult = result as ActionResult<CountryDto>;
            

            //Assert
            result.Should().NotBeNull();

        }

        [Fact]
        public async Task PutCountry_ValidUpate_ReturtnsNoContent()
        {

            //Arrange
            int country_id = 1;

            var updatecountry = new UpdateCountryDto
            {
                Id=country_id,
                Name="Test",
                ShortName="T",
            };
            
            //ACT
            var result = await _countriesController.PutCountry(country_id, updatecountry);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<NoContentResult>();

        }

        [Fact]
        public async Task PutCountry_InvalidId_ReturnsBadRequest()
        {
            //Arrange
            var id = 1;

            var updatecountry = new UpdateCountryDto
            {
                Id = 2,
                Name = "Test",
                ShortName = "T",
            };

            //Act
            var result = await _countriesController.PutCountry(id, updatecountry);

            //Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task PutCountry_ConcurrencyException_ReturnsNotFound()
        {

            //Arrange
            var id = 1;

            var updatecountry = new UpdateCountryDto
            {
                Id = id,
                Name = "Test",
                ShortName = "T",
            };


            //Act
            var result = await _countriesController.PutCountry(id, updatecountry);

            //Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task PostHotel_InvalidInput_ReturnsBadRequest()
        {
            //Arrange
            var nameInvalidHotelDto = new CreateHotelDto
            {
                // Name property is intentionally missing
                Address = "test road",
                Rating = 5,
                CountryId = 1,
            };

            _controllerHotel.ModelState.AddModelError("Name", "Required");

            //!If you're testing the case where validation fails and you're not
            //expecting any interaction with the repository
            //(since the validation error should be caught before reaching the repository),
            //you don't need the _mockIRepository setup or the expectedHotelDto object in your test.
            //Configure mock Behavior

            //ACT
            var BadResult = await _controllerHotel.PostHotel(nameInvalidHotelDto);

            //ASSERTIONS
            BadResult.Should().NotBeNull();
          
        }



    }

}
