using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma.ParametricFirearms
{
    public class PFDataContainer : MonoBehaviour
    {
        public PFData Data;
        
        void Start()
        {
            var pf = GetComponent<ParametricFirearm>();

            if(pf != null)
            {
                pf.Read(Data);
            }
        }
    }
}