using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Grandma;

public class AbilityTest2 : Ability
{
    public override void Enter()
    {
        base.Enter();

        Debug.Log("AbilityTest2 : Enter");
    }

    public override void Activate()
    {
        Debug.Log("AbilityTest2 : Activate");
    }

    public override void Exit()
    {
        base.Exit();

        Debug.Log("AbilityTest2 : Exit");
    }
}
