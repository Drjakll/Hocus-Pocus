using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chilled : MonoBehaviour
{
    public bool Unchill = false;
    private Transform[] children;
    private Vector3 MaxScale;
    // Start is called before the first frame update
    void Start()
    {
        children = GetComponentsInChildren<Transform>();
        MaxScale = children[1].localScale;
    }

    private void Update()
    {
        if (Unchill)
        {
            Object.Destroy(this.gameObject);
        }
    }

    public void ChangeIntensity(float Intensity)
    {
        if (!Unchill)
        {
            for(int i = 0; i < children.Length; i++)
            {
                children[i].localScale = MaxScale * Intensity;
            }
        }
    }
}
