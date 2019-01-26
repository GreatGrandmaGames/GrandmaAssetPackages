using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grandma;

public class BoomerangData : GrandmaComponentData
{
    public float backSpeed = 40f;
    public float forwardSpeed = 20f;
    public int ammoCapacity = 1;
    public float timeUntilDestroy = 5f;

    public GameObject bulletPrefab;
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

    [HideInInspector]
    public List<GameObject> allShots = new List<GameObject>();

    protected override void OnRead(GrandmaComponentData data)
    {
        base.OnRead(data);
        boomerangData = data as BoomerangData;
        if (initialized == false)
        {
            initialized = true;
            currentAmmo = boomerangData.ammoCapacity;
        }

    }

    public void Fire(Vector3 mousePosition)
    {
        if (currentAmmo > 0)
        {
            currentAmmo--;
            state = STATE.FORWARDS;
            GameObject bullet = (Instantiate(boomerangData.bulletPrefab, transform.position, transform.rotation)) as GameObject;
            bullet.GetComponent<Collider2D>().isTrigger = true;
            bullet.GetComponent<Rigidbody2D>().velocity = (mousePosition - transform.position).normalized * boomerangData.forwardSpeed;
            allShots.Add(bullet);
            Destroy(bullet, boomerangData.timeUntilDestroy);

        }
    }

    public void Reload()
    {
        currentAmmo = boomerangData.ammoCapacity;
    }

    public void Recall()
    {
        foreach (GameObject bullet in allShots)
            StartCoroutine(RecallBullet(bullet));
    }

    private IEnumerator RecallBullet(GameObject bullet)
    {
        if (bullet == null)
        {
            yield break;
        }
        else
        {
            bullet.GetComponent<Collider2D>().isTrigger = false;
            state = STATE.BACKWARDS;
        }
        while (bullet != null && Vector2.Distance(bullet.transform.position, transform.position) > Mathf.Epsilon)
        {
            if (bullet == null)
            {
                yield break;
            }

            Vector2 distance = new Vector2(transform.position.x - bullet.transform.position.x, transform.position.y - bullet.transform.position.y);
            distance = distance.normalized * boomerangData.backSpeed;
            bullet.GetComponent<Rigidbody2D>().velocity = distance;

            //this line tells the coroutine to wait for one frame
            yield return null;

        }
    }
}
