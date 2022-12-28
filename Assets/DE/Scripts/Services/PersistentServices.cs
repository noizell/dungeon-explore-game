using System.Collections.Generic;

namespace NPP.DE.Core.Services
{
    public interface IPersistent { }

    public class PersistentServices
    {

        private static Dictionary<string, IPersistent> _persistent;
        private static PersistentServices _current;

        public static PersistentServices Current
        {
            get
            {
                if (_current == null)
                    _current = new PersistentServices();

                return _current;
            }
        }

        private PersistentServices()
        {
            _persistent = new Dictionary<string, IPersistent>();
        }

        public void Register<T>(T persistent) where T : IPersistent
        {
            if (!_persistent.ContainsKey(typeof(T).Name))
            {
                _persistent.Add(typeof(T).Name, persistent);
            }
        }

        public void Unregister<T>(T persistent) where T : IPersistent
        {
            if (_persistent.ContainsKey(typeof(T).Name))
            {
                _persistent.Remove(typeof(T).Name);
            }
        }

        public T Get<T>() where T : IPersistent
        {
            if (_persistent.ContainsKey(typeof(T).Name))
            {
                return (T)_persistent[typeof(T).Name];
            }

            return default;
        }
    }
}
