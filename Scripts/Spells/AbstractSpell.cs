using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class AbstractSpell : MonoBehaviour
{
    [SerializeField] protected GameObject SpellPrefab;
    [SerializeField] protected GameObject SpecialEffect;
    
    public Sprite SpellImage;
    public float CurrentVelocity = 0;
    public float Cooldown = 1;
    public float currentCoolDown = 1;
    public float MaxDamage = 10;
    public float MinDamage = 4;
    public float Cost = 0;
    public bool GlobalCoolDown = false;
    public Vector3 CastAtPosition;

    protected string CasterName;
    protected float EffectDuration;

    public virtual void Init() { }
    public virtual Attacks CastSpell() { return Attacks.NoSpell; }
    public virtual bool InitMagicalObject(GameObject Weapon, GameObject Direction) { return false; }
    public virtual void UpdateCoolDown() { currentCoolDown = 1; }

    public virtual bool ShieldHandlerFromFrost(GameObject ObjHit)
    {
        CharacterStatus Charstats = ObjHit.GetComponentInParent<CharacterStatus>();
        switch (ObjHit.layer)
        {
            case 13:
                if (Charstats != null)
                {
                    Charstats.CurrentHealth -= Random.Range(MinDamage, MaxDamage) * .5f;
                    return true;
                }
                break;
            case 14:
                if (Charstats != null)
                {
                    Charstats.CurrentFireDuration += EffectDuration * .25f;
                    Charstats.CurrentHealth -= Random.Range(MinDamage, MaxDamage) * .25f;
                    if (ObjHit.GetComponentInParent<CharacterStatus>().GetComponentInChildren<Chilled>() == null)
                    {
                        GameObject frostEffect = Instantiate(SpecialEffect);
                        frostEffect.transform.position = ObjHit.transform.position;
                        frostEffect.transform.parent = ObjHit.transform;
                    }
                    Charstats.CurrentStats |= (short) Status.Slowed;
                    return true;
                }
                break;
        }
        return false;
    }

    public virtual bool ShieldHandlerFromFire(GameObject ObjHit)
    {
        CharacterStatus Charstats = ObjHit.GetComponentInParent<CharacterStatus>();
        switch (ObjHit.layer)
        {
            case 13:
                if (Charstats != null)
                {
                    Charstats.CurrentFireDuration += EffectDuration * .5f;
                    Charstats.CurrentHealth -= Random.Range(MinDamage, MaxDamage) * .25f;
                    if (ObjHit.GetComponentInParent<CharacterStatus>().GetComponentInChildren<Burning>() == null)
                    {
                        GameObject burnEffect = Instantiate(SpecialEffect);
                        burnEffect.transform.position = ObjHit.transform.position + (new Vector3(0, 3f, 0));
                        burnEffect.transform.parent = ObjHit.transform;
                    }
                    Charstats.CurrentStats |= (short)Status.Burning;
                    return true;
                }
                break;
            case 14:
                if (Charstats != null)
                {
                    Charstats.CurrentFireDuration += EffectDuration * .25f;
                    Charstats.CurrentHealth -= Random.Range(MinDamage, MaxDamage) * .5f;
                    if (ObjHit.GetComponentInParent<CharacterStatus>().GetComponentInChildren<Burning>() == null)
                    {
                        GameObject burnEffect = Instantiate(SpecialEffect);
                        burnEffect.transform.position = ObjHit.transform.position + (new Vector3(0, 3f, 0));
                        burnEffect.transform.parent = ObjHit.transform;
                    }
                    Charstats.CurrentStats |= (short)Status.Burning;
                    return true;
                }
                break;
        }
        return false;
    }

    public virtual CharacterStatus ShieldHandlerFromArcane(GameObject ObjHit) {
        CharacterStatus Charstats = ObjHit.GetComponentInParent<CharacterStatus>();
        switch (ObjHit.layer)
        {
            case 13:
                if (Charstats != null)
                {
                    return Charstats;
                }
                break;
            case 14:
                if (Charstats != null)
                {
                    return Charstats;
                }
                break;
        }
        return null;
    }
}
