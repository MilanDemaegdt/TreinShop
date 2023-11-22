using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TreinShop.Domain.Entities;
using TreinShop.Services.Interfaces;
using TreinShop.ViewModels;

namespace TreinShop.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private IService<Station> _stationService;
        public StationController(IMapper mapper, IService<Station> stationService)
        {
            _mapper = mapper;
            //DI
            _stationService = stationService;
        }
        /// <summary>
        /// Get the list of all Employees.
        /// </summary>
        /// <returns>The list of Employees.</returns>
        // GET: api/Station
        [HttpGet]
        public async Task<IEnumerable<StationVM>> Get()
        {
            var listEmployee = await _stationService.GetAll();
            return _mapper.Map<List<StationVM>>(listEmployee);
        }
        /// <summary>
        /// Get one Station.
        /// </summary>
        /// <returns>Get one Station</returns>
        // GET: api/Station/5
        [HttpGet("{id}", Name = "GetStationById")]
        public async Task<StationVM> Get(int id)
        {
            var Station = await _stationService.FindById(id);
            return _mapper.Map<StationVM>(Station);
        }
        /// <summary>
        /// Creates an Station.
        /// </summary>
        /// <remarks>
        /// Sample request
        ///     POST api/Station
        ///     {         
        ///       "firstName": "Karel",
        ///       "lastName": "Vermeulen",
        ///       "emailId": "Karel.Vermeulen@gmail.com"         
        ///     }
        /// </remarks>
        /// 
        // POST: api/Station
        //    [HttpPost]
        //    //  [Produces("application/json")]
        //    //  [FromForm] ‐ Gets values from posted form fields.
        //    public async Task<ActionResult<StationVM>> Post([FromForm] EmployeePostVM employeePostVM)
        //    {
        //        try
        //        {
        //            var Station = _mapper.Map<Station>(employeePostVM);
        //            // Logic to create new Station
        //            await _stationService.Add(Station);
        //            return CreatedAtAction(nameof(Get), new { id = Station.EmployeeId }, Station);
        //        }
        //        catch (Exception ex)
        //        { return BadRequest(); }
        //    }
    }
}
