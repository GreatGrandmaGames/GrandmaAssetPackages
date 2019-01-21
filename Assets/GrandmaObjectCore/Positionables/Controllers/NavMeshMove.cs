using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Grandma
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class NavMeshMove : MoveToVector
    {
        private NavMeshAgent nvAgent;

        public override bool CanMove { get => base.CanMove; 
            set 
            {
                base.CanMove = value;

                nvAgent.isStopped = !value;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            nvAgent = GetComponent<NavMeshAgent>();
        }

        public override void OnTargetSet(Vector3 target)
        {
            nvAgent.SetDestination(target);
        }
    }
}
