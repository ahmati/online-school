using OnlineSchool.Contract.Contacts;
using OnlineSchool.Contract.Courses;
using OnlineSchool.Contract.Payment;
using OnlineSchool.Contract.SpotMeeting;
using OnlineSchool.Core.Courses_;
using OnlineSchool.Core.SpotMeeting_.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineSchool.Core.Payment_.Service
{
    public class PurchasableItemFactory
    {
        public ICourseService CourseService { get; set; }
        public ISpotMeetingService SpotMeetingService { get; set; }
        public IPurchasableItem GetPurchasableItemService(PurchasableItemType purchasableItemType)
        {
            switch (purchasableItemType)
            {
                case PurchasableItemType.Course:
                    return CourseService;
                case PurchasableItemType.SpotMeeting:
                    return SpotMeetingService;
                default:
                    throw new ArgumentException($"The type {nameof(PurchasableItemType)} in input {purchasableItemType} is not managed.");
            }
        }
        
        /*public PurchasableItemModel GetPurchasableItemModel(PurchasableItemType purchasableItemType)
        {
            switch (purchasableItemType)
            {
                case PurchasableItemType.Course:
                    return new CourseModel();
                case PurchasableItemType.SpotMeeting:
                    return new CourseModel();
                default:
                    throw new ArgumentException($"The type {nameof(PurchasableItemType)} in input {purchasableItemType} is not managed.");
            }
        }*/
    }
}
