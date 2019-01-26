using System.Collections.Generic;
using UnityEngine;

namespace Grandma
{
    public class AbilityManager : MonoBehaviour
    {
        [SerializeField]
        public List<Ability> Abilities;

        private Ability curr;

        private void Update()
        {
            //Poll for input
            Abilities.ForEach(x =>
            {
                if (Input.GetKeyDown(x.enteringKey))
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
