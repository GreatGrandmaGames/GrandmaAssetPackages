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

            if (Physics.Raycast(ray, out RaycastHit hit))
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
