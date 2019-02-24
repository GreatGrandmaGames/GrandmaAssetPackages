using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Grandma;

public class GrandmaObjectUnitTest : MonoBehaviour {

    public GrandmaObject prefab;

    public GrandmaObject previouslySpawned_WithData;
    public GrandmaObject previouslySpawned_NoData;

	void Start ()
    {
        if(previouslySpawned_NoData == null || previouslySpawned_WithData == null)
        {
            Debug.Log("Please provide objects for testing");
            return;
        }

        StartCoroutine(Test());
	}

    private IEnumerator Test()
    {
        yield return new WaitForSeconds(0.1f);

        
        //Registration
        Assert(GrandmaObjectManager.Instance.AllObjects.Count == 2,
            "GrandmaObject registered");

        //Setting IDs
        var ps_withData_ID = previouslySpawned_WithData.Data.id;
        var ps_noData_ID = previouslySpawned_NoData.Data.id;

        Assert(string.IsNullOrEmpty(ps_noData_ID) == false, " PS No Data ID is set to " + ps_noData_ID);
        Assert(string.IsNullOrEmpty(ps_withData_ID) == false, " PS With Data ID is set to " + ps_withData_ID);

        //Get by ID
        var get_ps = GrandmaObjectManager.Instance.GetByObjectID(ps_withData_ID);

        Assert(get_ps == previouslySpawned_WithData, " Get By ID");

        //Registration

        Destroy(previouslySpawned_WithData.gameObject);

        yield return null;

        Assert(GrandmaObjectManager.Instance.AllObjects.Count == 1,
            "GrandmaObject unregistered");

        //Registration at runtime

        yield return null;

        Assert(GrandmaObjectManager.Instance.AllObjects.Count == 2,
                "Instantiated GrandmaObject registered");

        

        yield return null;
        var spawned = Instantiate(prefab);
        
    }

    private void Assert(bool test, string message)
    {
        if (test)
        {
            Debug.Log("GrandmaObjectUnitTest: Success " + message);
        } else
        {
            Debug.LogError("GrandmaObjectUnitTest: Failure " + message);
        }
    }
}
