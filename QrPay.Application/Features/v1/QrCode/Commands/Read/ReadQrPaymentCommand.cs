using MediatR;
using Microsoft.EntityFrameworkCore;
using QrPay.Application.Requests;
using QrPay.Application.Responses;
using QrPay.Domain.Entities;
using QrPay.Domain.Repository;
using QrPay.Shared.Interfaces;
using QrPay.Shared.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QrPay.Application.Features.v1.QrCode.Commands.Read
{
    public class ReadQrPaymentCommand : IRequest<IResponseResult<TransactionResponse>>
    {
        public byte[]? QrCode { get; set; }

        [JsonIgnore]
        public Guid? UserId { get; set; }
    }

    public class ReadQrPaymentCommandHandler(IQrCodeService qrCodeService, IRepository<Account> accountRepository) : IRequestHandler<ReadQrPaymentCommand, IResponseResult<TransactionResponse>> 
    {
        private readonly IQrCodeService _qrCodeService = qrCodeService;
        private readonly IRepository<Account> _accountRepository = accountRepository;
        public async Task<IResponseResult<TransactionResponse>> Handle(ReadQrPaymentCommand command, CancellationToken cancellationToken)
        {
            var senderAccount = await _accountRepository.Entities.SingleOrDefaultAsync(x => x.UserId == command.UserId, cancellationToken: cancellationToken)
                ?? throw new Exception("Account not found");

            MemoryStream stream = new(command.QrCode);

            var qrContent = _qrCodeService.ReadImage(stream);

            var transaction = JsonSerializer.Deserialize<TransactionRequest>(qrContent);

            var response = new TransactionResponse() 
            {
                SenderAccount = senderAccount.AccountNumber,
                SenderDocument = senderAccount.Document,
                SenderName = senderAccount.Name,
                ReceiverAccount = transaction?.ReceiverAccount,
                ReceiverDocument = transaction?.ReceiverDocument,
                ReceiverName = transaction?.ReceiverName,
                Amount = transaction.Amount,
                Date = transaction?.Date
            };

            return ResponseResult<TransactionResponse>.Sucess(response);
        }
    }
}
