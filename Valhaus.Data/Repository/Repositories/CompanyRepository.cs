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
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _db;

        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }



        public void Update(Company company)
        {
            _db.Companies.Update(company);
        }
    }
}
