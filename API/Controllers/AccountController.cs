﻿using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Models;
using API.RequestHelpers;
using API.Models.Identity;
using API.Interfaces;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAuthService _authService;
        private readonly DataContext _context;

        public AccountController(UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            DataContext context,
            IAuthService authService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _authService = authService;
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> LoginWithEmailAsync(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null) return Unauthorized();

            var result = await _userManager.CheckPasswordAsync(user, model.Password);
            //var token = _authService.CreateToken(user);

            if (result)
            {
                var role = await _userManager.GetRolesAsync(user);
                if (role != null)
                {
                    return await CreateUserObject(user, role[0]);
                }

                return await CreateUserObject(user);
            }

            return Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> RegisterWithEmailAsync(RegisterDto model)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == model.Email))
            {
                ModelState.AddModelError("email", "Email Taken");
                return ValidationProblem("Email Taken");
            }

            if (await _userManager.Users.AnyAsync(x => x.UserName == model.Username))
            {
                ModelState.AddModelError("username", "Username Taken");
                return ValidationProblem("Username Taken");
            }

            var user = new AppUser
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Username,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // add user to 'User' role by default.
                await _userManager.AddToRoleAsync(user, model.Role);

                return await CreateUserObject(user, model.Role);
            }

            return BadRequest(result.Errors);
        }

        [AllowAnonymous]
        [HttpPost("login-phone")]
        public async Task<ActionResult<UserDto>> LoginWithPhoneNumberAsync(PhoneLoginDto model)
        {
            // Verify phone number and verification code
            var isVerified = await _authService.VerifyPhoneNumber(model.PhoneNumber, model.VerificationCode);
            if (!isVerified)
            {
                return BadRequest("Invalid phone number or verification code");
            }

            // Proceed with login process
            var user = await _userManager.FindByNameAsync(model.PhoneNumber);

            if (user == null)
            {
                return Unauthorized();
            }

            // Generate token for the authenticated user
            var token = await _authService.CreatePhoneToken(user.PhoneNumber!);

            // Return token to the client
            return Ok(new { Token = token });
        }

        [AllowAnonymous]
        [HttpPost("register-phone")]
        public async Task<ActionResult<UserDto>> RegisterWithPhoneNumberAsync(PhoneRegisterDto model)
        {
            // Check if the phone number is already registered
            if (await _userManager.Users.AnyAsync(x => x.PhoneNumber == model.PhoneNumber))
            {
                ModelState.AddModelError("phoneNumber", "Phone number is already taken");
                return ValidationProblem("Phone number is already taken");
            }

            // Create a new user with the provided phone number
            var user = new AppUser
            {
                UserName = model.PhoneNumber, // Set phone number as the username
                PhoneNumber = model.PhoneNumber,
                DisplayName = model.DisplayName,
            };

            var result = await _userManager.CreateAsync(user);

            if (result.Succeeded)
            {
                // add user to 'User' role by default.
                await _userManager.AddToRoleAsync(user, model.Role);

                // Generate verification code and send it to the user's phone number
                var verificationCode = await _authService.CreatePhoneToken(model.PhoneNumber);

                // Generate token for the newly registered user
                var token = await _authService.CreateToken(user);

                // Return the verification code and token to the client for further processing
                return CreateOTPUserObject(model, user, verificationCode, token);
            }

            // Return any validation errors if user creation failed
            return BadRequest(result.Errors);
        }

        private UserDto CreateOTPUserObject(PhoneRegisterDto model, AppUser user, string verificationCode, string token)
        {
            return new UserDto
            {
                Id = user.Id,
                VerificationCode = verificationCode,
                Token = token,
                DisplayName = user.DisplayName,
                Role = model.Role
            };
        }

        //[Authorize(Roles = $"{RolesNames.VENDOR}, {RolesNames.IT}")]
        [AllowAnonymous]
        [HttpGet("get-all-roles")]
        public async Task<ActionResult<RolesDto>> GetAllRoles()
        {
            var roles = await _roleManager.Roles.Select(x => x.Name).ToListAsync();

            return Ok(new RolesDto { Roles = roles! });
        }

        [Authorize]
        [HttpGet("current-userId-roles")]
        public async Task<ActionResult<UserIdRolesDto>> GetCurrentUser()
        {
            return await getCurrentUserIdAndHisRoles();
        }

        [HttpPost("add-role")]
        [Authorize(Roles = RolesNames.ADMIN)]
        public async Task<IActionResult> AddRoleAsync(UserIdRoleId model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user is null || !await _roleManager.RoleExistsAsync(model.Role))
                return BadRequest("Invalid User Id or role");

            if (await _userManager.IsInRoleAsync(user, model.Role))
                return BadRequest($"User already assigned to this role");

            var result = await _userManager.AddToRoleAsync(user, model.Role);

            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok(model);
        }

        private async Task<ActionResult<UserDto>> CreateUserObject(AppUser user, string? role = RolesNames.CUSTOMER)
        {
            return new UserDto
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Username = user.UserName!,
                Token = await _authService.CreateToken(user),
                Role = role
            };
        }
        private async Task<ActionResult<CurrentUserDto>> CurrentUserObject(AppUser user)
        {
            return new CurrentUserDto
            {
                UserId = user.Id,
                DisplayName = user.DisplayName,
                Username = user.UserName!,
                Token = await _authService.CreateToken(user),
            };
        }

        private async Task<ActionResult<UserIdRolesDto>> getCurrentUserIdAndHisRoles()
        {
            var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (user is null) BadRequest("Retry, this user not found");

            var roles = await _userManager.GetRolesAsync(user!);

            if (roles is null) { BadRequest("Retry, this user not found"); }

            return new UserIdRolesDto
            {
                Roles = roles,
                UserId = user!.Id
            };
        }

        [HttpGet("get-users-by-role/{roleName}")]
        [Authorize(Roles = $"{RolesNames.VENDOR}, {RolesNames.IT}")]
        public async Task<ActionResult<List<UserDto>>> GetUsersByRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return NotFound($"Role '{roleName}' not found.");
            }

            var usersInRole = await _userManager.GetUsersInRoleAsync(roleName);
            if (usersInRole == null || !usersInRole.Any())
            {
                return NotFound($"No users found in the role '{roleName}'.");
            }

            var usersDto = new List<UserDto>();
            foreach (var user in usersInRole)
            {
                usersDto.Add(new UserDto
                {
                    Id = user.Id,
                    DisplayName = user.DisplayName,
                    Role = roleName,
                    Username = user.UserName,
                });
            }

            return Ok(usersDto);
        }
    }
}