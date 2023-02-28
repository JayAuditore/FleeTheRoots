using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Roots.Utility
{
    public class BaseSingletonWithoutMono<T> where T : new()
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                    return instance;
                }
                return instance;
            }
        }
    }
}