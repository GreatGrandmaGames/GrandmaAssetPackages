using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grandma;

public class LookAtPlayer : LookAt
{
    public GameObject player;
    public float speed = 2f;
    public override Vector3 GetTarget()
    {
        return player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        base.SlerpyRotation(speed);
    }
}
