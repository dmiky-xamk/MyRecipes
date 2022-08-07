using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyRecipes.Application.Common.Enums;
using MyRecipes.Application.Common.Interfaces;
using MyRecipes.Application.Common.Models;
using MyRecipes.Application.Users;

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

    // TODO: username password email as strings?
    public async Task<IdentificationResult<IdentificationError>> RegisterAsync(RegisterDto registerDto)
    {
        // TODO: Move to Validator?
        if (await _userManager.Users.AnyAsync(x => x.UserName.ToLower() == registerDto.Username.ToLower()))
        {
            return IdentificationResult<IdentificationError>.Failure(
                IdentificationError.UsernameTaken,
                "username",
                "The username has already been taken.");
        }

        if (await _userManager.Users.AnyAsync(x => x.Email.ToLower() == registerDto.Email.ToLower()))
        {
            return IdentificationResult<IdentificationError>.Failure(
                IdentificationError.EmailTaken,
                "email",
                "The email has already been taken.");
        }

        var appUser = new ApplicationUser()
        {
            UserName = registerDto.Username,
            Email = registerDto.Email
        };

        var result = await _userManager.CreateAsync(appUser, registerDto.Password);

        if (result.Succeeded)
        {
            return IdentificationResult<IdentificationError>.Success(IdentificationError.None);
        }

        return IdentificationResult<IdentificationError>.Failure(
            IdentificationError.UnknownError,
            "An unexpected error occured during registration.");
    }

    public async Task<IdentificationResult<IdentificationError>> LoginAsync(LoginDto loginDto)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(x => x.UserName == loginDto.Username);

        // The user not found in the database.
        if (user is null)
        {
            return IdentificationResult<IdentificationError>.Failure(IdentificationError.UserNotFound);
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (result.Succeeded)
        {
            return IdentificationResult<IdentificationError>.Success(IdentificationError.None);
        }

        return IdentificationResult<IdentificationError>.Failure(IdentificationError.WrongCredentials);
    }
}