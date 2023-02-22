using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyRecipes.Application.Common.Models;
using MyRecipes.Application.Features.Auth;
using MyRecipes.Application.Features.Auth.Login;
using MyRecipes.Application.Features.Auth.Register;
using MyRecipes.Application.Infrastructure.Identity;
using System.Security.Claims;

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

    public async Task<Result<string, AuthError>> RegisterAsync(RegisterDto registerDto)
    {
        if (await _userManager.Users.AnyAsync(x => x.Email.ToLower() == registerDto.Email.ToLower()))
        {
            return Result<string, AuthError>.Failure("The email has already been taken.", AuthError.EmailAlreadyTaken);
        }

        var appUser = new ApplicationUser()
        {
            UserName = registerDto.Email,
            Email = registerDto.Email
        };

        var result = await _userManager.CreateAsync(appUser, registerDto.Password);

        if (result.Succeeded)
        {
            return Result<string, AuthError>.Success(appUser.Id);
        }

        return Result<string, AuthError>.Failure("An unexpected error occured during registration.", AuthError.Unknown);
    }

    public async Task<Result<string, AuthError>> LoginAsync(LoginDto loginDto)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

        // The user not found in the database.
        if (user is null)
        {
            return Result<string, AuthError>.Failure("Invalid credentials", AuthError.InvalidCredentials);
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (result.Succeeded)
        {
            return Result<string, AuthError>.Success(user.Id);
        }

        return Result<string, AuthError>.Failure("Invalid credentials", AuthError.InvalidCredentials);
    }
}

//    public async Task<Result<string>> GetCurrentUserAsync(ClaimsPrincipal user)
//    {
//        var currentUser = await _userManager.Users
//            .FirstOrDefaultAsync(u => u.Email == user.FindFirstValue(ClaimTypes.Email));

//        if (currentUser is null)
//        {
//            return Result<string>.Failure("No user found");
//        }

//        return Result<string>.Success("Ok");

//    }
//}