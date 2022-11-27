using AutoMapper;
using Contracts;
using Enitites;
using Enitites.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Repositories;

namespace ElectricityData.Controllers
{
    [Route("api/[controller]")]
    public class ElectricityController : Controller
    {
        #region Injection
        private readonly IElectricityRepository _repository;
        private readonly IMapper _mapper;
        public ElectricityController(IElectricityRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
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
            List<AggregatedData> data = await _repository.GetAll();
            List<AggregatedDataViewModel> result = _mapper.Map<List<AggregatedDataViewModel>>(data);
            result.ForEach(x => x.Month = "2022-02, 2022-03, 2022-04, 2022-05");

            if (result == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
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
            if(month > 5 || month < 2)
            {
                return BadRequest("Wrong number for month");
            }

            List<AggregatedData> data = await _repository.GetByMonth(month);
            List<AggregatedDataViewModel> result = _mapper.Map<List<AggregatedDataViewModel>>(data);

            if (result == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(result);
        }
    }
}
