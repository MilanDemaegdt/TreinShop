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
    public class TreinController : ControllerBase
    {
        private readonly IMapper _mapper;
        private IService<Trein> _treinService;
        public TreinController(IMapper mapper, IService<Trein> treinService)
        {
            _mapper = mapper;
            //DI
            _treinService = treinService;
        }
        /// <summary>
        /// Get the list of all Employees.
        /// </summary>
        /// <returns>The list of Employees.</returns>
        // GET: api/Trein
        [HttpGet]
        public async Task<IEnumerable<TreinVM>> Get()
        {
            var listEmployee = await _treinService.GetAll();
            return _mapper.Map<List<TreinVM>>(listEmployee);
        }
        /// <summary>
        /// Get one Trein.
        /// </summary>
        /// <returns>Get one Trein</returns>
        // GET: api/Trein/5
        [HttpGet("{id}", Name = "GetTrainById")]
        public async Task<TreinVM> Get(int id)
        {
            var Trein = await _treinService.FindById(id);
            return _mapper.Map<TreinVM>(Trein);
        }

        [HttpGet("{vertrekID}/{aankomstID}", Name = "GetByStations")]
        public async Task<IEnumerable<TreinVM>> Get(int vertrekID, int aankomstID)
        {
            TimeSpan timeSpan = new TimeSpan(0, 0, 0);
            var Trein = await _treinService.GetTrainsByStation(vertrekID, aankomstID, timeSpan);
            return _mapper.Map<List<TreinVM>>(Trein.First());
        }
        /// <summary>
        /// Creates an Trein.
        /// </summary>
        /// <remarks>
        /// Sample request
        ///     POST api/Trein
        ///     {         
        ///       "firstName": "Karel",
        ///       "lastName": "Vermeulen",
        ///       "emailId": "Karel.Vermeulen@gmail.com"         
        ///     }
        /// </remarks>
        /// 
        // POST: api/Trein
        //    [HttpPost]
        //    //  [Produces("application/json")]
        //    //  [FromForm] ‐ Gets values from posted form fields.
        //    public async Task<ActionResult<TreinVM>> Post([FromForm] EmployeePostVM employeePostVM)
        //    {
        //        try
        //        {
        //            var Trein = _mapper.Map<Trein>(employeePostVM);
        //            // Logic to create new Trein
        //            await _treinService.Add(Trein);
        //            return CreatedAtAction(nameof(Get), new { id = Trein.EmployeeId }, Trein);
        //        }
        //        catch (Exception ex)
        //        { return BadRequest(); }
        //    }
    }
}
