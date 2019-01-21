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
        
        pf.OnStateChanged += (state) =>
        {
            Debug.Log(pf);
        };

        pf.OnUpdated += (pfComp) =>
        {
            if(pfComp == null || pfComp as ParametricFirearm == null || (pfComp as ParametricFirearm).Data as PFData == null)
            {
                return;
            }

            var pfData = ((pfComp as ParametricFirearm).Data as PFData);

            if(chargeUp != null)
            {
                chargeUp.value = pfData.Dynamic.ChargeUpTime;
                chargeUp.maxValue = pfData.ChargeTime.chargeTime;
                chargeUp.minValue = 0f;
            }

            if (coolDown)
            {
                coolDown.value = pfData.Dynamic.CoolDownTime;
                coolDown.maxValue = pfData.RateOfFire.ReloadTime;
                coolDown.minValue = 0f;
            }

            if(ammo) 
            {
                ammo.text = string.Format("{0} / {1}", pfData.Dynamic.CurrentAmmo, pfData.RateOfFire.AmmoCapacity);
            }
        };
	}
}
