using back_end.Core.Responses;
using back_end.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace back_end.Controllers
{
    [Authorize(Roles = "ADMIN, EMPLOYEE")]
    [Route("api/[controller]")]
    [ApiController]
    public class BaoCaoThongKeController : ControllerBase
    {
        private readonly IBaoCaoThongKeService reportService;

        public BaoCaoThongKeController(IBaoCaoThongKeService reportService)
        {
            this.reportService = reportService;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<BaseResponse> GetReportData([FromQuery] string type, [FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            
            var response = await reportService.GetReportData(type, from, to);
            return response;
        }

        [HttpGet("don-hang/nam")]
        [ProducesResponseType(200)]
        public async Task<BaseResponse> GetOrderPercentInYear([FromQuery] int nam)
        {
            return await reportService.GetOrderPercentInRangeYear(nam);
        }

        [HttpGet("don-hang/thang")]
        [ProducesResponseType(200)]
        public async Task<BaseResponse> GetOrderByMonth([FromQuery] DateTime? thang)
        {
            if(!thang.HasValue)
                return await reportService.GetOrderByMonth(DateTime.Now);
            return await reportService.GetOrderByMonth(thang.Value);
        }

        [HttpGet("top-san-pham")]
        [ProducesResponseType(200)]
        public async Task<BaseResponse> GetTopFiveBestSellerProducts([FromQuery] DateTime? batDau, [FromQuery] DateTime? ketThuc)
        {
            if(!batDau.HasValue && !ketThuc.HasValue)
            {
                return await reportService.GetTopFiveBestSellerProducts(null, DateTime.Now);
            }

            return await reportService.GetTopFiveBestSellerProducts(batDau, ketThuc);
        }
    }
}
