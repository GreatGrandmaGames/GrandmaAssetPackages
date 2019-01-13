using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma.Core
{
    [RequireComponent(typeof(MoveController))]
    [RequireComponent(typeof(ZeroGMovement))]
    [RequireComponent(typeof(GroundMovement))]

    //this class is the component you add to your Player GameObject
    public class FPSInputController : MonoBehaviour
    {
        private MoveController mc;

        //I think this should be an enumerator
        private GroundMovement groundMovement;
        private ZeroGMovement zeroGMovement;

        private void Awake()
        {
            mc = GetComponent<MoveController>();
            groundMovement = GetComponent<GroundMovement>();
            zeroGMovement = GetComponent<ZeroGMovement>();

            //setup the move controller properly
            mc.AddMode(groundMovement);
            mc.AddMode(zeroGMovement);

            mc.SwitchMode(groundMovement);

        }
        private void FixedUpdate()
        {
            SwitchMovement();
        }

        private void SwitchMovement()
        {
            if (Input.GetKeyDown("Switch"))
            {
                if (mc.active == groundMovement)
                {
                    mc.SwitchMode(zeroGMovement);
                }
                if (mc.active == zeroGMovement)
                {
                    mc.SwitchMode(groundMovement);
                }
            }
        }
    }
}
