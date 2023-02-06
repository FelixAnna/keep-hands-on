namespace EStore.SharedServices.Packages
{
    public interface IPackageService
    {
        Task<bool> DeliverOrderAsync(int orderId);
        Task<bool> ReceiveOrderAsync(int orderId);
    }
}
