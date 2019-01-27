using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grandma;

public class BoomerangAbility : Ability
{
    public GameObject boomerangPrefab;
    public Transform returnTransform;

    public override void Activate()
    {
        GameObject b = Instantiate(boomerangPrefab);
        b.GetComponent<Boomerang>().Fire(returnTransform);
    }

    public override bool CanEnter()
    {
        return base.CanEnter() && AbilityManager.Instance.AbilityStaged == false;
    }
}
