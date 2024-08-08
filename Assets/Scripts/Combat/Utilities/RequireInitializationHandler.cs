
using UnityEngine;

namespace Vanaring 
{
    public abstract class RequireInitializationHandler<T, U, V>
    {
        private bool _initialize = false;
        protected bool IsInit => _initialize;
        protected bool SetInit(bool init) => _initialize = init;
        public abstract void Initialize(T argc, U argv, V argg);

        protected void ThrowInitException()
        {
            throw new System.Exception("RequireInitializationHandler hasn't been inited");
        }
    }
}