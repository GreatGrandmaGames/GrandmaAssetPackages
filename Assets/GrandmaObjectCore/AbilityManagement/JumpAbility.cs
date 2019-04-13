using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grandma;
using System;

public class JumpAbility : Ability
{
    public Rigidbody rb;

    [NonSerialized]
    private JumpAbilityData jumpData;

    protected override void OnRead(GrandmaComponentData data)
    {
        base.OnRead(data);
        jumpData = Data as JumpAbilityData;
    }

    public void Jump(Rigidbody rb)
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(new Vector3(0f, jumpData.jumpForce, 0f), ForceMode.Impulse);
    }

    public override void Activate()
    {
        Jump(rb);
    }
}
