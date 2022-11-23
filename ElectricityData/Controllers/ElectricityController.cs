using Microsoft.AspNetCore.Mvc;
using Repositories.ElectricityRepositories;

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
        /// <summary>
        /// Add either on of these in the months field:
        /// For FEB - 10763/2022-02,
        /// For March - 10764/2022-03,
        /// For APR - 10765/2022-04,
        /// For MAY - 10766/2022-05
        /// </summary>
        /// <param name="month" example="10763/2022-02"></param>
        /// <returns>List of sums of the chosen month,
        /// if the data of a current month has already been added, no worries,
        /// it won't add the same values, it will just return a list without adding it to the database.
        /// </returns>
        /// <response code="400">If the format of the parameter is not like the examples, the api will return 400 BadRequest</response>
        [HttpPost("data")]
        public async Task<IActionResult> GetData(string month)
        {
            var stream = await _repository.GetStream(month);

            if (stream == null)
            {
                return BadRequest();
            }

            var result = await _repository.Add(stream);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        /// <summary>
        /// returns the sums of every record that has been added for each month, and it is grouped by TINKLAS
        /// </summary>
        /// <returns>List of all sums</returns>
        [HttpGet("get-all-sums")]
        public async Task<IActionResult> GetAllData()
        {
            var result = await _repository.GetFourMonthesSumData();

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }
    }
}
