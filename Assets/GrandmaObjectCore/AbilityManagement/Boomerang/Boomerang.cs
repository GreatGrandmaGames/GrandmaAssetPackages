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
        GetComponent<Rigidbody2D>().velocity = (mousePosition - returnTransform.position).normalized * boomerangData.forwardSpeed;

        Physics2D.IgnoreCollision(returnTransform.GetComponentInChildren<Collider2D>(), GetComponent<Collider2D>(), true);

        this.returnTransform = returnTransform;
        transform.position = returnTransform.position;

        state = STATE.FORWARDS;

        StartCoroutine(PollForRecall());
    }

    private IEnumerator RecallBullet()
    {
        state = STATE.BACKWARDS;

        Physics2D.IgnoreCollision(returnTransform.GetComponentInChildren<Collider2D>(), GetComponent<Collider2D>(), false);

        while (Vector2.Distance(transform.position, returnTransform.position) > Mathf.Epsilon)
        {         
            Vector2 distance = new Vector2(transform.position.x - returnTransform.position.x, transform.position.y - returnTransform.position.y);
            distance = distance.normalized * -boomerangData.backSpeed;
            GetComponent<Rigidbody2D>().velocity = distance;

            //this line tells the coroutine to wait for one frame
            yield return null;
        }
    }

    private IEnumerator PollForRecall()
    {
        while(Input.GetButtonDown("Fire2") == false)
        {
            yield return null;
        }
        StartCoroutine(RecallBullet());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponentInParent<Rigidbody2D>()?.transform == returnTransform)
        {
            Destroy(this.gameObject);
            return;
        }

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
