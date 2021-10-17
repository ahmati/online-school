using Microsoft.Extensions.Configuration;
using OnlineSchool.Contract.Infrastructure;
using OnlineSchool.Contract.Payment;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSchool.Contract.Contacts
{
    public interface IPurchasableItem
    {
        Task<PurchasableItemModel> GetPurchasableItemByIdAsync(int id);
        Task<ResponseBase<bool>> BuyAsync(string stripeToken, int id, string email);
        Task<ResponseBase<bool>> JoinAsync(int id, string email);
        Task<ResponseBase<bool>> GetExistingPurchasedItemAsync(string email, int id);
    }
}
