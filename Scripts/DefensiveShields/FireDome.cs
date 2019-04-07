using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class FireDome : AbstractSpell
{
    public override void Init()
    {
        Cooldown = 30;
        currentCoolDown = 30;
        Cost = 30;
        EffectDuration = 10;
    }

    public override Attacks CastSpell()
    {
        return Attacks.MagicLight;
    }

    public override bool InitMagicalObject(GameObject Weapon, GameObject Direction)
    {
        CharacterStatus Charstats = Weapon.GetComponentInParent<CharacterStatus>();
        if(Charstats != null && Charstats.CurrentMana >= Cost)
        {
            GameObject FireShield = Instantiate(SpellPrefab);
            MagicShield shield = FireShield.GetComponent<MagicShield>();
            shield.currentTime = EffectDuration;
            shield.MaxTime = EffectDuration;
            FireShield.transform.parent = Charstats.transform;
            FireShield.transform.localPosition = Vector3.zero + (new Vector3(0, 1, 0));
            Charstats.CurrentMana -= Cost;
            shield.StartCounting = true;
            GlobalCoolDown = true;
            return true;
        }
        return false;
    }

    public override void UpdateCoolDown()
    {
        currentCoolDown += Time.deltaTime;
        if (currentCoolDown > Cooldown)
        {
            currentCoolDown = Cooldown;
        }
    }
}
