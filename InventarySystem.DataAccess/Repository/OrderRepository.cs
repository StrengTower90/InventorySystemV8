using InventarySystem.DataAccess.Data;
using InventarySystem.DataAccess.Repository.IRepository;
using InventarySystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventarySystem.DataAccess.Repository
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Order order)
        {
            _db.Update(order);
        }

        public void UpdateStatus(int id, string orderStatus, string paymentStatus)
        {
            var orderBD = _db.Orders.FirstOrDefault(o => o.Id == id);
            if(orderBD != null)
            {
                orderBD.OrderStatus = orderStatus;
                orderBD.PaymentStatus = paymentStatus;
            }
        }

        public void UpdatePaymentStripeId(int id, string sessionId, string transaccionId)
        {
            var orderBD = _db.Orders.FirstOrDefault(o => o.Id == id);
            if (orderBD != null)
            {
                if(!String.IsNullOrEmpty(sessionId))
                {
                    orderBD.SessionId = sessionId;
                }
                if(!String.IsNullOrEmpty(transaccionId))
                {
                    orderBD.TransactionId = transaccionId;
                    orderBD.PaymentDate = DateTime.Now;
                }
            }
        }

    }
}
