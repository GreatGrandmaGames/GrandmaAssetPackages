using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using Grandma;

public class AbilityTest : MonoBehaviour
{
    public AbilityManager am;

    private void Start()
    {
        am.startingAbilities.ForEach(x =>
        {
            x.CoolDown.OnCoolingDown += (perc) =>
            {
                //Debug.Log(x.name + " is cooling down " + perc);
            };
        });
    }
}
