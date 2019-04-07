
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;
using TMPro;

public class CharacterStatus : MonoBehaviour
{
    private BaseControl charController;
    private Movement movements;
    private bool StartAnimation = false;
    private Animator Anim;
    private const float FireDamageOverTime = 1f;
    private const float PsnDamageOverTime = 10;
    private const float MaxSlowedDuration = 15;
    
    [SerializeField] TextMeshPro TMP;

    public float MaximumHealth = 1000;
    public float MaximumMana = 300;
    public float CurrentHealth = 1000;
    public float CurrentMana = 300;
    public short CurrentStats = 0;
    public float CurrentPsnDuration = 0;
    public float CurrentFireDuration = 0;
    public float CurrentSlowedDuration = 0;
    public bool RecentlyImpacted = false;

    public float MaxHealth {

        set {
            TMP.color = Color.green;
            TMP.text = "Max Health + " + ((int)value).ToString();
            GameObject PopText = Instantiate(TMP.gameObject);
            PopText.transform.parent = transform;
            PopText.transform.localPosition = Vector3.zero + (new Vector3(0, 2.2f, 0));
            MaximumHealth = MaximumHealth + value;
        }
        get { return MaximumHealth; }
    }

    public float CurHealth
    {
        set {
            if(value < 0) 
                TMP.color = Color.red;
            else
                TMP.color = Color.green;
            TMP.text = ((int)value).ToString();
            GameObject PopText = Instantiate(TMP.gameObject);
            PopText.transform.parent = transform;
            PopText.transform.localPosition = Vector3.zero + (new Vector3(0, 2.2f, 0));
            CurrentHealth += value;
        }
        get { return CurrentHealth; }
    }

    public float MaxMana
    {
        set {
            TMP.color = Color.blue;
            TMP.text = "Max Mana + " + ((int)value).ToString();
            GameObject PopText = Instantiate(TMP.gameObject);
            PopText.transform.parent = transform;
            PopText.transform.localPosition = Vector3.zero + (new Vector3(0, 2.2f, 0));
            MaximumMana = MaximumMana + value;
        }
        get { return MaximumMana; }
    }

    public float CurMana
    {
        set {
            TMP.color = Color.blue;
            TMP.text = ((int)value).ToString();
            GameObject PopText = Instantiate(TMP.gameObject);
            PopText.transform.parent = transform;
            PopText.transform.localPosition = Vector3.zero + (new Vector3(0, 2.2f, 0));
            CurrentMana += value;
        }
        get { return CurrentMana; }
    }

    // Start is called before the first frame update
    void Start()
    {
        charController = GetComponent<BaseControl>();
        movements = GetComponent<Movement>();
        Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentHealth < MaximumHealth)
        {
            CurrentHealth += Time.deltaTime * 5;
            if(CurrentHealth <= 0)
            {
                Anim.SetBool("Dead", true);
                CharacterController CharCtrler = GetComponent<CharacterController>();
                CharCtrler.height = .3f;
                CharCtrler.center = new Vector3(0, .3f, 0);
                CapsuleCollider CapColl = GetComponent<CapsuleCollider>();
                CapColl.height = .3f;
                CapColl.center = new Vector3(0, .3f, 0);
                GetComponent<BaseControl>().Controllable = false;
            }
        }
        else
        {
            CurrentHealth = MaximumHealth;
        }

        if (CurrentMana < MaximumMana)
        {
            CurrentMana += Time.deltaTime;
        }
        else
        {
            CurrentMana = MaximumMana;
        }

        if (CurrentStats > 0)
        {
            for (short StatusCheck = 1; StatusCheck <= 8; StatusCheck = (short)(StatusCheck << 1))
            {
                switch (CurrentStats & StatusCheck)
                {
                    case 1:
                        if(Anim.GetCurrentAnimatorClipInfo(2).Length > 0 && Anim.GetCurrentAnimatorStateInfo(2).normalizedTime <= 1.0f)
                        {
                            if(charController != null)
                            {
                                charController.Controllable = false;
                            }
                            StartAnimation = true;
                        }
                        else if(StartAnimation)
                        {
                            if (charController != null)
                            {
                                charController.Controllable = true;
                            }
                            CurrentStats &= 14;
                            StartAnimation = false;
                        }
                        break;
                    case 2:
                        CurrentFireDuration -= Time.deltaTime;
                        int burnIntensity = (int)(CurrentFireDuration / 2.5);
                        Burning burn = GetComponentInChildren<Burning>();
                        if(burn != null)
                        {
                            burn.ChangeIntensity(burnIntensity);
                        }
                        CurrentHealth -= CurrentFireDuration * FireDamageOverTime * Time.deltaTime;
                        if(CurrentFireDuration <= 0)
                        {
                            CurrentFireDuration = 0;
                            CurrentStats &= 13; 
                        }
                        CurrentSlowedDuration = CurrentSlowedDuration >= 0 ? CurrentSlowedDuration - Time.deltaTime : 0;
                        break;
                    case 4:
                        CurrentSlowedDuration -= Time.deltaTime;
                        if(MaxSlowedDuration < CurrentSlowedDuration)
                        {
                            CurrentSlowedDuration = MaxSlowedDuration;
                        }
                        float ChilledIntensity = CurrentSlowedDuration / MaxSlowedDuration;
                        movements.SetAnimationSpeed( 1 - ChilledIntensity );
                        Chilled chilled = GetComponentInChildren<Chilled>();
                        if(chilled != null)
                        {
                            chilled.ChangeIntensity(ChilledIntensity);
                        }
                        if(CurrentSlowedDuration <= 0 && chilled != null)
                        {
                            CurrentSlowedDuration = 0;
                            chilled.Unchill = true;
                            movements.SetAnimationSpeed(1);
                            CurrentStats &= 11;
                        }
                        break;
                    case 8:
                        break;
                }
            }
        }
    }

    public void ReactionToHit(GameObject HitByObj, Status[] status)
    {
        if (Anim.GetCurrentAnimatorClipInfo(2).Length == 0)
        {
            Vector2 HitFromDirection = -(new Vector2(HitByObj.transform.forward.x, HitByObj.transform.forward.z).normalized);
            float DirInAngle = HitFromDirRespectToChar(HitFromDirection);

            if (DirInAngle < 180)
            {
                if (DirInAngle > 135)
                {
                    Anim.SetTrigger("HitFromLeft");
                }
                else if (DirInAngle < 45)
                {
                    Anim.SetTrigger("HitFromRight");
                }
                else
                {
                    Anim.SetTrigger("HitFromFront");
                }
            }
            else
            {
                if (DirInAngle < 225)
                {
                    Anim.SetTrigger("HitFromLeft");
                }
                else if (DirInAngle > 315)
                {
                    Anim.SetTrigger("HitFromRight");
                }
                else
                {
                    Anim.SetTrigger("HitFromBack");
                }
            }
        }

        for (int i = 1; i < status.Length; i++)
        {
            CurrentStats |= (short)status[i];
        }
    }

    public float HitFromDirRespectToChar(Vector2 hitFromDir)
    {
        float Angle = Mathf.Acos(Vector3.Dot(hitFromDir, new Vector2(transform.right.x, transform.right.z)));
        float FrontBack = Vector3.Dot(hitFromDir, new Vector2(transform.forward.x, transform.forward.z));

        Angle = FrontBack >= 0 ? Angle : 2 * Mathf.PI - Angle; 

        return Angle * Mathf.Rad2Deg;
    }
}
