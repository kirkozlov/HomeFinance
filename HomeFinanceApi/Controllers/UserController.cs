using HomeFinance.Domain.Models;
using HomeFinanceApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeFinanceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        UserManager<HomeFinanceUser> _userManager;
        SignInManager<HomeFinanceUser> _signInManager;

        public UserController(UserManager<HomeFinanceUser> userManager, SignInManager<HomeFinanceUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            
        }

        [HttpPost]
        [Route("Register")]
        public async Task<Object> PostApplicationUser(UserViewModel userVM)
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

    }
}
