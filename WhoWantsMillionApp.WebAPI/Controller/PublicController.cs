using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WhoWantsMillionApp.WebAPI.Infrastructure.Auth;
using System.Security.Claims;
using WhoWantsMillionApp.WebAPI.Infrastructure.Models;
using FluentValidation;
using FluentValidation.Results;
using WhoWantsMillionApp.Services;
using Mapster;
using WhoWantsMillionApp.Services.Models;
using WhoWantsMillionApp.WebAPI.Infrastructure.Validations.User;
using WhoWantsMillionApp.WebAPI.Infrastructure.Mappers;

namespace WhoWantsMillionApp.WebAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class PublicController : ControllerBase
    {
        private IJwt _jwtMethods;
        private IUserService _userService;
        public PublicController(IJwt jwtMethods, IUserService userService)
        {
            this._jwtMethods = jwtMethods;
            this._userService = userService;
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDTO newUser, 
            [FromServices] IValidator<UserRegistrationDTO> validator, CancellationToken token)
        {
            ValidationResult validationResult = await validator.ValidateAsync(newUser,token);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

            return Ok(await _userService.RegisterUser
                (newUser.Adapt<UserRequestModel>(UserRegistrationDTOToEverything.UserRegistrationDTOToUserRequestModel),token));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginDTO user,
            [FromServices] IValidator<UserLoginDTO> validator, CancellationToken token)
        {
            ValidationResult res = await validator.ValidateAsync(user,token);

            if (!res.IsValid) return BadRequest(res.Errors);

            string userID = await _userService.GetUserIDWhere(u => u.UserName == user.UserName && u.Password == user.Password, token);

            return Ok(_jwtMethods.GenerateJWTToken(60, new Claim("Role", "User"), new Claim("ID", userID)));

        }
    }

}
