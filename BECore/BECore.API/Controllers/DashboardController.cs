using Application.OrderServices;
using Common.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BECore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IOrderService _service;

        public DashboardController(IOrderService service)
        {
            _service = service;
        }

        [HttpGet("dashboard")]
        public async Task<ServiceResponse> Dashboard(string tuNgay, string denNgay)
        {
            return await _service.Dashboard(tuNgay, denNgay);
        }
    }
}
