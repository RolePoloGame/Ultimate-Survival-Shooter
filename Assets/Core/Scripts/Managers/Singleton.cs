using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public abstract class Singleton : MonoBehaviour
    {

    }
    public class Singleton<T> : Singleton where T : Component
    {
        public virtual void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this as T;
        }
        private static T _instance;
        public static T Instance
        {
            get
            {
                return _instance;
            }
        }
    }
}
