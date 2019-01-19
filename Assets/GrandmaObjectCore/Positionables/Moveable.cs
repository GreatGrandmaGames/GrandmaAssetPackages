using System.Collections.Generic;
using UnityEngine;

namespace Grandma.Core
{
   
    public abstract class Moveable : Positionable
    {
        //MoveControllers can be set via inspector. Do not initialise a new list here
        //or the inspector fields will be wiped!
        [SerializeField]
        private List<MoveController> allModes;

        public List<MoveController> AllModes
        {
            get
            {
                if(allModes == null)
                {
                    allModes = new List<MoveController>();
                }

                return allModes;
            }
        }

        private MoveController active;

        //Can be null - no movement system will be active
        public void SwitchMode(MoveController switchTo)
        { 
            if (active != null)
            {
                active.Deactivate();
            }

            //Lazy add
            if (AllModes.Contains(switchTo) == false)
            {
                AllModes.Add(switchTo);
            }

            active = switchTo;

            if (active != null)
            {
                active.Activate();
            }
        }

        private void FixedUpdate()
        {
            if (active != null)
            {
                active.Move();
            }
        }

        public void NextMode()
        {
            if(AllModes.Count <= 0)
            {
                return;
            }

            int currIndex = allModes.FindIndex(x => x == active);
            SwitchMode(AllModes[(currIndex + 1) % AllModes.Count]);
        }
    }

    [RequireComponent(typeof(Rigidbody))]
    public abstract class RBMove : MoveController
    {
        protected Rigidbody rb;
        protected override void Awake()
        {
            base.Awake();
            rb = GetComponent<Rigidbody>();
        }
    }
}