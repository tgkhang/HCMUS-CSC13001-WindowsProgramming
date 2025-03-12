using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_For_Small_Shop.Data.Models;

namespace POS_For_Small_Shop.Services
{
    public class MockCategoryRepository : IRepository<Category>
    {
        //public List<Category> GetAll()
        //{
        //    return new List<Category>() {
        //            new Category() {ID = "1", Name = "MSI"},
        //            new Category() {ID = "2", Name = "Acer"},
        //            new Category() {ID = "3", Name = "Asus"},
        //            new Category() {ID = "4", Name = "HP"},
        //            new Category() {ID = "5", Name = "Apple"},
        //        };
        //}
        //}
        //public IRepository<Category> Categories { get; set; } = new MockCategoryRepository();
        public List<Category> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
