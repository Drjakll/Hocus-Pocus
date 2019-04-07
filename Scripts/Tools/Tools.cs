using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public enum Control { Forward, Backward, Left, Right, Still }
    public enum Rotation { Left, Right, NoRotate }
    public enum Attacks { ShootStraight, NoSpell, MagicLight, PhysicalAttack }
    public enum Status { Healthy, Stunned, Burning, NA1, Slowed, NA2, NA3, NA4, Poisoned }
    public enum Defensive { NotDefensive, Block, DodgeLeft, DodgeRight, Duck }
    public enum DefensiveBuffs { NoBuff, FireShield, IceShield}
    public delegate bool InitMagicObj(GameObject FromWeapon, GameObject Direction);
    public delegate bool OnExplosion(GameObject ObjectHit, GameObject thisObject);
    public delegate void Defensives();
}
