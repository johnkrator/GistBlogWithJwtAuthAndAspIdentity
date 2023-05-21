using GistBlog.BLL.Services.Contracts;
using GistBlog.DAL.Entities.DTOs;
using GistBlog.DAL.Entities.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GistBlog.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthorizationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthorizationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [SwaggerOperation(Summary = "Register")]
    [HttpPost("UserRegistration")]
    public async Task<IActionResult> Register([FromBody] RegistrationDto model)
    {
        var status = await _authenticationService.Register(model);

        if (status == null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return CreatedAtAction(nameof(Register), status);
    }

    [SwaggerOperation(Summary = "Login")]
    [HttpPost("Login")]
    public async Task<ActionResult> Login([FromBody] LoginDto model)
    {
        var userLogin = await _authenticationService.Login(model);

        if (userLogin == null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(userLogin);
    }

    [SwaggerOperation(Summary = "ChangePassword")]
    [HttpPost("ChangePassword")]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
    {
        var changePassword = await _authenticationService.ChangePassword(model);

        if (changePassword == null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(changePassword);
    }

    [SwaggerOperation(Summary = "AdminRegistration")]
    [HttpPost("AdminRegistration")]
    public async Task<IActionResult> AdminRegistration([FromBody] RegistrationDto model)
    {
        var newAdminRegistration = await _authenticationService.AdminRegistration(model);

        if (newAdminRegistration == null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return CreatedAtAction(nameof(AdminRegistration), newAdminRegistration);
    }

    // public async Task<IActionResult> SendVerificationEmail(string email, string verificationToken)
    // {
    // }
    [SwaggerOperation(Summary = "VerifyUser")]
    [HttpGet("VerifyUser")]
    public async Task<IActionResult> VerifyUser([FromQuery] string email, [FromQuery] string verificationToken)
    {
        var isVerified = await _authenticationService.VerifyUser(email, verificationToken);

        return Ok(isVerified);
    }
}
