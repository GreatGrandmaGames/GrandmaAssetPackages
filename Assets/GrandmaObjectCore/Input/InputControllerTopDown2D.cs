using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grandma;

[RequireComponent(typeof(Moveable))]
[RequireComponent(typeof(GroundMovement2D))]
public class InputControllerTopDown2D : MonoBehaviour
{
    private Moveable moveable;

    private GroundMovement2D groundMovement2D;
    private void Awake()
    {
        moveable = GetComponent<Moveable>();

        groundMovement2D = GetComponent<GroundMovement2D>();

        if (groundMovement2D == null)
        {
            Debug.Log("GROUND MOVEMENT NULL");
        }

        if (groundMovement2D != null)
        {
            groundMovement2D.Activate();
            moveable.AllModes.Add(groundMovement2D);
            moveable.SwitchMode(groundMovement2D);
        }
        else
        {
            throw new System.Exception("FPSInputController: Awake - Components Null");
        }
    }
}
