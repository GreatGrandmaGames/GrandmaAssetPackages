using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma
{
    public class IDGenerator
    {
        private int counter = 0;
        private List<string> allocatedIDs = new List<string>();

        public string NewID()
        {
            while (allocatedIDs.Contains(counter.ToString()))
            {
                counter++;
            }

            return counter.ToString();
        }

        public string RegisterID(string id)
        {
            if (string.IsNullOrEmpty(id) || allocatedIDs.Contains(id))
            {
                id = NewID();
            }

            allocatedIDs.Add(id);

            return id;
        }
    }
}
