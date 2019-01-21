using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma
{
    [RequireComponent(typeof(Moveable))]
    [RequireComponent(typeof(ZeroGMovement))]
    [RequireComponent(typeof(GroundMovement))]
    //this class is the component you add to your Player GameObject
    public class FPSInputController : MonoBehaviour
    {
        private Moveable mc;

        //I think this should be an enumerator
        private GroundMovement groundMovement;
        private ZeroGMovement zeroGMovement;

        private void Awake()
        {
            mc = GetComponent<Moveable>();
            groundMovement = GetComponent<GroundMovement>();
            zeroGMovement = GetComponent<ZeroGMovement>();

            if (groundMovement == null)
            {
                Debug.Log("GROUND MOVEMENT NULL");
            }

            //setup the move controller properly
            if (groundMovement != null && zeroGMovement != null)
            {
                mc.AllModes.Add(groundMovement);
                mc.AllModes.Add(zeroGMovement);

                mc.SwitchMode(groundMovement);
            }
            else
            {
                throw new System.Exception("FPSInputController: Awake - Components Null");
            }

        }
        private void FixedUpdate()
        {
            SwitchMovement();
        }

        private void SwitchMovement()
        {
            if (Input.GetKeyDown("Switch"))
            {
                mc.NextMode();
            }
        }
    }
}
