using UnityEngine;

namespace Game.Util
{
    public abstract class PersistentSingletonMonobehaviour<T> : SingletonMonobehaviour<T>
        where T : PersistentSingletonMonobehaviour<T>
    {
        protected override void OnSingletonRegistered()
        {
            DontDestroyOnLoad(transform.root.gameObject);
        }
    }
}
