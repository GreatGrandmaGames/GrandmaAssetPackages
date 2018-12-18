using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Grandma.ParametricFirearms;

[RequireComponent(typeof(ParametricFirearm))]
public class PFLogger : MonoBehaviour {

    public Slider chargeUp;
    public Slider coolDown;
    public Text ammo;

	void Start ()
    {
        var pf = GetComponent<ParametricFirearm>();
        
        pf.onStateChanged += (state) =>
        {
            Debug.Log(pf);
        };

        pf.OnDataUpdated += (pfComp) =>
        {
            if(pfComp == null || pfComp as ParametricFirearm == null || (pfComp as ParametricFirearm).Data as PFData == null)
            {
                return;
            }

            var pfData = ((pfComp as ParametricFirearm).Data as PFData);

            chargeUp.value = pfData.Dynamic.ChargeUpTime;
            chargeUp.maxValue = pfData.ChargeTime.chargeTime;
            chargeUp.minValue = 0f;

            coolDown.value = pfData.Dynamic.CoolDownTime;
            coolDown.maxValue = pfData.RateOfFire.reloadingData.time;//TODO: current cooldown
            coolDown.minValue = 0f;
            ammo.text = string.Format("{0} / {1}", pfData.Dynamic.CurrentAmmo, pfData.RateOfFire.AmmoCapacity);
        };
	}
}
