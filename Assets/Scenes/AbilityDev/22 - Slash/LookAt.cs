using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma
{
    public abstract class LookAt : MonoBehaviour
    {
        //this should have a vector3 target and a transform

        public void InstantRotation()
        {
            transform.rotation = CalculateRotation();
        }

        public void SlerpyRotation(float speed)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, CalculateRotation(), speed * Time.deltaTime);
        }

        private Quaternion CalculateRotation()
        {
            Vector3 difference = GetTarget() - transform.position;
            float angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            return rotation;
        }


        public abstract Vector3 GetTarget();
    }
}
