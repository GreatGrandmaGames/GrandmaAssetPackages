using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Grandma;
using Grandma.ParametricFirearms;

public class PFAIDemo : MonoBehaviour
{
    public PFAI ai;
    public Damageable demoTarget;

    private void Start()
    {
        ai.SetTarget(demoTarget);
    }

    private void Update()
    {
        /*
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (ai.GetComponent<Moveable>().Active is MoveToVector moveTo)
                {
                    moveTo.Target = hit.point;
                }
            }
        }
        */


    }
}
