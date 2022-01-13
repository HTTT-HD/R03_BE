using Common.Utils;
using Common.ViewModels.Order;

using System;
using System.Threading.Tasks;

namespace Application.OrderServices
{
    public interface IOrderService
    {
        Task<ServiceResponse> Order(OrderViewModel model);
        Task<ServiceResponse> RejectOrder(Guid donHangId);
        Task<ServiceResponse> UpdatePayment(UpdatePayment payment);
        Task<ServiceResponse> ReceiveOrder(Guid donHangId);
        Task<ServiceResponse> TransportOrder(Guid donHangId);
        Task<ServiceResponse> FinishOrder(Guid donHangId);
        Task<ServiceResponse> GetAll(OrderRequest request);
        Task<ServiceResponse> GetAllForUser(OrderRequest request);
        Task<ServiceResponse> OrderDetail(Guid donHangId);
        Task<ServiceResponse> Dashboard(string tuNgay, string denNgay);
    }
}
