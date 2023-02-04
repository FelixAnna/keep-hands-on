using EStore.Common.Entities;
using EStore.OrderAPI.ApplicationServices;
using EStore.SharedServices.Orders.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EStore.OrderAPI.Controllers
{
    [Authorize(Policy = "Customer")]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrdersApplicationService orderService;

        public OrdersController(OrdersApplicationService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet]
        public async Task<GetOrderListResponse> GetByUserIdAsync()
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value!;
            var response = await orderService.GetByUserIdAsync(userId);
            return response;
        }

        [HttpGet("{orderId}")]
        public async Task<GetOrderResponse> GetByIdAsync(int orderId)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value!;
            var response = await orderService.GetByIdAsync(userId, orderId);
            return response;
        }

        [HttpPut]
        public async Task<bool> AddAsync()
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value!;
            var response = await orderService.AddAsync(userId);
            return response;
        }

        [HttpPost("{orderId}/pay")]
        public async Task<bool> UpdateAsync(int orderId)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value!;
            var response = await orderService.PayOrderAsync(userId, orderId);
            return response;
        }

        [HttpPost("{orderId}/delivery")]
        public async Task<bool> DeliveryAsync(int orderId)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value!;
            var response = await orderService.DeliveryOrderAsync(userId, orderId);
            return response;
        }

        [HttpPost("{orderId}/receive")]
        public async Task<bool> ReceiveAsync(int orderId)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value!;
            var response = await orderService.ReceiveOrderAsync(userId, orderId);
            return response;
        }

        [HttpPost("{orderId}/finish")]
        public async Task<bool> FinishAsync(int orderId)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value!;
            var response = await orderService.FinishOrderAsync(userId, orderId);
            return response;
        }

        [HttpDelete("{orderId}")]
        public async Task<int> RemoveAsync(int orderId)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value!;
            var count = await orderService.RemoveAsync(userId, orderId);
            return count;
        }
    }
}
