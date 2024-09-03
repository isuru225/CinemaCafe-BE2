using MovieAppBackend.Frontend.Models;

namespace MovieAppBackend.IServices
{
    public interface IPaymentService
    {
        public Task<int> AddPurchaseInfos(PurchaseInfos purchaseInfos);
    }
}
