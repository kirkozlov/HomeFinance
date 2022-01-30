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

            var allOperations = (await _unitOfWork.OperationRepository.GetAll(userId)).ToList();

            return wallets.Select(i =>  new { 
                id=i.Id.Value,
                name=i.Name,
                groupName=i.GroupName,
                comment=i.Comment,
                balance = allOperations.GetSumFor(i.Id.Value) 
            });
        }

        
    }
}
