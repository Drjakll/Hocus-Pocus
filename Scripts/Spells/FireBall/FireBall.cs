using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

public class FireBall : AbstractSpell
{

    public override void Init()
    {
        Cooldown = .75f;
        currentCoolDown = .75f;
        MaxDamage = 100;
        MinDamage = 75;
        EffectDuration = 5;
        Cost = 10;
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
            GameObject fireBall = Instantiate(SpellPrefab);
            fireBall.transform.position = Weapon.transform.position - Weapon.transform.right * 2;
            fireBall.transform.LookAt(Direction.transform);
            MovingObject MO = fireBall.GetComponent<MovingObject>();
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
        if (!ShieldHandlerFromFire(ObjHit) && CharStats != null)
        {
            Status[] status = new Status[2];
            status[0] = Status.Stunned;
            status[1] = Status.Burning;
            CharStats.CurrentFireDuration += EffectDuration;
            CharStats.CurHealth = -Random.Range(MinDamage, MaxDamage);
            if (ObjHit.GetComponentInChildren<Burning>() == null)
            {
                GameObject burnEffect = Instantiate(SpecialEffect);
                burnEffect.transform.position = ObjHit.transform.position + (new Vector3(0, 3f, 0));
                burnEffect.transform.parent = ObjHit.transform;
            }
            CharStats.ReactionToHit(thisObj, status);
        }
        return true;
    }
    

    public override void UpdateCoolDown()
    {
        currentCoolDown += Time.deltaTime;
        if(currentCoolDown > Cooldown)
        {
            currentCoolDown = Cooldown;
        }
    }
}
