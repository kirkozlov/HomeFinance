﻿using HomeFinance.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HomeFinance.Domain.Requests;

namespace HomeFinanceApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    UserManager<HomeFinanceUser> _userManager;
    SignInManager<HomeFinanceUser> _signInManager;
    IConfiguration _configuration;

    public UserController(UserManager<HomeFinanceUser> userManager, SignInManager<HomeFinanceUser> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
            
    }

    [HttpPost]
    [Route("Register")]
    public async Task<object> PostApplicationUser(UserRegistrationRequest userVM)
    {
        var user = new HomeFinanceUser()
        {
            UserName = userVM.UserName,
            Email = userVM.Email
        };
        try
        {
            var result = await _userManager.CreateAsync(user, userVM.Password);
            return Ok(result);
        }
        catch (Exception)
        {
            throw;
        }

    }
    
    private string GetTokenForUser(HomeFinanceUser user)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim("UserId", user.Id, ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(5),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["ApplicationSettings:JWT_Secret"])),
                SecurityAlgorithms.HmacSha256Signature
            )
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(securityToken);
    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login(UserLoginRequest userVM)
    {
        var user = await _userManager.FindByEmailAsync(userVM.UserNameOrEmail) ??
                   await _userManager.FindByNameAsync(userVM.UserNameOrEmail);

        if (user != null && await _userManager.CheckPasswordAsync(user, userVM.Password))
        {
            var token = GetTokenForUser(user);
            return Ok(new UserLoginResponse(){ Token=token });
        }

        return BadRequest(new { message = "Username or password is incorrect." });
    }

    [HttpPost]
    [Route("passwordchange")]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest changePasswordVM)
    {
        var user = await _userManager.FindByEmailAsync(changePasswordVM.UserNameOrEmail) ??
                   await _userManager.FindByNameAsync(changePasswordVM.UserNameOrEmail);

        if (user != null)
        {
            var result =
                await _userManager.ChangePasswordAsync(user, changePasswordVM.OldPassword,
                    changePasswordVM.NewPassword);

            if (result.Succeeded)
            {
                var token = GetTokenForUser(user);
                return Ok(new UserLoginResponse() { Token = token });
            }
        }
        
        return BadRequest(new { message = "Username or password is incorrect." });
    }

    
}