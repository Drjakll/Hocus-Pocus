using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class FlameStrike : AbstractSpell
{
    public override void Init()
    {
        Cooldown = 10;
        currentCoolDown = 10;
        MaxDamage = 100;
        MinDamage = 75;
        EffectDuration = 5;
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
            GameObject Flames = Instantiate(SpellPrefab);
            Flames.transform.position = Charstats.gameObject.transform.position + (new Vector3(0, .5f, 0));
            Impulse impulseWave = Flames.GetComponent<Impulse>();
            impulseWave.wave = ImpulseDamage;
            Charstats.CurMana = -Cost;
            return true;
        }
        return false;
    }

    public bool ImpulseDamage(GameObject hitObj, GameObject thisObj)
    {
        if (hitObj.name == CasterName)
            return false;
        CharacterStatus Charstats = hitObj.GetComponent<CharacterStatus>();
        if (!ShieldHandlerFromFire(hitObj) && Charstats != null)
        {
            Charstats.CurHealth = -Random.Range(MinDamage, MaxDamage);
            Charstats.CurrentFireDuration += EffectDuration * Time.deltaTime;
            if (hitObj.GetComponentInChildren<Burning>() == null)
            {
                GameObject burnEffect = Instantiate(SpecialEffect);
                burnEffect.transform.position = hitObj.transform.position + (new Vector3(0, 3f, 0));
                burnEffect.transform.parent = hitObj.transform;
            }
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
