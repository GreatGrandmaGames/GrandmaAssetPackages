using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma
{
    public static class ControlUtility2D
    {
        public static GrandmaObject GetObjectUnderMouse()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

            return hit.transform?.GetComponentInParent<GrandmaObject>();
        }
    }
}
