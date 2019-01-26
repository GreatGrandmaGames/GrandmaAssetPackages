using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grandma;
public class InputShoot : MonoBehaviour
{
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ShootManager.Instance.FireBullet(mousePosition);
        }
        if (Input.GetButtonDown("Fire2"))
        {
            ShootManager.Instance.Recall();
        }
        if (Input.GetButtonDown("Reload"))
        {
            ShootManager.Instance.Reload();
        }
    }
}
