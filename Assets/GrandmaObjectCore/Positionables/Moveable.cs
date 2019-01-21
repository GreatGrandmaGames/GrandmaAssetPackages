using System.Collections.Generic;
using UnityEngine;

namespace Grandma
{ 
    public class Moveable : Positionable
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

        //Calls SwitchMode for this controller on Start
        public MoveController StartingController;

        public MoveController Active { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            
            SwitchMode(StartingController);
        }

        //Can be null - no movement system will be active
        public void SwitchMode(MoveController switchTo)
        { 
            if (Active != null)
            {
                Active.Deactivate();
            }

            //Lazy add
            if (AllModes.Contains(switchTo) == false)
            {
                AllModes.Add(switchTo);
            }

            Active = switchTo;

            if (Active != null)
            {
                Active.Activate();
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

            int currIndex = allModes.FindIndex(x => x == Active);
            SwitchMode(AllModes[(currIndex + 1) % AllModes.Count]);
        }
    }

}