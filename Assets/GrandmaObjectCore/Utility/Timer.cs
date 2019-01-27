using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Grandma 
{
    public class Timer
    {
        public float time;

        public bool IsCounting { get; private set; }

        /// <summary>
        /// Returns percentage complete
        /// </summary>
        public Action<float> OnCountingDown;
        public Action OnFinished;

        private Coroutine coroutine;

        public void Begin()
        {
            coroutine = CoroutineDispatcher.Instance.StartCoroutine(CoolDownCo());
        }

        public void Cancel()
        {
            if(coroutine != null)
            {
                CoroutineDispatcher.Instance.StopCoroutine(coroutine);
            }
        }

        IEnumerator CoolDownCo()
        {
            IsCounting = true;

            float timer = 0f;

            while(timer < time)
            {
                OnCountingDown?.Invoke(timer / time);

                timer += Time.deltaTime;

                yield return null;
            }

            IsCounting = false;

            OnFinished?.Invoke();
        }
    }
}
