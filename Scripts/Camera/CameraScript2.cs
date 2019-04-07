using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript2 : MonoBehaviour
{
    [SerializeField] GameObject Center;

    private float VerticalRadius;
    private float HorizontalRadius;
    private float VerticalAngle;
    private float HorizontalAngle;

    public float MaxZoom = 10;
    public float MinZoom = 5;
    public float ZoomSpeed = 500;
    public float RotateSpeed = 12.5f;

    // Start is called before the first frame update
    void Start()
    {
        transform.localPosition = new Vector3(0, 0, -5);
        VerticalRadius = 5;
        HorizontalRadius = 5;
        VerticalAngle = 0;
        HorizontalAngle = Mathf.PI * 1.5f;
        transform.LookAt(Center.transform);
    }

    // Update is called once per frame
    void Update()
    {
        float Zoom = Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * ZoomSpeed;
        if(Zoom != 0)
        {
            Zooming(Zoom);
        }

        if (Input.GetMouseButton(1))
        {
            float deltaY = Input.GetAxis("Mouse Y") * RotateSpeed * Time.deltaTime;
            if (deltaY != 0)
                rotateVertically(-deltaY);
        }
    }

    private void rotateVertically(float deltaVertical)
    {
        VerticalAngle += deltaVertical;
        if (VerticalAngle > Mathf.PI * .125f || VerticalAngle < 0)
        {
            VerticalAngle -= deltaVertical;
            return;
        }
        float VertPos = Mathf.Sin(VerticalAngle) * VerticalRadius;
        float HorizPos = Mathf.Cos(VerticalAngle) * VerticalRadius;
        Vector3 localPos = transform.localPosition;
        transform.localPosition = new Vector3(localPos.x, VertPos, -HorizPos);
        transform.LookAt(Center.transform);
    }

    private void Zooming(float zoomAmount)
    {
        transform.Translate(new Vector3(0, 0, zoomAmount), Space.Self);
        VerticalRadius = transform.localPosition.magnitude;
        if(VerticalRadius < MinZoom || VerticalRadius > MaxZoom)
        {
            transform.Translate(new Vector3(0, 0, -zoomAmount), Space.Self);
            VerticalRadius = transform.localPosition.magnitude;
        }
        transform.LookAt(Center.transform);
    }
}
