using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Grandma;

public class SwitchAbility : Ability
{
    public Positionable positionable;

    public override bool WillActivate()
    {
        //INPUT
        return base.WillActivate() && Input.GetMouseButtonDown(0) && positionable != null;
    }

    public override void Activate()
    {
        var other = ControlUtility2D.GetObjectUnderMouse()?.GetComponent<Positionable>();

        if (other != null && other != positionable)
        {
            var otherPos = other.transform.position;
            var otherRot = other.transform.rotation;
            //var otherLS = other.transform.localScale;

            other.transform.position = positionable.transform.position;
            other.transform.rotation = positionable.transform.rotation;
            //other.transform.localScale = positionable.transform.localScale;

            positionable.transform.position = otherPos;
            positionable.transform.rotation = otherRot;
            //positionable.transform.localScale = otherLS;
        }
    }
}
