using UnityEngine;

namespace Manager
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject singleton;
                    singleton = GameObject.Find(typeof(T).Name);
                    if (singleton == null)
                    {
                        singleton = new GameObject(typeof(T).Name);
                        instance = singleton.AddComponent<T>();
                    }
                    else
                    {
                        instance = singleton.GetComponent<T>();
                    }
                }
                return instance;
            }
        }
    }
}
