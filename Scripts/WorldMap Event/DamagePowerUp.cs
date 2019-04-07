using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePowerUp : PowerUp
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
        if(Char != null)
        {
            TakeEffect(Char);
            GameObject.Find("Terrain").GetComponent<SpawnPowerUps>().PowerUpsCount--;
            Object.Destroy(this.gameObject);
        }
        UpAndDown();
    }

    public override void TakeEffect(GameObject Character)
    {
        BaseControl BC = Character.GetComponent<BaseControl>();
        if(BC != null)
        {
            PlayAnimation(BC.gameObject);
            foreach (AbstractSpell AS in BC.Spells)
            {
                float Coeff = Random.Range(.01f, .1f);
                AS.MaxDamage += AS.MaxDamage * Coeff;
                AS.MinDamage += AS.MinDamage * Coeff;
            }
        }
    }
}
