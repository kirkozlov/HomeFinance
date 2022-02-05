using HomeFinance.Domain.Utils;
using HomeFinanceApi.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HomeFinanceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        IGateway _unitOfWork;

        public CategoriesController(IGateway unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        // GET: api/<CategoriesController>
        [HttpGet]
        public async Task<IEnumerable<object>> Get()
        {
            var userId = User.Claims.First(i => i.Type == "UserId").Value;

            var categories = await _unitOfWork.CategoryRepository.GetAll(userId);

            return categories.Select(i => new {
                id = i.Id.Value,
                name = i.Name,
                operationType=i.OperationType,
                parentId= i.ParentId,
                comment = i.Comment
            });
        }

        //// GET api/<CategoriesController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        [HttpPost]
        [Authorize]
        public async Task Post(CategoryRequest categoryRequest)
        {
            var userId = User.Claims.First(i => i.Type == "UserId").Value;
            await _unitOfWork.CategoryRepository.Add(categoryRequest.ToDto(), userId);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task Put(int id, CategoryRequest categoryRequest)
        {
            var userId = User.Claims.First(i => i.Type == "UserId").Value;
            await _unitOfWork.CategoryRepository.Update(categoryRequest.ToDto(), userId);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task Delete(int id)
        {
            var userId = User.Claims.First(i => i.Type == "UserId").Value;
            await _unitOfWork.CategoryRepository.Remove(id, userId);
        }
    }
}
