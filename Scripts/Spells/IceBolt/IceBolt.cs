using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class IceBolt : AbstractSpell
{
    public override void Init()
    {
        Cooldown = .75f;
        currentCoolDown = .75f;
        MaxDamage = 75;
        MinDamage = 50;
        EffectDuration = 3;
        Cost = 7;
    }

    public override Attacks CastSpell()
    {
        return Attacks.ShootStraight;
    }

    public override bool InitMagicalObject(GameObject Weapon, GameObject Direction)
    {
        CharacterStatus Charstats = Weapon.GetComponentInParent<CharacterStatus>();
        if (Charstats.CurrentMana >= Cost)
        {
            GameObject iceBolt = Instantiate(SpellPrefab);
            iceBolt.transform.position = Weapon.transform.position - Weapon.transform.right * 2;
            iceBolt.transform.LookAt(Direction.transform);
            MovingObject MO = iceBolt.GetComponent<MovingObject>();
            MO.ExplosionEffect = ExplosionEffect;
            MO.InitVelocity = CurrentVelocity;
            MO.Go = true;
            MO.MaxDamage = MaxDamage;
            MO.MinDamage = MinDamage;
            MO.Duration = EffectDuration;
            MO.CasterName = Charstats.gameObject.name;
            Charstats.CurMana = -Cost;
            return true;
        }
        return false;
    }

    public bool ExplosionEffect(GameObject ObjHit, GameObject thisObj)
    {
        CharacterStatus CharStats = ObjHit.GetComponent<CharacterStatus>();
        if (ObjHit.name == CasterName)
            return false;
        if (!ShieldHandlerFromFrost(ObjHit) && CharStats != null)
        {
            Status[] status = new Status[2];
            status[0] = Status.Stunned;
            status[1] = Status.Slowed;
            CharStats.CurrentSlowedDuration += EffectDuration;
            CharStats.CurHealth = -Random.Range(MinDamage, MaxDamage);
            if (ObjHit.GetComponentInChildren<Chilled>() == null)
            {
                GameObject frostEffect = Instantiate(SpecialEffect);
                frostEffect.transform.parent = ObjHit.transform;
                frostEffect.transform.localPosition = Vector3.zero + (new Vector3(0, .1f, 0));
            }
            CharStats.ReactionToHit(thisObj, status);
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
