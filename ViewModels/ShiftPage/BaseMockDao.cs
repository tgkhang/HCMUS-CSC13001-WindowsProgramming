using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_For_Small_Shop.Services
{
    public class BaseMockRepository<T> : IRepository<T> where T : class, new()
    {
        protected List<T> _items = new List<T>();
        protected Func<T, int> GetIdFunc;
        protected Action<T, int> SetIdAction;

        public BaseMockRepository(Func<T, int> getIdFunc, Action<T, int> setIdAction)
        {
            GetIdFunc = getIdFunc;
            SetIdAction = setIdAction;
        }

        public virtual List<T> GetAll()
        {
            return _items.ToList(); // Return a copy
        }

        public virtual T GetById(int id)
        {
            return _items.FirstOrDefault(x => GetIdFunc(x) == id);
        }

        public virtual bool Insert(T item)
        {
            int newId = _items.Count > 0 ? _items.Max(x => GetIdFunc(x)) + 1 : 1;
            SetIdAction(item, newId);
            _items.Add(item);
            return true;
        }

        public virtual bool Update(int id, T item)
        {
            var existing = GetById(id);
            if (existing == null) return false;

            var index = _items.IndexOf(existing);
            _items[index] = item;
            return true;
        }

        public virtual bool Delete(int id)
        {
            var existing = GetById(id);
            if (existing == null) return false;

            _items.Remove(existing);
            return true;
        }
    }

}
