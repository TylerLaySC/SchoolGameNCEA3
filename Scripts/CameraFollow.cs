using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float followSpeed = 1f;
    public float yOffset = 0f;
    public Transform target;
    private Rigidbody2D rb;
    public float CameraSize = 7f;

    private void Start()
    {
        rb = target.GetComponent<Rigidbody2D>();
    }
    private Camera mainCam => Camera.main;
    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y + yOffset, -15f);
        transform.position = Vector3.Slerp(transform.position, newPos, followSpeed * Time.deltaTime);

        mainCam.orthographicSize = GetCameraZoom();
    }

    float GetCameraZoom()
    {
        return CameraSize * ((Mathf.Lerp(0, 1f, rb.velocity.magnitude / 11.625f) * 0.5f) + 1);
    }
}
