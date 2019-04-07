using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class AIController : BaseControl
{
    // Start is called before the first frame update
    [SerializeField] GameObject Aim;

    private AbstractSpell EquippedSpell;
    private Animator animator;

    public GameObject Target;

    void Start()
    {
        movements = GetComponent<Movement>();
        defensives = GetComponent<Defense>();
        MouseCtrl = GetComponent<MouseControl>();
        Charstats = GetComponent<CharacterStatus>();
        animator = GetComponent<Animator>();
        Target = MouseCtrl.SelectedObject;
        EquippedSpell = Spells[0];
        movements.EquippedSpell = Spells[0];
        foreach (AbstractSpell AS in Spells)
        {
            AS.Init();
        }
        foreach (AbstractSpell AS in DefensiveSpells)
        {
            AS.Init();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Controllable)
        {
            movements.attacks = Attacks.NoSpell;

            RaycastHit ObjectInfront;
            float DistanceFromEnemy = (transform.position - Target.transform.position).magnitude;

            Collider[] DangerSurrounding = Physics.OverlapSphere(transform.position, 30, 0b10000000000000000);
            Collider[] IncomingObj = Physics.OverlapSphere(transform.position, 30, 0b1001100000000000);
            Collider[] DetectPowerUps = Physics.OverlapSphere(transform.position, 50, 0b1000000000000000000);

            MagicShield MS = GetComponentInChildren<MagicShield>();

            if (movements.control == Control.Forward && Physics.SphereCast(new Ray(transform.position, transform.forward), 6, out ObjectInfront, 2, ~512))
            {
                movements.rotate = GoAround(ObjectInfront.transform.gameObject);
            }
            else if (DetectPowerUps.Length > 0)
            {
                Transform PowerUpTrans = DetectPowerUps[0].transform;
                Vector2 PowerUpDir = (new Vector2(PowerUpTrans.position.x - transform.position.x, PowerUpTrans.position.z - transform.position.z)).normalized;
                movements.rotate = Alignment(PowerUpDir, this.gameObject);
                if(movements.rotate == Rotation.NoRotate)
                {
                    movements.control = Control.Forward;
                }
                else
                {
                    movements.control = Control.Still;
                }
            }
            else if (DangerSurrounding.Length > 0 || Charstats.CurrentMana < EquippedSpell.Cost || Charstats.CurrentHealth < 150)
            {
                Debug.Log(Charstats.CurrentMana + " : " + EquippedSpell.Cost);
                if (DistanceFromEnemy < 15)
                    movements.rotate = RunAwayFromEnemy(Target.transform);
                else if (DistanceFromEnemy >= 15)
                    movements.rotate = CircleAroundEnemy(Target.transform);

                foreach (Collider Danger in DangerSurrounding)
                {
                    movements.rotate = RunAwayFromEnemy(Danger.gameObject.transform);
                }

                movements.control = Control.Forward;
            }
            else if(IncomingObj.Length > 0)
            {
                foreach (Collider IncObj in IncomingObj)
                {
                    MovingObject spellObj = IncObj.GetComponent<MovingObject>();
                    HomingObj homingSpell = IncObj.GetComponent<HomingObj>();
                    if (spellObj != null && spellObj.CasterName == this.gameObject.name)
                    {
                        continue;
                    }
                    if (homingSpell != null && homingSpell.Caster == this.gameObject.name)
                    {
                        continue;
                    }
                    switch (IncObj.gameObject.layer)
                    {
                        case 11:
                            if (DefensiveSpells[1].currentCoolDown < DefensiveSpells[1].Cooldown && movements.IsDoneAnimatingAttack())
                                defensives.defense = AvoidMissiles(IncObj.gameObject);
                            else if (MS == null || (MS != null && MS.gameObject.layer != 14))
                            {
                                EquippedSpell = DefensiveSpells[1];
                                CastSpell();
                            }
                            break;
                        case 12:
                            if (DefensiveSpells[0].currentCoolDown < DefensiveSpells[0].Cooldown && movements.IsDoneAnimatingAttack())
                                defensives.defense = AvoidMissiles(IncObj.gameObject);
                            else if (MS == null || (MS != null && MS.gameObject.layer != 13))
                            {
                                EquippedSpell = DefensiveSpells[0];
                                CastSpell();
                            }
                            break;
                        case 15:
                            if (movements.IsDoneAnimatingAttack())
                                defensives.defense = AvoidMissiles(IncObj.gameObject);
                            break;
                    }
                }
            }
            else if(animator.GetCurrentAnimatorClipInfo(3).Length == 0)
            {
                Vector2 EnemyDir = (new Vector2(Target.transform.position.x - transform.position.x, Target.transform.position.z - transform.position.z)).normalized;
                movements.rotate = Alignment(EnemyDir, Aim);
                GameObject Obstacle = null;

                if (DistanceFromEnemy < 15 && Spells[2].currentCoolDown >= Spells[2].Cooldown)
                {
                    EquippedSpell = Spells[2];
                    CastSpell();
                }
                else if (movements.rotate == Rotation.NoRotate)
                {
                    Vector2 EnemyFacingVec = (new Vector2(Target.transform.forward.x, Target.transform.forward.z)).normalized;
                    float EnemyFacingAngle = Mathf.Acos(Vector2.Dot(EnemyDir, EnemyFacingVec));
                    Vector3 RayDir = (new Vector3(Target.transform.position.x - Aim.transform.position.x, 0, Target.transform.position.z - Aim.transform.position.z)).normalized;
                    RaycastHit Rayhit;

                    Debug.DrawRay(Aim.transform.position, RayDir * 40f);

                    if (Physics.Raycast(new Ray(Aim.transform.position + Aim.transform.right / 2, RayDir), out Rayhit, 40f, 0b100000000000000000))
                    {
                        Obstacle = Rayhit.transform.gameObject;
                    }
                    else if (EnemyFacingAngle < Mathf.PI * .4f)
                    {
                        if (Spells[4].currentCoolDown >= Spells[4].Cooldown)
                        {
                            EquippedSpell = Spells[4];
                            EquippedSpell.CastAtPosition = Target.transform.position;
                            CastSpell();
                        }
                        else
                        {
                            EquippedSpell = Spells[3];
                            CastSpell();
                        }
                    }
                    else
                    {
                        EquippedSpell = Spells[1];
                        CastSpell();
                    }
                }

                if(Obstacle != null)
                {
                    movements.control = AvoidObstacle(Obstacle);
                }
                else if (DistanceFromEnemy < 30)
                {
                    movements.control = Control.Backward;
                }
                else if(DistanceFromEnemy > 40)
                {
                    movements.control = Control.Forward;
                }
                else
                {
                    movements.control = Control.Still;
                }
            }
        }
        else
        {
            movements.control = Control.Still;
            movements.rotate = Rotation.NoRotate;
            movements.attacks = Attacks.NoSpell;
            defensives.defense = Defensive.NotDefensive;
        }

        foreach (AbstractSpell AS in Spells)
        {
            AS.UpdateCoolDown();
        }
        foreach (AbstractSpell AS in DefensiveSpells)
        {
            if(AS.GlobalCoolDown)
            {
                foreach (AbstractSpell DS in DefensiveSpells)
                {
                    DS.currentCoolDown = 0;
                }
                AS.GlobalCoolDown = false;
                break;
            }
            AS.UpdateCoolDown();
        }
    }

    private void CastSpell()
    {
        if (EquippedSpell.currentCoolDown >= EquippedSpell.Cooldown && movements.IsDoneAnimatingAttack())
        {
            EquippedSpell.CurrentVelocity = GetComponent<CharacterController>().velocity.magnitude;
            movements.EquippedSpell = EquippedSpell;
            movements.attacks = EquippedSpell.CastSpell();
            movements.ShootMagicalObj = EquippedSpell.InitMagicalObject;
        }
    }

    private Rotation CircleAroundEnemy(Transform EnemyLoc)
    {
        Vector2 EnemyDir = (new Vector2(EnemyLoc.position.x - transform.position.x, EnemyLoc.position.z - transform.position.z).normalized);
        Vector2 PerpendicularToEnemy = (new Vector2(-EnemyDir.y, EnemyDir.x));
        return Alignment(PerpendicularToEnemy, this.gameObject);
    }

    private Rotation RunAwayFromEnemy(Transform EnemyLoc)
    {
        Vector2 FaceOppEnemyDir = -(new Vector2(EnemyLoc.position.x - transform.position.x, EnemyLoc.position.z - transform.position.z).normalized);
        return Alignment(FaceOppEnemyDir, this.gameObject);
    }

    private Rotation Alignment(Vector2 AlignDir, GameObject Self)
    {
        Vector2 ThisObjDir = new Vector2(Self.transform.forward.x, Self.transform.forward.z).normalized;
        float DirDifference = (AlignDir - ThisObjDir).magnitude;
        if (DirDifference < .05f)
            return Rotation.NoRotate;

        float Angle = Mathf.Acos(ThisObjDir.x);
        Angle = ThisObjDir.y >= 0 ? Angle : 2 * Mathf.PI - Angle;
        float PlusAngle = (Angle + .017f) % (2 * Mathf.PI);
        float MinusAngle = (Angle - .017f) % (2 * Mathf.PI);

        Vector2 Plus = new Vector2(Mathf.Cos(PlusAngle), Mathf.Sin(PlusAngle));
        Vector2 Minus = new Vector2(Mathf.Cos(MinusAngle), Mathf.Sin(MinusAngle));

        if ((Plus - AlignDir).magnitude < (Minus - AlignDir).magnitude)
        {
            return Rotation.Left;
        }
        else
        {
            return Rotation.Right;
        }
    }

    private Rotation GoAround(GameObject Obstacle)
    {
        //Debug.Log("going around");
        if (Obstacle == null)
            return Rotation.NoRotate;
        Vector2 ObstacleDir = (new Vector2(Obstacle.transform.position.x - transform.position.x, Obstacle.transform.position.z - transform.position.z)).normalized;
        Vector2 RightSide = (new Vector2(transform.right.x, transform.right.z)).normalized;
        Vector2 Forward = (new Vector2(transform.forward.x, transform.forward.z)).normalized;
        if ((Forward - ObstacleDir).magnitude > Mathf.Sqrt(2))
        {
            return Alignment(ObstacleDir, this.gameObject);
        }
        float Angle = Mathf.Acos(Vector2.Dot(ObstacleDir, RightSide));
        return Angle >= Mathf.PI / 2 ? Rotation.Right : Rotation.Left;
    }

    private Defensive AvoidMissiles(GameObject IncomingObj)
    {
        if (IncomingObj == null)
            return Defensive.NotDefensive;
        Vector2 ObstacleDir = (new Vector2(IncomingObj.transform.forward.x, IncomingObj.transform.forward.z)).normalized;
        Vector2 RightSide = (new Vector2(transform.right.x, transform.right.z)).normalized;
        float Angle = Mathf.Acos(Vector2.Dot(ObstacleDir, RightSide));
        Defensive defensive;
        if (Angle < Mathf.PI * .55f && Angle > Mathf.PI * .5f)
            defensive = Defensive.Duck;
        else
            defensive = Angle >= Mathf.PI * .55f ? Defensive.DodgeRight : Defensive.DodgeLeft;
        return defensive;
    }

    private Control AvoidObstacle(GameObject Obstacle)
    {
        Vector2 ObstacleLoca = new Vector2(Obstacle.transform.position.x - transform.position.x, Obstacle.transform.position.z - transform.position.z);
        Vector2 ObstacleDir = ObstacleLoca.normalized;
        Vector2 FacingDir = (new Vector2(transform.forward.x, transform.forward.z)).normalized;
        float Angle = Mathf.Acos(Vector2.Dot(ObstacleDir, FacingDir));
        if(Angle <= Mathf.PI / 2)
        {
            return Control.Left;
        }
        else
        {
            return Control.Right;
        }
    }
}
