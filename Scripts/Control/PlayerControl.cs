using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Tools;

public class PlayerControl : BaseControl
{
    [SerializeField] Image SpellImageHolder;
    [SerializeField] Image[] UnuseSpellImages;
    [SerializeField] Image[] DefensiveSpellImages;
    [SerializeField] GameObject LocationPointer;
    
    private AbstractSpell EquippedSpell;
    private Sprite EquippedSpellImage;
    private Vector2 CameraScreenDim;

    void Start()
    {
        EquippedSpell = Spells[0];
        movements = GetComponent<Movement>();
        defensives = GetComponent<Defense>();
        MouseCtrl = GetComponent<MouseControl>();
        movements.EquippedSpell = EquippedSpell;
        foreach(AbstractSpell AS in Spells)
        {
            AS.Init();
        }
        foreach (AbstractSpell AS in DefensiveSpells)
        {
            AS.Init();
        }
        CameraScreenDim = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
        SpellImageHolder.transform.position = new Vector2(CameraScreenDim.x * .075f, CameraScreenDim.y * .125f);
        SpellImageHolder.sprite = Spells[0].SpellImage;
        for (int i = 0; i < UnuseSpellImages.Length; i++)
        {
            UnuseSpellImages[i].transform.position = new Vector2(CameraScreenDim.x * (float)(.175f + (float)i * .05f), CameraScreenDim.y * .1f);
        }
        for(int j = 0; j < DefensiveSpellImages.Length; j++)
        {
            DefensiveSpellImages[j].transform.position = new Vector2(CameraScreenDim.x * (float)(.175f + (float)(j + 1) * .05f), CameraScreenDim.y * .21f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Controllable)
        {
            if (Input.GetMouseButtonDown(0) && !movements.wasAttacking)
            {
                if (EquippedSpell.currentCoolDown >= EquippedSpell.Cooldown && movements.IsDoneAnimatingAttack())
                {
                    EquippedSpell.CurrentVelocity = GetComponent<CharacterController>().velocity.magnitude;
                    movements.EquippedSpell = EquippedSpell;
                    movements.attacks = EquippedSpell.CastSpell();
                    movements.ShootMagicalObj = EquippedSpell.InitMagicalObject;
                    if(MouseCtrl.mousePointer != null)
                    {
                        EquippedSpell.CastAtPosition = MouseCtrl.mousePointer.transform.position;
                    }
                }
            }
            else
            {
                movements.attacks = Attacks.NoSpell;
            }

            int input = 0;
            if (Input.GetKey(KeyCode.W))
            {
                input |= 1;
            }
            if (Input.GetKey(KeyCode.S))
            {
                input |= 2;
            }
            if (Input.GetKey(KeyCode.Q))
            {
                input |= 4;
            }
            if (Input.GetKey(KeyCode.E))
            {
                input |= 8;
            }

            HandleHorVert(input);

            if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                movements.rotate = Rotation.Left;
            }
            else if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            {
                movements.rotate = Rotation.Right;
            }
            else
            {
                movements.rotate = Rotation.NoRotate;
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                defensives.defense = Defensive.DodgeLeft;
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                defensives.defense = Defensive.DodgeRight;
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                defensives.defense = Defensive.Duck;
            }
            else if (Input.GetKeyDown(KeyCode.Space))
            {
                defensives.defense = Defensive.Block;
            }
            else
            {
                defensives.defense = Defensive.NotDefensive;
            }
        }
        else
        {
            movements.rotate = Rotation.NoRotate;
            movements.control = Control.Still;
            movements.attacks = Attacks.NoSpell;
            defensives.defense = Defensive.NotDefensive;
        }

        foreach(AbstractSpell AS in Spells)
        {
            AS.UpdateCoolDown();
        }
        foreach(AbstractSpell AS in DefensiveSpells)
        {
            if (AS.GlobalCoolDown)
            {
                foreach(AbstractSpell DS in DefensiveSpells)
                {
                    DS.currentCoolDown = 0;
                }
                AS.GlobalCoolDown = false;
                break;
            }
            AS.UpdateCoolDown();
        }
    }

    private void HandleHorVert(int input)
    {
        switch (input)
        {
            case 1:
                movements.control = Control.Forward;
                break;
            case 2:
                movements.control = Control.Backward;
                break;
            case 4:
                movements.control = Control.Left;
                break;
            case 8:
                movements.control = Control.Right;
                break;
            default:
                movements.control = Control.Still;
                break;
        }
    }

    private void OnGUI()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquippedSpell = Spells[0];
            SpellImageHolder.sprite = Spells[0].SpellImage;
            if (MouseCtrl.mousePointer != null)
            {
                Object.Destroy(MouseCtrl.mousePointer);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquippedSpell = Spells[1];
            SpellImageHolder.sprite = Spells[1].SpellImage;
            if (MouseCtrl.mousePointer != null)
            {
                Object.Destroy(MouseCtrl.mousePointer);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            EquippedSpell = Spells[2];
            SpellImageHolder.sprite = Spells[2].SpellImage;
            if(MouseCtrl.mousePointer != null)
            {
                Object.Destroy(MouseCtrl.mousePointer);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            EquippedSpell = Spells[3];
            SpellImageHolder.sprite = Spells[3].SpellImage;
            if (MouseCtrl.mousePointer != null)
            {
                Object.Destroy(MouseCtrl.mousePointer);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            EquippedSpell = Spells[4];
            SpellImageHolder.sprite = Spells[4].SpellImage;
            if(MouseCtrl.mousePointer == null)
                MouseCtrl.mousePointer = Instantiate(LocationPointer);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            EquippedSpell = Spells[5];
            SpellImageHolder.sprite = Spells[5].SpellImage;
            if (MouseCtrl.mousePointer != null)
                Object.Destroy(MouseCtrl.mousePointer);
        }
        else if (Input.GetKeyDown(KeyCode.F1))
        {
            EquippedSpell = DefensiveSpells[0];
            SpellImageHolder.sprite = DefensiveSpells[0].SpellImage;
            if (MouseCtrl.mousePointer != null)
            {
                Object.Destroy(MouseCtrl.mousePointer);
            }
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            EquippedSpell = DefensiveSpells[1];
            SpellImageHolder.sprite = DefensiveSpells[1].SpellImage;
            if (MouseCtrl.mousePointer != null)
            {
                Object.Destroy(MouseCtrl.mousePointer);
            }
        }

        for (int i = 0; i < Spells.Length; i ++)
        {
            UnuseSpellImages[i].fillAmount = Spells[i].currentCoolDown / Spells[i].Cooldown;
        }
        for (int i = 0; i < DefensiveSpellImages.Length; i++)
        {
            DefensiveSpellImages[i].fillAmount = DefensiveSpells[i].currentCoolDown / DefensiveSpells[i].Cooldown;
        }

        SpellImageHolder.fillAmount = EquippedSpell.currentCoolDown / EquippedSpell.Cooldown;
    }
}
