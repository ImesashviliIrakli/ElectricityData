using Contracts;
using Enitites;
using Microsoft.AspNetCore.Mvc;
using Repositories;

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
        /// returns the sum of every record that has been added for each month. 
        /// It is grouped by TINKLAS.
        /// </summary>
        /// <returns>List of all sums</returns>
        [HttpGet("data")]
        public async Task<IActionResult> GetAllData()
        {
            List<AggregatedData> result = await _repository.GetAll();

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }

        /// <summary>
        /// Add either on of these in the months field:
        /// For FEB - 2,
        /// For March - 3,
        /// For APR - 4,
        /// For MAY - 5
        /// </summary>
        /// <param name="month" example="10763/2022-02"></param>
        /// <returns>List of sums of the chosen month,
        /// if the data of a current month has already been added, no worries,
        /// it won't add the same values, it will just return a list without adding it to the database.
        /// </returns>
        /// <response code="400">If the format of the parameter is not like the examples, the api will return 400 BadRequest</response>
        [HttpGet("data/{month}")]
        public async Task<IActionResult> GetData(int month)
        {
            List<AggregatedData> result = await _repository.GetByMonth(month);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(result);
        }
    }
}
