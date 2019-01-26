using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma
{
    public static class ControlUtility
    {
        public static GrandmaObject GetObjectUnderMouse()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                return hit.transform?.GetComponentInParent<GrandmaObject>();
            }
            else
            {
                return null;
            }
        }
    }
}
