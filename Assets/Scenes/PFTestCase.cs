using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Grandma.ParametricFirearms;


public class PFTestCase : MonoBehaviour
{
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

        testPF.Read(new PFData(testPF.GrandmaObjectID)
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
        });


    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            testPF.TriggerPress();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            testPF.TriggerRelease();
        }

        if(testPF != null)
        {
            Debug.Log(testPF.ToString());
        }
    }

    private void Assert(bool test, string message)
    {
        if (test)
        {
            Debug.Log("GrandmaObjectUnitTest: Success " + message);
        }
        else
        {
            Debug.LogError("GrandmaObjectUnitTest: Failure " + message);
        }
    }
}

