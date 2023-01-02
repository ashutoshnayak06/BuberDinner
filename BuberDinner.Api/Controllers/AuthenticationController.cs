using BuberDinner.Api.Filters;
using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Authentication.Queries.Login;
using BuberDinner.Application.Common.Errors;
using BuberDinner.Contracts.Authentication;
using ErrorOr;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace BuberDinner.Api.Controllers;


[Route("auth")]
//[ErrorHandlingFilter]
public class AuthenticationController:ApiController
{

    private readonly ISender _mediator;

    public AuthenticationController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = new RegisterCommand(request.FirstName, request.LastName, request.Email, request.Password);
        ErrorOr<AuthenticationResult> authResult =await _mediator.Send(command);
        return authResult.Match(
            authResult=> Ok(MapAuthResult(authResult)),
            error => Problem(error)
        );

        // if (registerResult.IsSuccess)
        // {
        //     return Ok(MapAuthResult(registerResult.Value));
        // }

        // var firstError=registerResult.Errors[0];
        // if (firstError is DuplicateEmailError)
        // {
        //     return Problem(statusCode:StatusCodes.Status409Conflict,detail:"Email already exist");
        // }
        // return Problem();
    }

    private static AuthenticationResponse MapAuthResult(AuthenticationResult authResult)
    {
        return new AuthenticationResponse(
             authResult.user.Id,
             authResult.user.FirstName,
             authResult.user.LastName,
             authResult.user.Email,
             authResult.Token
        );
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request){

        var query=new LoginQuery(request.Email,request.Password);
       var authResult=await _mediator.Send(query);
       
       return authResult.Match(
            authResult=> Ok(MapAuthResult(authResult)),
            error => Problem(error)
        );

    }
}