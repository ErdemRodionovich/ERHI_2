using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BER_ERHI_c223901b45f74af0a160b6a254574b90
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {

        private static T instance;
        private static bool shuttingDown = false;
        private static object mLock = new object();

        public static T Instance
        {
            get
            {
                if (shuttingDown)
                {
                    Debug.LogError("[Singleton] Instance of " + typeof(T) +
                        " already destroyed. Returning null.");
                    return null;
                }

                if (instance == null)
                {
                    initInstance();
                }

                return instance;

            }
        }

        public static bool IsInitialized
        {
            get { return instance != null; }
        }

        private static void initInstance()
        {
            if (instance == null)
            {
                lock (mLock)
                {
                    instance = (T)FindObjectOfType<T>();
                    if (instance == null)
                    {
                        var singletonObject = new GameObject();
                        instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString() + " (Singleton)";

                        DontDestroyOnLoad(singletonObject);
                    }
                }
            }
        }

        protected virtual void Awake()
        {
            if (instance != null
                && instance != this)
            {
                Debug.LogError("[Singleton] Trying instantiate second object of singleton class " + typeof(T));
                Destroy(gameObject);
            }
            else if (instance == null)
            {
                initInstance();
            }
        }

        protected virtual void OnDestroy()
        {
            if (instance == this)
            {
                shuttingDown = true;
                instance = null;
            }
        }

        protected virtual void OnApplicationQuit()
        {
            shuttingDown = true;
        }

    }
}