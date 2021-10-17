using OnlineSchool.Contract.Contacts;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Contract.Payment
{
    public abstract class PurchasableItemModel
    {
        public abstract int ItemId { get; }
        public abstract string ItemName { get; }
        public abstract double ItemPrice { get; }
        public abstract PurchasableItemType PurchasableItemType { get; }
    }
}
