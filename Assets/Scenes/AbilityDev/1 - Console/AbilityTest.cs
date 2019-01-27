using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using Grandma;

public class AbilityTest : MonoBehaviour
{
    public AbilityManager am;

    private void Start()
    {
        am.Abilities.ForEach(x =>
        {
            x.CoolDown.OnCountingDown += (perc) =>
            {
                //Debug.Log(x.name + " is cooling down " + perc);
            };
        });
    }
}
