using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;

namespace POS_For_Small_Shop.Services
{
    public static class Service
    {
        private static Dictionary<string, object> _singletons = new Dictionary<string, object>();

        public static void AddKeyedSingleton<IParent, Child>(string key) where Child : IParent, new()
        {
            _singletons[key] = new Child();
        }

        public static IParent GetKeyedSingleton<IParent>(string key)
        {
            if (_singletons.TryGetValue(key, out var instance))
            {
                return (IParent)instance;
            }

            throw new KeyNotFoundException($"No singleton registered with key '{key}'.");
        }
    }
}

