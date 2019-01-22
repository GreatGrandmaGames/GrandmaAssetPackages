using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Grandma 
{
    public class CoolDown
    {
        public float time;

        public bool IsCooling { get; private set; }

        /// <summary>
        /// Returns percentage complete
        /// </summary>
        public Action<float> OnCoolingDown;
        public Action OnFinished;

        public void Begin()
        {
            CoroutineDispatcher.Instance.StartCoroutine(CoolDownCo());
        }

        IEnumerator CoolDownCo()
        {
            IsCooling = true;

            float timer = 0f;

            while(timer < time)
            {
                OnCoolingDown?.Invoke(timer / time);

                timer += Time.deltaTime;

                yield return null;
            }

            IsCooling = false;

            OnFinished?.Invoke();
        }
    }
}
