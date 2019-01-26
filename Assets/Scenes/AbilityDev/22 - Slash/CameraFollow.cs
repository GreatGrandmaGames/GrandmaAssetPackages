using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;

    //the higher the movement, the faster the camera will find player
    public float followSpeed = 1.25f;
    public Vector2 offset;
    private Rigidbody2D rb;
    private float z;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        z = transform.position.z;
    }

    void Update()
    {
        Vector3 desiredPosition = target.position + (Vector3) offset;
        Vector3 smoothedPosition = Vector3.Lerp (transform.position, desiredPosition, followSpeed * Time.deltaTime);
        smoothedPosition.z = z;
        transform.position = smoothedPosition;
    }
}
