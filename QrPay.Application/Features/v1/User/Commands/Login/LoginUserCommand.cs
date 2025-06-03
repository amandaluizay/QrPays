using MediatR;
using Microsoft.EntityFrameworkCore;
using QrPay.Application.Requests;
using QrPay.Domain.Repository;
using QrPay.Shared.Interfaces;
using QrPay.Shared.Models;
using System.Text;
using userEntity = QrPay.Domain.Entities.User;


namespace QrPay.Application.Features.v1.User.Commands.Login
{
    public class LoginUserCommand : UserRequest, IRequest<IResponseResult>
    {
    }

    internal class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, IResponseResult>
    {
        private readonly ITokenService _tokenService;
        private readonly IRepository<userEntity> _userRepository;
        public LoginUserCommandHandler(ITokenService tokenService, IRepository<Domain.Entities.User> userRepository)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
        }
        public async Task<IResponseResult> Handle(LoginUserCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.Entities
                                            .SingleOrDefaultAsync(x => x.Email == command.Email &&
                                                                       x.UserName == command.Name &&
                                                                       x.PasswordHash == Encoding.ASCII.GetBytes(command.Password).ToString(),
                                                                  cancellationToken: cancellationToken);

            if(user == null)
            {
                return ResponseResult.Fail("Invalid Email or Passoword, please check and try again");
            }

            var token = await _tokenService.GetTokenAsync(user);

            if (token == null)
            {
                return ResponseResult.Fail("Failed to generate token");
            }

            return ResponseResult<string>.Sucess(token);

        }
    }
}
