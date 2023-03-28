using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyRecipes.Application.Features.Auth;
using MyRecipes.Application.Features.Auth.Login;
using MyRecipes.Application.Features.Auth.Register;
using MyRecipes.Application.Infrastructure.Identity;
using OneOf;

namespace MyRecipes.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public IdentityService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<OneOf<string, AuthenticationError>> RegisterAsync(RegisterDto registerDto)
    {
        if (await _userManager.Users.AnyAsync(x => x.Email.ToLower() == registerDto.Email.ToLower()))
        {
            return new AuthenticationError(AuthError.EmailAlreadyTaken, "The email has already been taken.");
        }

        var appUser = new ApplicationUser()
        {
            UserName = registerDto.Email,
            Email = registerDto.Email
        };

        var result = await _userManager.CreateAsync(appUser, registerDto.Password);

        if (result.Succeeded)
        {
            return appUser.Id;
        }

        // Log
        return new AuthenticationError(AuthError.Unknown, "An unexpected error occured during registration.");
    }

    public async Task<OneOf<string, AuthenticationError>> LoginAsync(LoginDto loginDto)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

        // The user not found in the database.
        if (user is null)
        {
            return new AuthenticationError(AuthError.InvalidCredentials, "Invalid credentials");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (result.Succeeded)
        {
            return user.Id;
        }

        return new AuthenticationError(AuthError.InvalidCredentials, "Invalid credentials");
    }
}