using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class Defense : MonoBehaviour
{
    public Defensive defense = Defensive.NotDefensive;
    public bool BeingDefensive = false;

    private Movement movements;
    private Animator animator;
    private BaseControl baseControl;
    private CharacterController charCtrl;
    private float CurrentHeightCoeff = 1.0f;
    private float OriginalHeight;
    private bool Animating = false;
    private Defensives defensive;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        baseControl = GetComponent<BaseControl>();
        charCtrl = GetComponent<CharacterController>();
        movements = GetComponent<Movement>();
        OriginalHeight = charCtrl.height;
        defensive = NoDefense;
    }

    // Update is called once per frame
    void Update()
    {
        if (!movements.wasAttacking)
        {
            switch (defense)
            {
                case Defensive.Block:
                    BeingDefensive = true;
                    animator.SetTrigger("Block");
                    baseControl.Controllable = false;
                    defensive = Block;
                    break;
                case Defensive.DodgeLeft:
                    BeingDefensive = true;
                    animator.SetTrigger("DodgeLeft");
                    baseControl.Controllable = false;
                    defensive = Dodge;
                    break;
                case Defensive.DodgeRight:
                    BeingDefensive = true;
                    animator.SetTrigger("DodgeRight");
                    baseControl.Controllable = false;
                    defensive = Dodge;
                    break;
                case Defensive.Duck:
                    BeingDefensive = true;
                    animator.SetTrigger("Duck");
                    baseControl.Controllable = false;
                    defensive = Duck;
                    break;
                case Defensive.NotDefensive:
                    break;
            }
            defensive.Invoke();
        }
    }

    private void NoDefense()
    {
    }

    private void Duck()
    {
        if(animator.GetCurrentAnimatorClipInfo(3).Length > 0 && animator.GetCurrentAnimatorStateInfo(3).normalizedTime <= 1.0f)
        {
            if(animator.GetCurrentAnimatorStateInfo(3).normalizedTime <= .5f)
            {
                CurrentHeightCoeff = 1 - animator.GetCurrentAnimatorStateInfo(3).normalizedTime;
            }
            else
            {
                CurrentHeightCoeff = animator.GetCurrentAnimatorStateInfo(3).normalizedTime;
            }
            charCtrl.height = OriginalHeight * CurrentHeightCoeff;
            Animating = true;
        }
        else if (Animating)
        {
            charCtrl.height = OriginalHeight;
            Animating = false;
            baseControl.Controllable = true;
            defensive = NoDefense;
            BeingDefensive = false;
        }
    }

    private void Dodge()
    {
        if (animator.GetCurrentAnimatorClipInfo(3).Length > 0 && animator.GetCurrentAnimatorStateInfo(3).normalizedTime <= 1.0f)
        {
            Animating = true;
        }
        else if (Animating)
        {
            Animating = false;
            baseControl.Controllable = true;
            defensive = NoDefense;
            BeingDefensive = false;
        }
    }

    private void Block()
    {
        if (animator.GetCurrentAnimatorClipInfo(3).Length > 0 && animator.GetCurrentAnimatorStateInfo(3).normalizedTime <= 1.0f)
        {
            Animating = true;
        }
        else if (Animating)
        {
            Animating = false;
            baseControl.Controllable = true;
            defensive = NoDefense;
            BeingDefensive = false;
        }
    }
}
