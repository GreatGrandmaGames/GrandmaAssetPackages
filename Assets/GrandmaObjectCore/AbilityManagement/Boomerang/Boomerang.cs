using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grandma;


[CreateAssetMenu(menuName = "BoomerangData")]
public class BoomerangData : GrandmaComponentData
{
    public float backSpeed = 40f;
    public float forwardSpeed = 20f;
    public float timeUntilDestroy = 5f;
    public float stunTime = 2f;
    public float damage = 5f;
       
}

public class Boomerang : GrandmaComponent
{
    public enum STATE
    {
        FORWARDS,
        BACKWARDS
    }

    private STATE state;
    private BoomerangData boomerangData;
    private int currentAmmo;
    private bool initialized = false;

    private Transform returnTransform;

    protected override void Awake()
    {
        base.Awake();
        StartCoroutine(PollForRecall());
        GetComponent<Collider2D>().isTrigger = true;
        Destroy(this, boomerangData.timeUntilDestroy);
    }

    protected override void OnRead(GrandmaComponentData data)
    {
        base.OnRead(data);
        boomerangData = data as BoomerangData;
        if (initialized == false)
        {
            initialized = true;
        }
    }

    public void Fire(Transform returnTransform)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        state = STATE.FORWARDS;
        GetComponent<Rigidbody2D>().velocity = (mousePosition - returnTransform.position).normalized * boomerangData.forwardSpeed;
        this.returnTransform = returnTransform;
    }

    private IEnumerator RecallBullet()
    {
        state = STATE.BACKWARDS;

        while (Vector2.Distance(transform.position, returnTransform.position) > Mathf.Epsilon)
        {
           
            Vector2 distance = new Vector2(transform.position.x - returnTransform.position.x, transform.position.y - returnTransform.position.y);
            distance = distance.normalized * boomerangData.backSpeed;
            GetComponent<Rigidbody2D>().velocity = distance;

            //this line tells the coroutine to wait for one frame
            yield return null;

        }
    }

    private IEnumerator PollForRecall()
    {
        while(Input.GetButtonDown("Recall") == false)
        {
            Debug.Log("Not recalling");
            yield return null;
        }
        StartCoroutine(RecallBullet());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.state == STATE.FORWARDS)
        {
            // ? null propogation, basically if (collision != null)
            collision?.GetComponent<Stunnable>()?.Stun(boomerangData.stunTime);
        }
        else if(this.state == STATE.BACKWARDS)
        {
            collision?.GetComponent<Damageable>()?.Damage(new DamageablePayload()
            {
                amount = boomerangData.damage,
                agentID = "BackwardsBoomerang",
            });
        }
    }
}
