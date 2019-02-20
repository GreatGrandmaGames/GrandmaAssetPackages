using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grandma;

public class SwitchZeroGAbility : Ability
{
    // Start is called before the first frame update
    public Moveable moveable;

    [System.NonSerialized]
    SwitchZeroGAbilityData switchData;

    protected override void OnRead(GrandmaComponentData data)
    {
        base.OnRead(data);
        switchData = data as SwitchZeroGAbilityData;
    }

    public override bool WillActivate()
    {
        return base.WillActivate();
    }

    public override void Activate()
    {
        moveable.NextMode();
        Timer t = new Timer();
        t.time = switchData.activeTime;
        t.Begin();
        t.OnFinished += moveable.NextMode;
    }
}
