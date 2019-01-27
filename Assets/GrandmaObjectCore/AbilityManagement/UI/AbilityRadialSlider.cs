using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma
{
    public class AbilityRadialSlider : RadialSlider
    {
        public Ability ability;

        public void Start()
        {
            ability.CoolDown.OnCountingDown += SetValue;

            ability.CoolDown.OnFinished += () => { SetValue(1); };
        }
    }
}
