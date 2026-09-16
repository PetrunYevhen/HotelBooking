using System.Security.Cryptography;
using System.Text;
using HotelBooking.API.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Users.Application.Auth;
using Users.Application.Auth.Login;
using Users.Application.Auth.Logout;
using Users.Application.Auth.Refresh;
using Users.Application.Auth.Register;
using Users.Application.Contracts;
using Users.Application.Query.GetUserById;
using Accommodations.Application.HotelierApplications;
using Accommodations.Application.Contracts;
using SharedKernel.Contracts;
using Users.Application.Services;
using Users.Domain.Entities;
using Users.Domain.Enums;

namespace HotelBooking.API.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class UsersController : ControllerBase
{
    private const string RefreshCookie = "hotelbooking_refresh";
    private const string CsrfCookie = "hotelbooking_csrf";
    private const string CsrfHeader = "X-CSRF-TOKEN";
    private readonly IUsersModule _users;
    private readonly IAccommodationsModule _accommodations;
    private readonly IJwtTokenService _jwtTokens;
    private readonly IWebHostEnvironment _environment;
    private readonly JwtSettings _jwtSettings;

    public UsersController(IUsersModule users, IJwtTokenService jwtTokens, IWebHostEnvironment environment, JwtSettings jwtSettings, IAccommodationsModule accommodations)
    {
        _users = users;
        _accommodations = accommodations;
        _jwtTokens = jwtTokens;
        _environment = environment;
        _jwtSettings = jwtSettings;
    }

    [HttpPost("signup")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> SignUp(SignUpRequest request, CancellationToken cancellationToken)
    {
        var result = await _users.ExecuteCommandAsync(new RegisterUserCommand
        {
            Username = request.Username,
            Password = request.Password,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            Intent = request.Intent,
            Onboarding = request.BusinessApplication is { } details
                ? new HotelierApplicationDetails(details.LegalBusinessName, details.RegistrationNumber, details.TaxNumber,
                    details.BusinessEmail, details.BusinessPhoneNumber, details.FirstPropertyName, details.FirstPropertyAddress)
                : null
        }, cancellationToken);
        if (result.IsFailure)
            return this.ToProblem(result.Error);

        return Created(string.Empty, CreateAuthenticatedResponse(result.Value));
    }

    [HttpPost("signin")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> SignIn(SignInRequest request, CancellationToken cancellationToken)
    {
        var result = await _users.ExecuteCommandAsync(new LoginCommand
        {
            UsernameOrEmail = request.UsernameOrEmail,
            Password = request.Password
        }, cancellationToken);

        if (result.IsFailure)
            return Unauthorized(new { message = "Invalid credentials" });

        return Ok(CreateAuthenticatedResponse(result.Value));
    }

    [HttpPost("refresh")]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh(CancellationToken cancellationToken)
    {
        if (!HasValidCsrfToken())
            return BadRequest(new { message = "CSRF token validation failed." });

        Request.Cookies.TryGetValue(RefreshCookie, out var refreshToken);
        var result = await _users.ExecuteCommandAsync(new RefreshTokenCommand { RefreshToken = refreshToken ?? string.Empty }, cancellationToken);
        if (result.IsFailure)
        {
            DeleteSessionCookies();
            return Unauthorized(new { message = "Refresh token is invalid or expired." });
        }

        return Ok(CreateAuthenticatedResponse(result.Value));
    }

    [HttpPost("logout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        if (!HasValidCsrfToken())
            return BadRequest(new { message = "CSRF token validation failed." });

        Request.Cookies.TryGetValue(RefreshCookie, out var refreshToken);
        await _users.ExecuteCommandAsync(new LogoutCommand { RefreshToken = refreshToken ?? string.Empty }, cancellationToken);
        DeleteSessionCookies();
        return NoContent();
    }

    [Authorize]
    [HttpGet("me")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Me(CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var userId))
            return Unauthorized();

        var user = await _users.ExecuteQueryAsync(new GetUserByIdQuery(new UserId(userId)), cancellationToken);
        return user is null ? Unauthorized() : Ok(user);
    }

    [Authorize]
    [HttpGet("hotelier-application")]
    public async Task<IActionResult> GetHotelierApplication(CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var userId)) return Unauthorized();
        return Ok(await _accommodations.ExecuteQueryAsync(new GetMyHotelierApplicationQuery(new AccountId(userId)), cancellationToken));
    }

