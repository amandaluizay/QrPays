using MediatR;
using Microsoft.EntityFrameworkCore;
using QrPay.Application.Requests;
using QrPay.Domain.Entities;
using QrPay.Domain.Repository;
using QrPay.Shared.Interfaces;
using QrPay.Shared.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QrPay.Application.Features.v1.QrCode.Commands.Create
{
    public record class CreateQrPaymentCommand : TransactionRequest, IRequest<IResponseResult>
    {
        [JsonIgnore]
        public Guid SenderUserId { get; set; }
    }

    public class CreateQrPaymentCommandHandler(IQrCodeService qrCodeService, IRepository<Account> accountRepository) : IRequestHandler<CreateQrPaymentCommand, IResponseResult>
    {
        private readonly IQrCodeService _qrCodeService = qrCodeService;
        private readonly IRepository<Account> _accountRepository = accountRepository;

        public async Task<IResponseResult> Handle(CreateQrPaymentCommand command, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.Entities.SingleOrDefaultAsync(x => x.UserId == command.SenderUserId, cancellationToken: cancellationToken);

            if (account == null)
            {
                return ResponseFileResult.Fail("Account not found");
            }

            command.ReceiverAccount = account.AccountNumber;
            command.ReceiverDocument = account.Document;
            command.ReceiverName = account.Name;

            var content = JsonSerializer.Serialize(command);

            var image = _qrCodeService.GenerateImage(content);

            return ResponseFileResult.Sucess(image);
        }
    }
}
