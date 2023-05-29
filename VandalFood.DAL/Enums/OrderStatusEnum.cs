using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VandalFood.DAL.Enums
{
    public enum OrderStatusEnum
    {
        AwaitingProcessing,
        Confirmed,
        AwaitingPreparation,
        AwaitingDelivery,
        Delivered,
        Completed,
        Rejected
    }
}
