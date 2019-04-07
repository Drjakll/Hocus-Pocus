using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPowerUps : MonoBehaviour
{
    [SerializeField] GameObject Field;
    [SerializeField] PowerUp[] PowerUps;

    public float Period = 30;
    public int PowerUpsCount = 0;

    private Vector3 FieldDimension;
    private float Timer = 0;
    private int CurrentIndex = 0;

    void Start()
    {
        FieldDimension = Field.GetComponent<Collider>().bounds.extents * 2;
    }

    // Update is called once per frame
    void Update()
    {
        if((Timer %= Period) <= .03f && PowerUpsCount <= 15)
        {
            float x = Random.Range(50, FieldDimension.x - 50);
            float z = Random.Range(50, FieldDimension.z - 50);
            float y = 3;

            GameObject PU = Instantiate(PowerUps[CurrentIndex].gameObject);
            PU.transform.parent = transform;
            PU.transform.localPosition = (new Vector3(x, y, z));

            PowerUpsCount++;
            Timer += .03f;

            CurrentIndex = (CurrentIndex + 1) % PowerUps.Length;
        }
        Timer += Time.deltaTime;
    }
}
