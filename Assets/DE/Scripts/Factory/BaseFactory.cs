using UnityEngine;
using Zenject;

namespace NPP.DE.Core.Factory
{

    public interface IFactoryMember { }

    public abstract class BaseFactory<T> : ScriptableObject, IFactory<T> where T : IFactoryMember
    {
        public virtual T Create()
        {
            return default;
        }
    }
}