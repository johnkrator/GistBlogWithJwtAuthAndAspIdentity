using GistBlog.BLL.Services.Contracts;
using GistBlog.DAL.Configurations.EmailConfig.messages;
using GistBlog.DAL.Configurations.EmailConfig.services;
using GistBlog.DAL.Entities.DTOs;
using GistBlog.DAL.Entities.Models.UserEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Swashbuckle.AspNetCore.Annotations;

namespace GistBlog.API.Controllers;

[ApiController]
[Route("api/v1/")]
public class AuthorizationController : BaseController
{
    private readonly IAuthenticationService _authenticationService;
    private readonly IEmailSender _emailSender;
    private readonly ITokenService _jwtHandler;
    private readonly ILogger<AuthorizationController> _logger;
    private readonly UserManager<AppUser> _userManager;

    public AuthorizationController(IAuthenticationService authenticationService,
        ILogger<AuthorizationController> logger, UserManager<AppUser> userManager, IEmailSender emailSender,
        ITokenService jwtHandler) : base(logger)
    {
        _authenticationService = authenticationService;
        _logger = logger;
        _userManager = userManager;
        _emailSender = emailSender;
        _jwtHandler = jwtHandler;
    }

    [SwaggerOperation(Summary = "Register")]
    [HttpPost("user-registration")]
    public async Task<IActionResult> Register([FromBody] RegistrationDto model)
    {
        var scheme = Request.Scheme;
        var status = await _authenticationService.SignupAsync(model, scheme);

        if (status == null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return CreatedAtAction(nameof(Register), status);
    }

    [SwaggerOperation(Summary = "Login")]
    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginDto model)
    {
        var userLogin = await _authenticationService.LoginAsync(model);

        if (userLogin == null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(userLogin);
    }

    [SwaggerOperation(Summary = "ConfirmEmail")]
    [HttpPost("confirm-email")]
    public async Task<ActionResult> ConfirmEmail([FromBody] string token, string email)
    {
        var response = await _authenticationService.ConfirmEmail(token, email);

        if (response == null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(response);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(string username)
    {
        var loggedOutUser = await _authenticationService.LogoutAsync(username);

        if (loggedOutUser == null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(loggedOutUser);
    }

    #region other auths

    [HttpPost("login-status")]
    public async Task<IActionResult> LoginStatusAsync(string username)
    {
        var loginStatus = await _authenticationService.LoginStatusAsync(username);

        if (loginStatus == null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(loginStatus);
    }

    [SwaggerOperation(Summary = "ChangePassword")]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
    {
        var changePassword = await _authenticationService.ChangePasswordAsync(model);

        if (changePassword == null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(changePassword);
    }

    [SwaggerOperation(Summary = "AdminRegistration")]
    [HttpPost("admin-registration")]
    public async Task<IActionResult> AdminRegistration([FromBody] RegistrationDto model)
    {
        var newAdminRegistration = await _authenticationService.AdminRegistrationAsync(model);

        if (newAdminRegistration == null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return CreatedAtAction(nameof(AdminRegistration), newAdminRegistration);
    }

    [HttpPost("create-roles")]
    public async Task<IActionResult> CreateRoles([FromBody] List<string> roles)
    {
        var createdRoles = await _authenticationService.CreateRolesAsync(roles);

        if (createdRoles is null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(createdRoles);
    }

    [HttpPost("assign-roles")]
    public async Task<IActionResult> AssignRoles([FromBody] List<string> roles, string username)
    {
        var assignedRoles = await _authenticationService.AssignRolesAsync(roles, username);

        if (assignedRoles is null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(assignedRoles);
    }

    [HttpPut("edit-role")]
    public async Task<IActionResult> EditRoleAsync([FromBody] EditRoleDto model)
    {
        var editRoleAsync = await _authenticationService.EditRoleAsync(model);

        if (editRoleAsync is null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(editRoleAsync);
    }

    [HttpDelete("delete-role")]
    public async Task<IActionResult> DeleteRoleAsync(string roleName)
    {
        var deleteRoleAsync = await _authenticationService.DeleteRoleAsync(roleName);

        if (deleteRoleAsync is null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(deleteRoleAsync);
    }

    [HttpPut("edit-user-role")]
    public async Task<IActionResult> EditUserRoleAsync([FromBody] EditUserRoleDto model)
    {
        var editUserRoleDto = await _authenticationService.EditUserRoleAsync(model);

        if (editUserRoleDto is null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(editUserRoleDto);
    }

    [HttpDelete("delete-user-role")]
    public async Task<IActionResult> DeleteUserRoleAsync([FromBody] DeleteUserRoleDto model)
    {
        var deleteUserRoleAsync = await _authenticationService.DeleteUserRoleAsync(model);

        if (deleteUserRoleAsync is null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(deleteUserRoleAsync);
    }

    [HttpGet("get-all-roles")]
    public async Task<IActionResult> GetAllRolesAsync()
    {
        var getRoles = await _authenticationService.GetAllRolesAsync();

        if (getRoles is null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(getRoles);
    }

    [HttpGet("get-user-roles")]
    public async Task<IActionResult> GetUserRolesAsync(string username)
    {
        var getUserRoles = await _authenticationService.GetUserRolesAsync(username);

        if (getUserRoles is null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(getUserRoles);
    }

    [HttpGet("get-all-users-and-roles")]
    public async Task<IActionResult> GetAllUsersAndRolesAsync()
    {
        var getAllUsersRoles = await _authenticationService.GetAllUsersAndRolesAsync();

        if (getAllUsersRoles is null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(getAllUsersRoles);
    }

    [HttpDelete("delete-user")]
    public async Task<IActionResult> DeleteUserAsync(string username)
    {
        var deleteUser = await _authenticationService.DeleteUserAsync(username);

        if (deleteUser is null)
            return StatusCode(StatusCodes.Status400BadRequest);

        return Ok(deleteUser);
    }

    [HttpGet("get-all-users")]
    public async Task<IActionResult> GetAllUsersAsync()
    {
        var getAllUsersAsync = await _authenticationService.GetAllUsersAsync();

        if (getAllUsersAsync is null)
            return StatusCode(StatusCodes.Status404NotFound);

        return Ok(getAllUsersAsync);
    }

    [HttpPost("forgotPassword")]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var token = await _authenticationService.GeneratePasswordResetTokenAsync(new ForgotPasswordResetDto
        {
            Email = forgotPasswordDto.Email!
        });

        if (token == null)
            return BadRequest("Invalid Request");

        var resetDto = new ForgotPasswordResetDto
        {
            Email = forgotPasswordDto.Email!,
            CallbackUrl = Url.Action(nameof(ForgotPassword), "Authorization",
                new { token, email = forgotPasswordDto.Email })!
        };

        var isEmailSent = await _authenticationService.SendForgotPasswordEmailAsync(resetDto);
        if (!isEmailSent)
            return BadRequest("Failed to send email");

        return Ok($"Use The Generated Token Below To Reset Password:\n\n {token}");
    }

    [HttpPost("resetPassword")]
    public async Task<IActionResult> ResetPassword(ResetPasswordModelDto resetPasswordModelDto)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var resetInfoDto = new PasswordResetInfoDto
        {
            Email = resetPasswordModelDto.Email,
            Token = resetPasswordModelDto.Token,
            NewPassword = resetPasswordModelDto.Password
        };

        var isSuccess = await _authenticationService.ResetPasswordAsync(resetInfoDto);

        if (!isSuccess)
            return BadRequest("Invalid Request or Failed to Reset Password");

        return Ok("Password Reset Was Successful!");
    }

    #endregion
}