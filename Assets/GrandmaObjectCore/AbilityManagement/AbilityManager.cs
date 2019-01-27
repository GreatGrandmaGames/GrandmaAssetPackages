using System.Collections.Generic;
using UnityEngine;

namespace Grandma
{
    public class AbilityManager : MonoBehaviour
    {
        public static AbilityManager Instance { get; private set; }

        [SerializeField]
        public List<Ability> Abilities;

        private Ability curr;

        public bool AbilityStaged
        {
            get
            {
                return curr != null;
            }
        }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("AbilityManager: AbilityManager is a Singleton, but more than one instance was found");
                Destroy(this);
                return;
            }

            Instance = this;
        }

        private void Update()
        {
            //Poll for input
            Abilities.ForEach(x =>
            {
                if (Input.GetButtonDown(x.enteringKey))
                {
                    //Ability switched without firing - cancel
                    if (curr != null)
                    {
                        curr.Exit();
                    }

                    if (x.CanEnter())
                    {
                        curr = x;

                        curr.Enter();
                    }
                }
            });

            if (curr != null)
            {
                if (curr.WillActivate())
                {
                    curr.Activate();
                    curr.Exit();
                    curr = null;
                }
            }
        }
    }
}
