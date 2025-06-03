using MediatR;
using Microsoft.AspNetCore.Mvc;
using QrPay.API.Controllers.Shared;
using QrPay.Application.Features.v1.User.Commands.Create;
using QrPay.Application.Features.v1.User.Commands.Login;
using QrPay.Shared.Interfaces;

namespace QrPay.API.Controllers
{
    public class UserController(IMediator mediator, IUserContext userContext) : ControllerBase<AccountController>(mediator)
    {
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        {
            return await CommandAsync(command);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserCommand command)
        {
            return await CommandAsync(command);
        }

    }
}
