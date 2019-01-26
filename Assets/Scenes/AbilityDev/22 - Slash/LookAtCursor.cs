using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grandma;

public class LookAtCursor : LookAt
{
    public override Vector3 GetTarget()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    public void FixedUpdate()
    {
        base.InstantRotation();
    }
}
