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
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private IService<AspNetUser> _userService;
        public UserController(IMapper mapper, IService<AspNetUser> userService)
        {
            _mapper = mapper;
            //DI
            _userService = userService;
        }
        /// <summary>
        /// Get the list of all Employees.
        /// </summary>
        /// <returns>The list of Employees.</returns>
        // GET: api/User
        [HttpGet]
        public async Task<IEnumerable<UserVM>> Get()
        {
            var listEmployee = await _userService.GetAll();
            return _mapper.Map<List<UserVM>>(listEmployee);
        }
        /// <summary>
        /// Get one Trein.
        /// </summary>
        /// <returns>Get one Trein</returns>
        // GET: api/User/5
        [HttpGet("{id}", Name = "GetUserById")]
        public async Task<UserVM> Get(int id)
        {
            var Trein = await _userService.FindById(id);
            return _mapper.Map<UserVM>(Trein);
        }
    }
}
