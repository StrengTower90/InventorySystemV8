using InventarySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarySystem.DataAccess.Repository.IRepository
{
     public interface IOrderRepository : IRepository<Order>
    {
        void Update(Order order);
        void UpdateStatus(int id, string orderStatus, string paymentStatus);
        void UpdatePaymentStripeId(int id, string sessionId, string transaccionId);
    }
}
