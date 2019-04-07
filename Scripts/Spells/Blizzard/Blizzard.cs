using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class Blizzard : AbstractSpell
{

    public override void Init()
    {
        Cooldown = 10;
        currentCoolDown = 10;
        MaxDamage = 30;
        MinDamage = 15;
        Cost = 20;
    }

    public override Attacks CastSpell()
    {
        return Attacks.MagicLight;
    }

    public override bool InitMagicalObject(GameObject Weapon, GameObject Direction)
    {
        CharacterStatus Charstats = Weapon.GetComponentInParent<CharacterStatus>();
        CasterName = Charstats.gameObject.name;
        if (Charstats.CurrentMana >= Cost)
        {
            GameObject Blizz = Instantiate(SpellPrefab);
            Blizz.transform.position = CastAtPosition;
            Stationary stationary = Blizz.GetComponent<Stationary>();
            stationary.maxDuration = Cooldown;
            stationary.duration = Cooldown;
            stationary.OnVictim = Effect;
            Charstats.CurMana = -Cost;
            return true;
        }
        return false;
    }

    public bool Effect(GameObject HitObj, GameObject thisObj)
    {
        if (HitObj.name == CasterName)
            return false;
        CharacterStatus CharStats = HitObj.GetComponent<CharacterStatus>();

        if(!ShieldHandlerFromFrost(HitObj) && CharStats != null)
        {
            if (HitObj.GetComponentInChildren<Chilled>() == null)
            {
                GameObject frostEffect = Instantiate(SpecialEffect);
                frostEffect.transform.parent = HitObj.transform;
                frostEffect.transform.localPosition = Vector3.zero + (new Vector3(0, .1f, 0));
                CharStats.CurrentStats = (short)Status.Slowed;
            }
            CharStats.CurHealth = -Random.Range(MinDamage, MaxDamage);
        }
        return true;
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
