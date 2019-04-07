using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicShield : MonoBehaviour
{
    public float MaxTime;
    public float currentTime;
    public bool StartCounting = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (StartCounting) {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0)
            {
                Object.Destroy(this.gameObject);
            }
        }
    }
}
