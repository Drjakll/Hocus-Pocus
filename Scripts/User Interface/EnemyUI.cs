using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    [SerializeField] IndicatorLogic Statusbar;
    [SerializeField] Canvas UI;

    public float scale = .5f;
    public bool Selected = false;

    private CharacterStatus CharStatus;
    
    // Start is called before the first frame update
    void Start()
    {
        UI.targetDisplay = 1;
        CharStatus = GetComponent<CharacterStatus>();
        Statusbar.gameObject.transform.position = new Vector2(Camera.main.pixelWidth - 300, Camera.main.pixelHeight - 25);
    }

    private void OnGUI()
    {
        if (Selected)
        {
            UI.targetDisplay = 0;
            Statusbar.topBar.Value = CharStatus.CurrentHealth / CharStatus.MaximumHealth;
            Statusbar.downBar.Value = CharStatus.CurrentMana / CharStatus.MaximumMana;
        }
        else
        {
            UI.targetDisplay = 1;
        }
    }
}