    [Authorize]
    [HttpPost("hotelier-application")]
    public async Task<IActionResult> SubmitHotelierApplication(BusinessApplicationRequest request, CancellationToken cancellationToken)
    {
        if (!TryGetCurrentUserId(out var userId)) return Unauthorized();
        var result = await _accommodations.ExecuteCommandAsync(new SubmitHotelierApplicationCommand
        {
            ApplicantId = new AccountId(userId), LegalBusinessName = request.LegalBusinessName,
            RegistrationNumber = request.RegistrationNumber, TaxNumber = request.TaxNumber,
            BusinessEmail = request.BusinessEmail, BusinessPhoneNumber = request.BusinessPhoneNumber,
            FirstPropertyName = request.FirstPropertyName, FirstPropertyAddress = request.FirstPropertyAddress
        }, cancellationToken);
        return result.IsFailure ? this.ToProblem(result.Error) : StatusCode(StatusCodes.Status201Created);
    }

    private AuthResponse CreateAuthenticatedResponse(AuthSession session)
    {
        SetRefreshCookie(session.RefreshToken);
        var csrfToken = SetCsrfCookie();
        return new AuthResponse(
            _jwtTokens.CreateAccessToken(session.UserId, session.Username, Enum.Parse<Role>(session.Role)),
            "Bearer",
            _jwtSettings.AccessTokenMinutes * 60,
            csrfToken);
    }

    private void SetRefreshCookie(string token) => Response.Cookies.Append(RefreshCookie, token, new CookieOptions
    {
        HttpOnly = true,
        Secure = !_environment.IsDevelopment(),
        SameSite = SameSiteMode.Strict,
        Path = "/api/auth",
        Expires = DateTimeOffset.UtcNow.AddDays(30)
    });

    private string SetCsrfCookie()
    {
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        Response.Cookies.Append(CsrfCookie, token, new CookieOptions
        {
            HttpOnly = false,
            Secure = !_environment.IsDevelopment(),
            SameSite = SameSiteMode.Strict,
            Path = "/",
            Expires = DateTimeOffset.UtcNow.AddDays(30)
        });
        return token;
    }

    private bool HasValidCsrfToken()
    {
        if (!Request.Cookies.TryGetValue(CsrfCookie, out var cookieToken) ||
            !Request.Headers.TryGetValue(CsrfHeader, out var headerToken) || string.IsNullOrWhiteSpace(cookieToken))
            return false;

        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(cookieToken),
            Encoding.UTF8.GetBytes(headerToken.ToString()));
    }

    private void DeleteSessionCookies()
    {
        Response.Cookies.Delete(RefreshCookie, new CookieOptions { Path = "/api/auth", Secure = !_environment.IsDevelopment(), SameSite = SameSiteMode.Strict });
        Response.Cookies.Delete(CsrfCookie, new CookieOptions { Path = "/", Secure = !_environment.IsDevelopment(), SameSite = SameSiteMode.Strict });
    }

    private bool TryGetCurrentUserId(out Guid userId) =>
        Guid.TryParse(User.FindFirst("sub")?.Value, out userId);
}

public sealed record SignUpRequest(string Username, string Password, string Email, string FirstName, string LastName, string PhoneNumber, string Intent = "guest", BusinessApplicationRequest? BusinessApplication = null);
public sealed record BusinessApplicationRequest(string LegalBusinessName, string RegistrationNumber, string? TaxNumber, string BusinessEmail, string BusinessPhoneNumber, string FirstPropertyName, string FirstPropertyAddress);
public sealed record SignInRequest(string UsernameOrEmail, string Password);
public sealed record AuthResponse(string AccessToken, string TokenType, int ExpiresIn, string CsrfToken);
