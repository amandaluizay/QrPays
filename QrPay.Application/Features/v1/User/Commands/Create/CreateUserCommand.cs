using MediatR;
using Microsoft.EntityFrameworkCore;
using QrPay.Application.Requests;
using QrPay.Domain.Repository;
using QrPay.Shared.Configuration;
using QrPay.Shared.Interfaces;
using QrPay.Shared.Models;
using System.Text;
using userEntity = QrPay.Domain.Entities.User;

namespace QrPay.Application.Features.v1.User.Commands.Create
{
    public class CreateUserCommand : UserRequest, IRequest<IResponseResult>
    {
    }

    internal class CreateUserCommandHandler(ITokenService tokenService, IRepository<userEntity> userRepository) : IRequestHandler<CreateUserCommand, IResponseResult>
    {
        private readonly ITokenService _userService = tokenService;
        public async Task<IResponseResult> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            var user = new userEntity
            {
                Id = Guid.NewGuid(),
                UserName = command.Name,
                Email = command.Email,
                PasswordHash = Encoding.ASCII.GetBytes(command.Password).ToString()
            };

            var existingEmail = await userRepository.Entities.SingleOrDefaultAsync(x => x.Email == command.Email, cancellationToken: cancellationToken);

            if(existingEmail != null)
            {
                return ResponseResult.Fail("User already exists");
                ;
            }


        }
    }
}
