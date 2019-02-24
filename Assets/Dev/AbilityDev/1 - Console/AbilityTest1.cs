using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Grandma;

public class AbilityTest1 : Ability
{
    public override void Enter()
    {
        base.Enter();

        Debug.Log("AbilityTest1 : Enter");
    }

    public override void Activate()
    {
        Debug.Log("AbilityTest1 : Activate");
    }

    public override void Exit()
    {
        base.Exit();

        Debug.Log("AbilityTest1 : Exit");
    }

    public override bool WillActivate()
    {
        return Input.anyKey;
    }
}
