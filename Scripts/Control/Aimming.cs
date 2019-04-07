using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aimming : MonoBehaviour
{
    private Vector3 CurrentCoord;
    private Camera mainCam;
    private float MaxScreenRadius;

    public GameObject TrackingObject;
    public float MaxWorldRadius = .25f;
    public Vector3 MouseCoord;

    // Start is called before the first frame update
    void Start()
    {
        MouseCoord = Input.mousePosition;
        CurrentCoord = transform.transform.localPosition;
        mainCam = Camera.main;
        MaxScreenRadius = (new Vector2(mainCam.pixelWidth / 2, mainCam.pixelHeight / 2)).magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        MouseCoord = Input.mousePosition;
        Vector2 ScreenSpace = new Vector2(mainCam.pixelWidth / 2, mainCam.pixelHeight / 2);
        Vector2 Displacement = new Vector2(MouseCoord.x - ScreenSpace.x, MouseCoord.y - ScreenSpace.y);
        Vector2 normDisplace = Displacement.normalized;
        float Radius = .25f * Displacement.magnitude / MaxScreenRadius;

        CurrentCoord = Radius <= MaxWorldRadius ? new Vector3(transform.localPosition.x, transform.localPosition.y + normDisplace.y * Radius, transform.localPosition.z + normDisplace.x * Radius) :
                                                  new Vector3(transform.localPosition.x, transform.localPosition.y + normDisplace.y * MaxWorldRadius, transform.localPosition.z + normDisplace.x * MaxWorldRadius);

        TrackingObject.transform.localPosition = CurrentCoord;
    }
}
