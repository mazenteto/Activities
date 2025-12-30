using System;
using System.Net.Http.Headers;
using System.Text;
using API.DTOs;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using static API.DTOs.GitHubInfo;

namespace API.Controllers;

public class AccountController(SignInManager<User> signInManager,
    IEmailSender<User> emailSender, IConfiguration config) : BaseApiController
{
    [AllowAnonymous]
    [HttpPost("github-login")]
    public async Task<ActionResult> LoginWithGitHub(string code)
    {
        if (string.IsNullOrEmpty(code))
            return BadRequest("Missing authorization code");
        using var httpclient = new HttpClient();
        httpclient.DefaultRequestHeaders.Accept
            .Add(new MediaTypeWithQualityHeaderValue("application/json"));

        // step 1 - exchange code for access token
        var tokenResponse = await httpclient.PostAsJsonAsync(config["Authentication:GitHub:LoginWithTokenUrl"],
            new GitHubAuthRequest
            {
                Code = code,
                ClientId = config["Authentication:GitHub:ClientId"]!,
                ClientSecret = config["Authentication:GitHub:ClientSecret"]!,
                RefirectUri = $"{config["ClientAppUrl"]!}/auth-callback",


            });

        if (!tokenResponse.IsSuccessStatusCode)
            return BadRequest("Failed to get access token");

        var tokencontent = await tokenResponse.Content.ReadFromJsonAsync<GitHubTokenResponse>();

        if (string.IsNullOrEmpty(tokencontent?.AccessToken))
            return BadRequest("Failed to retrieve access token");

        //step 2 - fetch user info from GitHub
        httpclient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", tokencontent.AccessToken);
        httpclient.DefaultRequestHeaders.UserAgent.ParseAdd("Activity");
        var userResponse = await httpclient.GetAsync(config["Authentication:GitHub:UserApi"]);

        if (!userResponse.IsSuccessStatusCode)
            return BadRequest("Faild to fetch user from GitHub");

        var user = await userResponse.Content.ReadFromJsonAsync<GitHubUser>();

        if (user == null)
            return BadRequest("Faild to read the user from GitHub");

        // step 3 - getting the email if needed

        if (string.IsNullOrEmpty(user?.Email))
        {
            var emailResponse = await httpclient.GetAsync(config["Authentication:GitHub:UserEmailApi"]);
            if (emailResponse.IsSuccessStatusCode)
            {
                var emails = await emailResponse.Content.ReadFromJsonAsync<List<GitHubEmail>>();

                var primary = emails?.FirstOrDefault(e => e is { primary: true, verified: true })?.Email;

                if (string.IsNullOrEmpty(primary))
                    return BadRequest("Faild to get email from GitHub");

                user!.Email = primary;
            }
        }

        //step 4 - finde or create user and sign in
        

        var existingUser= await signInManager.UserManager.FindByEmailAsync(user!.Email);
        
        if(existingUser==null)
        {
            existingUser=new User
            {
                Email=user.Email,
                UserName=user.Email,
                DisplayName=user.name,
                ImageUrl=user.ImagUrl

            };

            var createResult= await signInManager.UserManager.CreateAsync(existingUser);
            if(!createResult.Succeeded)
                return BadRequest("Faild to create user");
            
        }

        await signInManager.SignInAsync(existingUser,false);


        return Ok();

    }


    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDto registerDto)
    {
        var user = new User
        {
            UserName = registerDto.Email,
            Email = registerDto.Email,
            DisplayName = registerDto.DisplayName
        };
        var result = await signInManager.UserManager.CreateAsync(user, registerDto.Password);
        if (result.Succeeded)
        {
            await SendConfirmationEmailAsync(user, registerDto.Email);
            return Ok();
        }
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(error.Code, error.Description);
        }
        return ValidationProblem();
    }
    [AllowAnonymous]
    [HttpGet("resendConfirmEmail")]
    public async Task<ActionResult> ResendConfirmEmail(string? email, string userId)
    {
        if (string.IsNullOrEmpty(email) && string.IsNullOrEmpty(userId))
            return BadRequest("Email or Userid must be provided");

        var user = await signInManager.UserManager.Users
            .FirstOrDefaultAsync(x => x.Email == email || x.Id == userId);
        if (user == null || string.IsNullOrEmpty(user.Email))
            return BadRequest("Invalid email");
        await SendConfirmationEmailAsync(user, user.Email);
        return Ok();
    }



    private async Task SendConfirmationEmailAsync(User user, string email)
    {
        var code = await signInManager.UserManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        var confirmEmailUrl = $"{config["ClientAppUrl"]}/confirm-email?userId={user.Id}&code={code}";
        await emailSender.SendConfirmationLinkAsync(user, email, confirmEmailUrl);

    }

    [AllowAnonymous]
    [HttpGet("user-info")]
    public async Task<ActionResult> GetUserInfo()
    {
        if (User.Identity?.IsAuthenticated == false) return NoContent();
        var user = await signInManager.UserManager.GetUserAsync(User);
        if (user == null) return Unauthorized();
        return Ok(new
        {
            user.DisplayName,
            user.Email,
            user.Id,
            user.ImageUrl
        });
    }
    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return NoContent();
    }

    [HttpPost("change-password")]
    public async Task<ActionResult> ChabgePassword(ChangePasswordDto passwordDto)
    {

        var user = await signInManager.UserManager.GetUserAsync(User);

        if (user == null)
            return Unauthorized();

        var result = await signInManager.UserManager
            .ChangePasswordAsync(user, passwordDto.CurrentPassword, passwordDto.NewPassword);

        if (result.Succeeded) return Ok();

        return BadRequest(result.Errors.First().Description);
    }
}
