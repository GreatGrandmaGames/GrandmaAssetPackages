using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Grandma.ParametricFirearms;


public class PFTestCase : MonoBehaviour
{
    public PFData exampleData;
    public ParametricFirearm prefab;

    private ParametricFirearm testPF;

    void Start()
    {
        if (prefab == null)
        {
            Debug.Log("Please provide objects for testing");
            return;
        }

        StartCoroutine(Test());
    }

    private IEnumerator Test()
    {
        yield return new WaitForSeconds(0.1f);

        testPF = Instantiate(prefab);

        exampleData.associatedObjID = testPF.ObjectID;

        testPF.Read(exampleData);
            /*
            new PFData(testPF.GrandmaObjectID)
        {
            Meta = new PFMetaData()
            {
                name = "Test case"
            },
            Multishot = new PFMultishotData()
            {
                numberOfShots = 1
            },
            ChargeTime = new PFChargeTimeData()
            {
                chargeTime = 0.5f,
                requireFullyCharged = false,
            },
            RateOfFire = new PFRateOfFireData()
            {
                baseRate = 0.1f,
                reloadingData = new PFBurstData(10, 1f) 
            },
            Projectile = new PFProjectileData(testPF.GrandmaObjectID)
            {
                ImpactDamage = new PFImpactDamageData()
                {
                    baseDamage = 10f,
                    damageChangeByDistance = 0.01f
                },
                AreaDamage = new PFAreaDamageData()
                {
                    explodable = true,
                    maxBlastRange = 1f,
                    maxDamage = 40f,
                    numImpactsToDetonate = 2
                },
                Trajectory = new PFTrajectoryData()
                {
                    dropOffRatio = 0.01f,
                    initialForceVector = 50f,
                    maxInitialSpreadAngle = 1f
                }
            }
        }
        );*/

        testPF.OnStateChanged += (newState) => 
        {
            Debug.Log(testPF);
        };

       //GetComponent<PlayerPFController>().inventory.Add(testPF);
      //GetComponent<PlayerPFController>().ScrollToNextWeapon();
    }
}

