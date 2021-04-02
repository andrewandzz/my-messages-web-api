using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyMessages.Api.Models;
using MyMessages.Logics.Infrastructure;
using MyMessages.Logics.Interfaces;
using System.Threading.Tasks;

namespace MyMessages.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService accountService;
        private readonly ITokenService tokenService;

        public AccountsController(IAccountService accountService, ITokenService tokenService)
        {
            this.accountService = accountService;
            this.tokenService = tokenService;
        }

        [HttpHead]
        [Route("verify-token")]
        [Authorize]
        public ActionResult IsLoggedIn()
        {
            // if the request was able to come into this method
            // then the user is authorized
            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<TokenModel>> Login([FromBody] LoginModel model)
        {
            try
            {
                var userDto = await accountService.AuthenticateAsync(model.Name, model.Password);

                var tokenModel = new TokenModel()
                {
                    Token = tokenService.GenerateNewIncludingUserId(userDto.Id)
                };

                return Ok(tokenModel);
            }
            catch (NotFoundException)
            {
                // not depending on whether the user is found or not
                // we are returning 401 Unauthorized
                return Unauthorized();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register([FromBody] RegisterModel model)
        {
            try
            {
                await accountService.RegisterAsync(model.Name, model.Password);

                return NoContent();
            }
            catch (ConflictException e)
            {
                return StatusCode(StatusCodes.Status409Conflict, e.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
