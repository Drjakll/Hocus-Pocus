using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class Movement : MonoBehaviour
{
    [SerializeField] float RunSpeed = 20;
    [SerializeField] float SideSpeed = 5;
    [SerializeField] float BackwardSpeed = 5;
    [SerializeField] float RotateSpeed = 100;
    [SerializeField] GameObject Weapon;
    [SerializeField] GameObject Direction;

    public Control control = Control.Still;
    public Rotation rotate = Rotation.NoRotate;
    public Attacks attacks = Attacks.NoSpell;
    public InitMagicObj ShootMagicalObj;
    public AbstractSpell EquippedSpell;
    public bool wasAttacking;

    private Animator animator;
    private CharacterController CharCtrl;
    private float MaxRunSpeed = 20;
    private float MaxSideSpeed = 5;
    private float MaxBackwardSpeed = 5;
    private float MaxRotateSpeed = 100;
    private Defense defensive;
    private Control CurrentMovement = Control.Still;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        CharCtrl = GetComponent<CharacterController>();
        wasAttacking = false;
        defensive = GetComponent<Defense>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!defensive.BeingDefensive && EquippedSpell.currentCoolDown >= EquippedSpell.Cooldown)
        {
            switch (attacks)
            {
                case Attacks.ShootStraight:
                    if (IsDoneAnimatingAttack())
                    {
                        animator.SetTrigger("ShootStraight");
                        wasAttacking = true;
                    }
                    break;
                case Attacks.MagicLight:
                    if (IsDoneAnimatingAttack())
                    {
                        animator.SetTrigger("MagicLight");
                        wasAttacking = true;
                    }
                    break;
                case Attacks.PhysicalAttack:
                    if (IsDoneAnimatingAttack())
                    {
                        int AttackNumber = Random.Range(1, 4);
                        animator.SetInteger("PhysicalAttack", AttackNumber);
                        wasAttacking = true;
                    }
                    break;
                case Attacks.NoSpell:
                    if (wasAttacking && ReadyToFire())
                    {
                        if (ShootMagicalObj.Invoke(Weapon, Direction))
                        {
                            EquippedSpell.currentCoolDown = 0;
                        }
                        animator.SetInteger("PhysicalAttack", 0);
                        wasAttacking = false;
                    }
                    break;
            }
        }

        if (CurrentMovement != control)
        {
            animator.SetBool("RunForward", false);
            animator.SetBool("WalkBackward", false);
            animator.SetBool("MovingRight", false);
            animator.SetBool("MovingLeft", false);
            switch (control)
            {
                case Control.Forward:
                    animator.SetBool("RunForward", true);
                    break;
                case Control.Backward:
                    animator.SetBool("WalkBackward", true);
                    break;
                case Control.Left:
                    animator.SetBool("MovingLeft", true);
                    break;
                case Control.Right:
                    animator.SetBool("MovingRight", true);
                    break;
                case Control.Still:
                    break;
            }
            CurrentMovement = control;
        }

        switch (control)
        {
            case Control.Forward:
                CharCtrl.Move(transform.forward * RunSpeed * Time.deltaTime);
                break;
            case Control.Backward:
                CharCtrl.Move(-transform.forward * BackwardSpeed * Time.deltaTime);
                break;
            case Control.Left:
                CharCtrl.Move(-transform.right * Time.deltaTime * SideSpeed);
                break;
            case Control.Right:
                CharCtrl.Move(transform.right * Time.deltaTime * SideSpeed);
                break;
            case Control.Still:
                break;
        }

        CharCtrl.Move(Physics.gravity);


        switch (rotate)
        {
            case Rotation.Left:
                transform.Rotate(0, -Time.deltaTime * RotateSpeed, 0, Space.Self);
                break;
            case Rotation.Right:
                transform.Rotate(0, Time.deltaTime * RotateSpeed, 0, Space.Self);
                break;
        }
    }

    public bool ReadyToFire()
    {
        return animator.GetCurrentAnimatorClipInfo(1).Length > 0 && animator.GetCurrentAnimatorStateInfo(1).normalizedTime >= 0.5f;
    }

    public bool IsDoneAnimatingAttack()
    {
        return animator.GetCurrentAnimatorClipInfo(1).Length == 0;
    }

    public void SetAnimationSpeed(float AnimeSpeed)
    {
        animator.speed = AnimeSpeed;
        animator.SetLayerWeight(2, AnimeSpeed);
        RunSpeed = MaxRunSpeed * AnimeSpeed;
        SideSpeed = MaxSideSpeed * AnimeSpeed;
        BackwardSpeed = MaxBackwardSpeed * AnimeSpeed;
        RotateSpeed = MaxRotateSpeed * AnimeSpeed;
    }
}
