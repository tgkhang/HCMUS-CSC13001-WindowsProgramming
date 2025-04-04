using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_For_Small_Shop.Data.Models;

namespace POS_For_Small_Shop.Services.Repository
{
    public class PostgresCustomerRepository : IRepository<Customer>
    {
        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Customer> GetAll()
        {
            throw new NotImplementedException();
        }

        public Customer GetById(int id)
        {
            throw new NotImplementedException();
        }

        public bool Insert(Customer item)
        {
            throw new NotImplementedException();
        }

        public bool Update(int id, Customer item)
        {
            throw new NotImplementedException();
        }
    }
}