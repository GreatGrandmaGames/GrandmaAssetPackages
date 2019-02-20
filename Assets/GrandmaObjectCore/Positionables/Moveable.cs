using System.Collections.Generic;
using UnityEngine;

namespace Grandma
{ 
    public class Moveable : Positionable
    {
        public KeyCode switchCode;
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

        //Calls SwitchMode for this controller on Start
        public MoveController StartingController;

        public MoveController ActiveController { get; private set; }
        public LockDown ChangeMovementLock { get; private set; } = new LockDown();

        protected override void Awake()
        {
            base.Awake();
            
            SwitchMode(StartingController);
        }

        public void EnableMovement(bool enabled)
        {
            if (ChangeMovementLock.IsUnlocked)
            {
                if (ActiveController != null)
                {
                    if (enabled)
                    {
                        ActiveController.Activate();
                    } else
                    {
                        ActiveController.Deactivate();
                    }
                }
            }
        }

        //Can be null - no movement system will be active
        public void SwitchMode(MoveController switchTo)
        {
            if (ChangeMovementLock.IsUnlocked)
            {
                EnableMovement(false);

                //Lazy add
                if (AllModes.Contains(switchTo) == false)
                {
                    AllModes.Add(switchTo);
                }

                ActiveController = switchTo;

                EnableMovement(true);
            }
        }

        /// <summary>
        /// For debugging
        /// </summary>
        public void NextMode()
        {
            if(AllModes.Count <= 0)
            {
                return;
            }

            int currIndex = allModes.FindIndex(x => x == ActiveController);
            SwitchMode(AllModes[(currIndex + 1) % AllModes.Count]);
        }

        /*private void Update()
        {
            if (Input.GetKeyDown(switchCode))
            {
                NextMode();
            }
        }*/
    }
    
}