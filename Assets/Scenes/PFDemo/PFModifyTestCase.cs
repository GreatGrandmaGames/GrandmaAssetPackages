using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Grandma;
using Grandma.ParametricFirearms;

public class PFModifyTestCase : MonoBehaviour
{
    public PFData mod1;
    public PFData mod2;
    public ParametricFirearm pf;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            pf.Data.Modify("Mod1", mod1, 2f, GCDModifierType.Addition);

            PrintMod();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            pf.Data.Modify("Mod2", mod2, 1f, GCDModifierType.Multiplication);

            PrintMod();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            pf.Data.RemoveModifier(mod1);
            pf.Data.RemoveModifier(mod2);

            PrintMod();
        }
    }

    private void PrintMod()
    {
        Debug.Log(pf.Data.Modified);
    }
}
