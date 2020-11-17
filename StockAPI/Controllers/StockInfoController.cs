using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonService.Models.StockInfoDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using StockAPI.Controllers.Services;

namespace StockAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockInfoController : ControllerBase
    {
        private readonly StockIDataService _stockdataService;

        public StockInfoController(StockIDataService stockdataService)
        {
            _stockdataService = stockdataService;
        }

        [HttpGet]
        public async Task<IEnumerable<StockIDModel>> Get()
        {
            return await _stockdataService.StockInfo_Get();
        }

        [HttpGet("{StockID}")]
        public async Task<IEnumerable<StockIDModel>> Get(string StockID)
        {
            return await _stockdataService.StockInfo_Get();
        }

        [HttpPut("{StockID}")]
        public async Task<ReturnModel> Put(string StockID, [FromBody]int status)
        {
            return await _stockdataService.StockID_Update(StockID,status);
        }
    }
}