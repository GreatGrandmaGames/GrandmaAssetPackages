using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grandma;

public class JumpAbility : Ability
{
    // Start is called before the first frame update
    public Rigidbody rb;
    public float jumpForce;
    protected override void Awake()
    {
        base.Awake();
        //rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    public override bool WillActivate()
    {
        return base.WillActivate() && Input.GetButtonDown(enteringKey);
    }

    public void Jump(Rigidbody rb)
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(new Vector3(0f, jumpForce, 0f), ForceMode.Impulse);
    }

    public override void Activate()
    {
        Jump(rb);
    }
}
