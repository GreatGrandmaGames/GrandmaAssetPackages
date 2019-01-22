using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma
{
    public class CoroutineDispatcher : MonoBehaviour
    {
        private static CoroutineDispatcher instance;

        public static CoroutineDispatcher Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject();
                    go.name = "CoroutineDispatcher";
                    instance = go.AddComponent<CoroutineDispatcher>();
                }

                return instance;
            }
        }
    }
}