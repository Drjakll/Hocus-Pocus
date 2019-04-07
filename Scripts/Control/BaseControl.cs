using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseControl : MonoBehaviour
{
    public AbstractSpell[] Spells;
    public AbstractSpell[] DefensiveSpells;
    public bool Controllable = true;
    protected Movement movements;
    protected Defense defensives;
    protected MouseControl MouseCtrl;
    protected CharacterStatus Charstats;
}
