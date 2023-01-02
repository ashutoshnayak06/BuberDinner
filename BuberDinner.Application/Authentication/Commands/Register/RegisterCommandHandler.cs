using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Entities;
using ErrorOr;
using MediatR;

namespace BuberDinner.Application.Authentication.Commands.Register;


public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public RegisterCommandHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
    {

        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }



    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        //1. Validate user doesnot exsist

        if (_userRepository.GetUserByEmail(command.Email) is not null)
        {
           return Domain.Common.Errors.Errors.User.DuplivateEmail;
        }
        //Create user (generate unique id) and persists to DB

            var user=new User
            {
                FirstName=command.FirstName,
                LastName=command.LastName,
                Email=command.Email,
                Password=command.Password
            };

            _userRepository.Add(user);

        // create jwt token

        var token =_jwtTokenGenerator.GenerateToken(user);
        return new AuthenticationResult(
            user,
            token
        );
    }
}