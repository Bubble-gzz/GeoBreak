using UnityEngine;

namespace Game.Util
{
    public abstract class SingletonMonobehaviour<T> : MonoBehaviour where T : SingletonMonobehaviour<T>
    {
        protected static T instance;
        public static T Instance
        {
            get => instance;
            private set => instance = value;
        }

        protected void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = (T)this;
            OnSingletonRegistered();
            MyAwake();
        }

        protected virtual void OnSingletonRegistered() { }

        protected void OnDestroy()
        {
            if (instance == this) instance = null;
            MyDestroy();
        }

        protected virtual void MyAwake() { }
        protected virtual void MyDestroy() { }
    }
}
