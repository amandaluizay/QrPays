using MediatR;
using Microsoft.EntityFrameworkCore;
using QrPay.Domain.Entities;
using QrPay.Domain.Repository;
using QrPay.Shared.Interfaces;
using QrPay.Shared.Models;
using System.Text.Json.Serialization;

namespace QrPay.Application.Features.v1.Accounts.Commands.Create
{
    public record class CreateAccountCommand : IRequest<IResponseResult>
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
        public string Document { get; set; }
        public string Name { get; set; }
    }

    public class CreateAccountCommandHandler(IRepository<Account> accountRepository) : IRequestHandler<CreateAccountCommand, IResponseResult>
    {
        private readonly IRepository<Account> _accountRepository = accountRepository;
        public async Task<IResponseResult> Handle(CreateAccountCommand command, CancellationToken cancellationToken)
        {
            var account = new Account
            {
                Document = command.Document,
                Name = command.Name,
                AccountNumber = Guid.NewGuid().ToString().Replace("-", ""),
                Balance = 0,
                UserId = command.UserId
            };

            var existingAccount = await _accountRepository.Entities.SingleOrDefaultAsync(x => x.UserId == command.UserId, cancellationToken: cancellationToken);

            if (existingAccount != null)
            {
                return ResponseResult.Fail("Account already exists");
            }

            await _accountRepository.AddAsync(account, cancellationToken);

            return ResponseResult.Sucess();
        }
    }
}
