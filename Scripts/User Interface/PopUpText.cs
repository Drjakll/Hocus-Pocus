using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PopUpText : MonoBehaviour
{
    // Start is called before the first frame update
    public float Speed = 2;
    public TextMeshPro TMP;

    void Start()
    {
        TMP = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        Speed -= Time.deltaTime;
        transform.Translate(new Vector3(0, Speed * Time.deltaTime, 0), Space.Self);
        transform.LookAt(Camera.main.transform);
        transform.Rotate(new Vector3(0, 180, 0));
        if(Speed < 0)
        {
            Object.Destroy(this.gameObject);
        }
    }
}
