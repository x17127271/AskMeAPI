using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AskMe.Domain.Interfaces;
using AskMe.Domain.Models;
using AskMeAPI.Helpers;
using AskMeAPI.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AskMeAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // Variables to use for dependency injection
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly AppSettings _appSettings;
        // constructor
        public UsersController(
            IMapper mapper,
            IUserService userService,
            IOptions<AppSettings> appSettings)
        {
            _mapper = mapper;
            _userService = userService;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// This method get a userId and returns the user
        /// if exist on the database.
        /// HTTP GET.
        /// </summary>
        /// <param name="userId">integer</param>
        /// <returns>UserDto</returns>
        [HttpGet("{userId}", Name = "GetUser")]
        public async Task<IActionResult> GetUser(int userId)
        {
            // call user service to search the user by id
            var user = await _userService.GetUserById(userId);
            // if user is null, means that user does not exist
            if(user == null)
            {
                // returns not found
                return NotFound();
            }
            //if user exists returns 200OK and the user details
            return Ok(_mapper.Map<UserDto>(user));
        }

        /// <summary>
        /// Returns the list of existing users on the database.
        /// HTTP GET verb.
        /// </summary>
        /// <returns>List<User>()</returns>
        [HttpGet()]
        public async Task<IActionResult> GetUsers()
        {
            // call user service to get the list of users
            var userEntitie = await _userService.GetUsers();
            // if the list is null
            if (userEntitie == null)
            {
                // returns not found 404
                return NotFound();
            }
            // if the list is not null, returns the list of users 200Ok
            return Ok(userEntitie);
        }

        /// <summary>
        /// This method is used to authenticate an existing user.
        /// Generates a Json Web Token for this user.
        /// HTTP POST verb.
        /// </summary>
        /// <param name="model">UserAuthenticateDto</param>
        /// <returns>User</returns>
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UserAuthenticateDto model)
        {
            // call user service to authenticate the user
            var user = await _userService.Authenticate(model.Username, model.Password);
            // if user is null
            if (user == null)
            {
                // returns bad request 400
                return BadRequest(new { message = "Username or password is not valid." });
            }
            // to generate the token for the JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

           // returns the user and adds the generated JWT token 200ok
            return Ok(new
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = tokenString
            });
        }

        /// <summary>
        /// This method creates a new user.
        /// HTTP POST verb.
        /// </summary>
        /// <param name="model">UserForCreationDto</param>
        /// <returns>200Ok</returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserForCreationDto model)
        {
            // map resource to domain model
            var user = _mapper.Map<User>(model);

            try
            {
                // call user service to create a new user
               await _userService.Create(user, model.Password);
                // returns 200ok
                return Ok();
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
