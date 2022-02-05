using HomeFinance.Domain.Utils;
using HomeFinanceApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HomeFinanceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletsController : ControllerBase
    {
        IGateway _unitOfWork;

        public WalletsController(IGateway unitOfWork)
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

        [HttpPost]
        [Authorize]
        public async Task Post(WalletRequest walletRequest)
        {
            var userId = User.Claims.First(i => i.Type == "UserId").Value;
            await _unitOfWork.WalletRepository.Add(walletRequest.ToDto(), userId);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task Put(int id, WalletRequest walletRequest)
        {
            var userId = User.Claims.First(i => i.Type == "UserId").Value;
            await _unitOfWork.WalletRepository.Update(walletRequest.ToDto(), userId);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task Delete(int id)
        {
            var userId = User.Claims.First(i => i.Type == "UserId").Value;
            await _unitOfWork.WalletRepository.Remove(id, userId);
        }
    }
}
