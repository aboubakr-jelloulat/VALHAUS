using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valhaus.Data.Data;
using Valhaus.Data.Repository.IRepository;
using Valhaus.Models.Models;

namespace Valhaus.Data.Repository.Repositories
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderHeader orderHeader)
        {
            this._db.OrderHeaders.Update(orderHeader);
        }

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            var orderFromDb = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);
            if (orderFromDb is not null)
            {
                orderFromDb.OrderStatus = orderStatus;
                if (!string.IsNullOrEmpty(paymentStatus))
                    orderFromDb.PaymentStatus = paymentStatus;
            }
        }

        public void UpdateStripePaymentID(int id, string sessionId, string PaymentIntentId)
        {
            var orderFromDb = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);
            if (!string.IsNullOrEmpty(sessionId))
                orderFromDb.SessionId = sessionId;

            if (!string.IsNullOrEmpty(PaymentIntentId))
            {
                orderFromDb.PaymentIntentId = PaymentIntentId;
                orderFromDb.PaymentDate = DateTime.Now;
            }

        }
    }
}
