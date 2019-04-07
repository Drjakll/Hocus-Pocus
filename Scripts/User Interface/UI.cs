using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] IndicatorLogic Statusbar;
    [SerializeField] Camera PersonalCamera;

    public float scale = 2;

    private CharacterStatus PlayerCharStatus;
    private RenderTexture rendertexture;

    // Start is called before the first frame update
    void Start()
    {
        rendertexture = PersonalCamera.targetTexture;
        PlayerCharStatus = GetComponent<CharacterStatus>();
        Statusbar.gameObject.transform.position = new Vector2(25, Camera.main.pixelHeight - 25);
    }

    private void OnGUI()
    {
        Statusbar.topBar.Value = PlayerCharStatus.CurrentHealth / PlayerCharStatus.MaximumHealth;
        Statusbar.downBar.Value = PlayerCharStatus.CurrentMana / PlayerCharStatus.MaximumMana;
    }
}
