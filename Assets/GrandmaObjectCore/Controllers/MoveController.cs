using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma.Core
{
    //This class essentially instantiates all of the different movement modes we want
    //and, when called, switches between them
    public class MoveController: MonoBehaviour
    {
        public Moveable active;

        private List<Moveable> allModes = new List<Moveable>();

        public void SwitchMode(Moveable switchTo)
        {
            if (active != null)
            {
                active.Deactivate();
            }
            active = allModes.Find(x => x == switchTo);
            if (active == null)
            {
                AddMode(switchTo);
                active = switchTo;
            }
            if (active != null)
            {
                active.Activate();
            }
            else
            {
                //just for now
                throw new System.Exception("MoveController : SwitchMode - Active should never be null.");
            }
        }

        private void FixedUpdate()
        {
            if (active != null)
            {
                active.Move();
            }
        }
        public void AddMode(Moveable addMe)
        {
            allModes.Add(addMe);
        }

        public void AddModes(List<Moveable> addMeList)
        {
            allModes.AddRange(addMeList);
        }
    }
}
