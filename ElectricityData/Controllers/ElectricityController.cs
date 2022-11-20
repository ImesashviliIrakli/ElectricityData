using ElectricityData.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ElectricityData.Controllers
{
    [Route("api/[controller]")]
    public class ElectricityController : Controller
    {
        #region Injection
        private readonly IElectricityRepository _repository;
        public ElectricityController(IElectricityRepository repository)
        {
            _repository = repository;
        }

        #endregion

        [HttpGet("get-may")]
        public async Task<IActionResult> GetMay()
        {
            var stream = await _repository.GetStreamMay();

            var result = await _repository.Add(stream);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpGet("get-april")]
        public async Task<IActionResult> GetApril()
        {
            var stream = await _repository.GetStreamApril();

            var result = await _repository.Add(stream);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpGet("get-march")]
        public async Task<IActionResult> GetMarch()
        {
            var stream = await _repository.GetStreamMarch();

            var result = await _repository.Add(stream);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpGet("get-february")]
        public async Task<IActionResult> GetFebruary()
        {
            var stream = await _repository.GetStreamFebruary();

            var result = await _repository.Add(stream);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }
    }
}
