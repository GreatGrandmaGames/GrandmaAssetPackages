using System.Collections.Generic;
using UnityEngine;

namespace Grandma
{
    public class AbilityManager : MonoBehaviour
    {
        public List<Ability> startingAbilities;

        private List<Ability> abilities = new List<Ability>();

        private Ability curr;

        private void Awake()
        {
            if(startingAbilities != null)
            {
                abilities.AddRange(startingAbilities);
            }
        }

        private void Update()
        {
            //Poll for input
            abilities.ForEach(x =>
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
