using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grandma
{
    [CreateAssetMenu(menuName = "Core/Managers/ShootManagerData")]

    public class ShootManagerData : GrandmaComponentData
    {
        public float backSpeed = 40f;
        public float forwardSpeed = 20f;
        public int ammoCapacity = 1;
        public float timeUntilDestroy = 5f;

        public GameObject bulletPrefab;
    }

    public class ShootManager : GrandmaComponent
    {
        //This is the singleton
        public static ShootManager Instance { get; private set; }

        [System.NonSerialized]
        private ShootManagerData shootManagerData;
        protected override void OnRead(GrandmaComponentData data)
        {
            base.OnRead(data);
            shootManagerData = data as ShootManagerData;

        }
        //public Transform player;
        [HideInInspector] //this hides public variables from the inspector
        public List<GameObject> allShots;

        [HideInInspector]
        public int currentAmmo;
        /*
        [SerializeField]
        GameObject iAmPrivate; i will appear in the inspector but im public
        */

        protected override void Start()
        {
            base.Start();
            Instance = this;
            Reload();
            allShots = new List<GameObject>();
        }

        public void Recall()
        {
            foreach (GameObject bullet in allShots)
            {
                StartCoroutine(RecallBullet(bullet));
                bullet.GetComponent<Collider2D>().isTrigger = false;
            }
        }

        public void Reload()
        {
            currentAmmo = shootManagerData.ammoCapacity;
        }


        public IEnumerator RecallBullet(GameObject bullet)
        {
            if (bullet == null)
            {
                yield break;
            }
            while (bullet != null && Vector2.Distance(bullet.transform.position, transform.position) > Mathf.Epsilon)
            {

                if (bullet == null)
                {
                    yield break;
                }

                Vector2 distance = new Vector2(transform.position.x - bullet.transform.position.x, transform.position.y - bullet.transform.position.y);
                distance = distance.normalized;
                distance *= (shootManagerData.backSpeed);
                bullet.GetComponent<Rigidbody2D>().velocity = distance;
                //this line tells the coroutine to wait for one frame
                yield return null;

            }
        }
        public void FireBullet(Vector3 mousePosition)
        {
            if (currentAmmo > 0)
            {
                currentAmmo--;
                GameObject bullet = (Instantiate(shootManagerData.bulletPrefab, transform.position, transform.rotation)) as GameObject;
                bullet.GetComponent<Rigidbody2D>().velocity = (mousePosition - transform.position).normalized * shootManagerData.forwardSpeed;
                bullet.GetComponent<Collider2D>().isTrigger = true;

                Destroy(bullet, shootManagerData.timeUntilDestroy);

                // ShootManager is a manager for every bullet that is fired
                // The instance allows us to access the current ShootManager from anywhere in the project
                // This line adds the current instantiated bullet to the list of allShots
                ShootManager.Instance.allShots.Add(bullet);
            }
        }
    }
}
