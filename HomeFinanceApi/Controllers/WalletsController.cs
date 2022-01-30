using HomeFinance.Domain.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HomeFinanceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletsController : ControllerBase
    {
        IUnitOfWork _unitOfWork;

        public WalletsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/<WalletsController>
        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<object>> Get()
        {
            var userId = User.Claims.First(i => i.Type == "UserId").Value;

            var wallets = await _unitOfWork.WalletRepository.GetAll(userId);

            return wallets;
        }

        
    }
}
