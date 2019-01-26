using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma
{
    /// <summary>
    /// Allows other classes to enable / disable a component, without overlapping
    /// </summary>
    public class LockDown
    {
        public List<string> lockingComponentIDs = new List<string>();

        public bool IsUnlocked
        {
            get
            {
                return lockingComponentIDs.Count <= 0;
            }
        }

        public void Lock(string id)
        {
            if (lockingComponentIDs.Contains(id) == false)
            {
                lockingComponentIDs.Add(id);
            }
        }

        public void Unlock(string id)
        {
            if (lockingComponentIDs.Contains(id))
            {
                lockingComponentIDs.Remove(id);
            }
        }
    }
}
