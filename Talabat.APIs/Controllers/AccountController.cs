using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;
using Talabat.Repository.Data;

namespace Talabat.APIs.Controllers
{

    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;


        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;

        }


        // Register
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await _userManager.UserExists(registerDto.Email)) return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest,"Email is taken"));

            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email.Split('@')[0],
                PhoneNumber = registerDto.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest));

            var ReturnedUser = new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager),
            };

            return Ok(ReturnedUser);
        }
        


       // Login
       [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) return Unauthorized(new ApiResponse(StatusCodes.Status401Unauthorized,"Invalid Email"));

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized(new ApiResponse(StatusCodes.Status401Unauthorized, "Invalid Password"));

            return new UserDto
            {
                DisplayName = user.DisplayName,
                Token = await _tokenService.CreateTokenAsync(user, _userManager),
                Email = user.Email
            };
        }
        [Authorize]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
           var Email = User.FindFirstValue(ClaimTypes.Email);

           var user = await _userManager.FindByEmailAsync(Email);

            return new UserDto
            {
                DisplayName = user.DisplayName,
                Token = await _tokenService.CreateTokenAsync(user, _userManager),
                Email = user.Email
            };
        }

        [Authorize]
        [HttpGet("GetCurrentUserAddress")]
        public async Task<ActionResult<AddressDto>> GetCurrentUserAddress()
        {
           var user = await _userManager.FindByEmailFromClaimsPrincipleWithAddressAsync(User);

            var addressDto = _mapper.Map<Address, AddressDto>(user.Address);
            return Ok(addressDto);
        }

        [Authorize]
        [HttpPut("UpdateUserAddress")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto addressDto)
        {
            var user = await _userManager.FindByEmailFromClaimsPrincipleWithAddressAsync(User);

            addressDto.Id = user.Address.Id;    // Ensure the address id is not changed
            user.Address = _mapper.Map<AddressDto, Address>(addressDto);

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded) return Ok(addressDto);

            return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Problem updating the user"));
        }
 
        [HttpGet("emailExists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }
    }
}
 