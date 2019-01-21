using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma
{

    public class Level : Positionable
    {
        public enum LevelState
        {
            //null
            Invalid,
            //player is not in level
            Ready,
            //player is in the level, not finished w required objective
            InProgress,
            //player has completed objectives.
            Complete
        }

        private LevelState state;
        public Objective primaryObjective;

        protected override void Awake()
        {
            base.Awake();
            state = LevelState.Ready;
        }

        //Level Functions changing state
        public void StartLevel()
        {
            if (state != LevelState.Ready)
            {
                Debug.LogError("Can't start a level if level isn't ready");
                return;
            }
            if (primaryObjective == null)
            {
                Debug.LogError("Can't start a level without a primary objective");
                return;
            }
            state = LevelState.InProgress;

            //listens to when primary objective's on complete action
            //is finished and calls the added function, finish level
            primaryObjective.OnComplete += FinishLevel;
        }
        public void LeaveLevel()
        {
            if (state != LevelState.InProgress)
            {
                Debug.LogError("Can't leave a level if level isn't in progress.");
                return;
            }
            state = LevelState.Ready;
        }
        public void FailLevel()
        {
            if (state != LevelState.InProgress)
            {
                Debug.LogError("Can't fail a level if level isn't in progress.");
                return;
            }
            state = LevelState.Ready;
        }
        private void FinishLevel()
        {
            if (state != LevelState.InProgress)
            {
                Debug.LogError("Can't finish a level if level isn't in progress.");
                return;
            }
            state = LevelState.Complete;
        }
        //RestartLevel()


    }

}
