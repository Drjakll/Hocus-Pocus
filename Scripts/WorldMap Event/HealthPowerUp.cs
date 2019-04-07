using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerUp : PowerUp
{
    // Start is called before the first frame update
    void Start()
    {
        Radius = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject Char = DetectNearbyCharacter();
        if (Char != null)
        {
            TakeEffect(Char);
            GameObject.Find("Terrain").GetComponent<SpawnPowerUps>().PowerUpsCount--;
            Object.Destroy(this.gameObject);
        }
        UpAndDown();
    }

    public override void TakeEffect(GameObject Character)
    {
        CharacterStatus CS = Character.GetComponent<CharacterStatus>();
        if (CS != null)
        {
            PlayAnimation(CS.gameObject);
            CS.MaxHealth = CS.MaximumHealth * Random.Range(.05f, .1f);
        }
    }
}
