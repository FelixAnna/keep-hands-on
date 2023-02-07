namespace EStore.SharedServices.Packages
{
    public interface IPackService
    {
        Task<bool> DeliverOrderAsync(int orderId);
        Task<bool> ReceiveOrderAsync(int orderId);
    }
}
