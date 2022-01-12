using Application.OrderServices;
using Common.Utils;
using Common.ViewModels.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace BECore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }

        [HttpPost("order")]
        public async Task<ServiceResponse> Order(OrderViewModel model)
        {
            return await _service.Order(model);
        }

        [HttpPut("reject")]
        public async Task<ServiceResponse> RejectOrder(Guid donHangId)
        {
            return await _service.RejectOrder(donHangId);
        }

        [HttpPut("receive")]
        public async Task<ServiceResponse> ReceiveOrder(Guid donHangId)
        {
            return await _service.ReceiveOrder(donHangId);
        }

        [HttpPut("transport")]
        public async Task<ServiceResponse> TransportOrder(Guid donHangId)
        {
            return await _service.TransportOrder(donHangId);
        }

        [HttpPut("finish")]
        public async Task<ServiceResponse> FinishOrder(Guid donHangId)
        {
            return await _service.FinishOrder(donHangId);
        }

        [HttpGet("get-all")]
        public async Task<ServiceResponse> GetAll([FromQuery] OrderRequest request)
        {
            return await _service.GetAll(request);
        }

        [HttpGet("get-all-for-user")]
        public async Task<ServiceResponse> GetAllForUser([FromQuery] OrderRequest request)
        {
            return await _service.GetAllForUser(request);
        }

        [HttpGet("detail")]
        public async Task<ServiceResponse> Detail(Guid donHangId)
        {
            return await _service.OrderDetail(donHangId);
        }
    }
}
