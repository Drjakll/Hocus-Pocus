using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class HomingSphere : AbstractSpell
{
    public override void Init()
    {
        Cooldown = 2;
        currentCoolDown = 10;
        MaxDamage = 50;
        MinDamage = 35;
        Cost = 30;
        EffectDuration = 7;
    }

    public override Attacks CastSpell()
    {
        return Attacks.ShootStraight;
    }

    public override bool InitMagicalObject(GameObject Weapon, GameObject Direction)
    {
        CharacterStatus Charstats = Weapon.GetComponentInParent<CharacterStatus>();
        GameObject target = Charstats.GetComponent<MouseControl>().SelectedObject;
        if (Charstats.CurrentMana >= Cost && target != null)
        {
            GameObject Sphere = Instantiate(SpellPrefab);
            Sphere.transform.position = Weapon.transform.position - Weapon.transform.right * 2;
            Sphere.transform.LookAt(Direction.transform);
            HomingObj homingSphere = Sphere.GetComponent<HomingObj>();
            homingSphere.onExplosion = OnExplosion;
            homingSphere.Duration = EffectDuration;
            homingSphere.Target = target.GetComponent<CharacterStatus>();
            homingSphere.Caster = Charstats.gameObject.name;
            homingSphere.Go = true;
            Charstats.CurMana = -Cost;
            return true;
        }
        return false;
    }

    public bool OnExplosion(GameObject ObjHit, GameObject ThisObj)
    {
        CharacterStatus CharStats = ShieldHandlerFromArcane(ObjHit);
        if (CharStats == null)
            CharStats = ObjHit.GetComponent<CharacterStatus>();
        if (ObjHit.name == CasterName)
            return false;
        if (CharStats != null)
        {
            Status[] status = new Status[1];
            status[0] = Status.Stunned;
            CharStats.CurrentFireDuration += EffectDuration;
            CharStats.CurHealth = -Random.Range(MinDamage, MaxDamage);
            CharStats.ReactionToHit(ThisObj, status);
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
