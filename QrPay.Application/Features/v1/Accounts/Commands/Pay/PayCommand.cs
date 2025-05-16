using MediatR;
using Microsoft.EntityFrameworkCore;
using QrPay.Application.Responses;
using QrPay.Domain.Entities;
using QrPay.Domain.Enums;
using QrPay.Domain.Repository;
using QrPay.Shared.Interfaces;
using QrPay.Shared.Models;

namespace QrPay.Application.Features.v1.Accounts.Commands.Pay
{
    public class PayCommand : TransactionResponse, IRequest<IResponseResult>
    {
    }

    internal class PayCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<PayCommand, IResponseResult>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<IResponseResult> Handle(PayCommand request, CancellationToken cancellationToken)
        {
            var accountRepository = _unitOfWork.Repository<Account>();
            var transactionRepository = _unitOfWork.Repository<Transaction>();

            var senderAccount = await accountRepository.Entities.SingleOrDefaultAsync(x => x.AccountNumber == request.SenderAccount, cancellationToken: cancellationToken);

            if (senderAccount == null)
            {
                return ResponseResult.Fail("Sender account not found");
            }

            if (senderAccount.Balance < request.Amount)
            {
                return ResponseResult.Fail("Insufficient balance");
            }

            var receiverAccount = await accountRepository.Entities.SingleOrDefaultAsync(x => x.AccountNumber == request.ReceiverAccount, cancellationToken: cancellationToken);

            if (receiverAccount == null)
            {
                return ResponseResult.Fail("Receiver account not found");
            }

            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
               senderAccount.Balance -= request.Amount;
               await accountRepository.UpdateAsync(senderAccount, cancellationToken);

               var senderTransaction = new Transaction
               {
                   AccountId = senderAccount.Id,
                   Amount = request.Amount,
                   Description = $"Payment to {request.ReceiverName}",
                   Type = ETransactionType.Debit,
                   TransactionDate = DateTime.UtcNow
               };

                senderAccount.Balance += request.Amount;
                await accountRepository.UpdateAsync(senderAccount, cancellationToken);

                var receiverTransaction = new Transaction
                {
                    AccountId = receiverAccount.Id,
                    Amount = request.Amount,
                    Description = $"Receivement from {request.SenderName}",
                    Type = ETransactionType.Credit,
                    TransactionDate = DateTime.UtcNow
                };

                await transactionRepository.AddRangeAsync([senderTransaction, receiverTransaction], cancellationToken);

                await _unitOfWork.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                return ResponseResult.Fail(ex.Message);
            }

            return ResponseResult.Sucess();
        }
    }
}
