using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valhaus.Data.Data;
using Valhaus.Data.Repository.IRepository;
using Valhaus.Models;
using Valhaus.Models.Models;

namespace Valhaus.Data.Repository.Repositories
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderDetailRepository(ApplicationDbContext db) : base(db)
        {
            this._db = db;
        }



        public void Update(OrderDetail OrderDetail)
        {
            this._db.OrderDetails.Update(OrderDetail);
        }
    }
}
