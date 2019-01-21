using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Grandma.ParametricFirearms
{
    /// <summary>
    /// Parameters to define the rate of fire for a PF
    /// </summary>
    //The fields here are private, to allow this class to do some
    //pre-processing before being sent off to the PF
    [Serializable]
    public class PFRateOfFireData : IGrandmaModifiable
    {
        [Tooltip("The default time between shots. Measured in seconds")]
        public float baseRate;

        //Separated out for clarity, since players can trigger the reload manually
        //and when we call this data, we have to reset the ammo count
        [SerializeField]
        public PFBurstData reloadingData;

        //All non-reload burst data. N < reloadingData.N, otherwise the burst data will never be triggered
        [SerializeField]
        public List<PFBurstData> burstData = new List<PFBurstData>();

        public int AmmoCapacity
        {
            get
            {
                return reloadingData.count;
            }
        }

        public float ReloadTime
        {
            get
            {
                return reloadingData.time;
            }
        }

        /*
         * N is the number of bullets in a burst. 
         * A burst can be a single bullet, or 
         * represent burst fire (e.g. triple shot)
         * or represent reload time.
         * 
         * For decreasing N, we see if the ammo
         * remaining is divisible by N. If so, we return
         * the associated wait time. 
         * We run by decreasing values, since if a number
         * is divisible by x, then it will be divisible
         * by x * y, so larger N would be ignored.
         * 
         */
        /// <summary>
        /// Finds the correct wait time from the provided possible times.
        /// </summary>
        /// <param name="ammoRemaining"></param>
        /// <returns></returns>
        public float GetWaitTime(int ammoRemaining)
        {
            var burstAndReload = new List<PFBurstData>();
            burstAndReload.AddRange(burstData ?? new List<PFBurstData>());
            burstAndReload.Add(reloadingData);

            burstAndReload.Sort((a, b) =>
            {
                return b.count.CompareTo(a.count);
            });

            foreach (PFBurstData x in burstAndReload)
            {
                //Prevent divide by zero
                if(x.count == 0)
                {
                    continue;
                }

                if ((ammoRemaining % x.count) == 0)
                {
                    return x.time;
                }
            }
            return baseRate;
        }
    }

    [Serializable]
    public struct PFBurstData
    {
        [Tooltip("The number of shots in the current burst")]
        public int count;
        [Tooltip("The time between shots for the current burst. Measured in seconds")]
        public float time;

        public PFBurstData(int count, float time)
        {
            this.count = count;
            this.time = time;
        }
    }
}