using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QrPay.API.Controllers.Shared;
using QrPay.Application.Features.v1.Accounts.Commands.Create;
using QrPay.Application.Features.v1.Accounts.Commands.Pay;
using QrPay.Application.Features.v1.QrCode.Commands.Create;
using QrPay.Application.Features.v1.QrCode.Commands.Read;
using QrPay.Shared.Interfaces;

namespace QrPay.API.Controllers
{
    [Authorize]
    public class AccountController(IMediator mediator, IUserContext userContext) : ControllerBase<AccountController>(mediator)
    {
        private readonly IUserContext _userContext = userContext;

        [HttpPost("create")]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountCommand command)
        {
            return await CommandAsync(command with { UserId = _userContext.UserId.GetValueOrDefault()});
        }

        [HttpPost("pay")]
        public async Task<IActionResult> Pay([FromBody] PayCommand command)
        {
            return await CommandAsync(command);
        }

        [HttpPost("qrCode/generate")]
        public async Task<IActionResult> GenerateQrCode([FromBody] CreateQrPaymentCommand command)
        {
            return await CommandAsync(command with { SenderUserId = _userContext.UserId.GetValueOrDefault()});
        }

        [HttpPost("qrCode/read")]
        [DisableRequestSizeLimit, RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue, ValueLengthLimit = int.MaxValue)]
        public async Task<IActionResult> ReadQrCode(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("None file");

            byte[] fileBytes;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                fileBytes = memoryStream.ToArray();
            }

            return await CommandAsync(new ReadQrPaymentCommand
            {
                QrCode = fileBytes
            });
        }
    }
}
