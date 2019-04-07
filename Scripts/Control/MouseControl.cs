using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    public GameObject SelectedObject;
    public GameObject mousePointer;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject Target = DetectObject();
            if(Target != null)
            {
                Light spotLight = Target.GetComponentInChildren<Light>();
                if(spotLight != null)
                {
                    spotLight.enabled = true;
                }
                EnemyUI EUI = Target.GetComponent<EnemyUI>();
                if(EUI != null)
                {
                    EUI.Selected = true;
                }
                SelectedObject = Target;
            }
            else if (SelectedObject != null && Input.GetKey(KeyCode.LeftShift))
            {
                Light spotLight = SelectedObject.GetComponentInChildren<Light>();
                if (spotLight != null)
                {
                    spotLight.enabled = false;
                }
                EnemyUI EUI = SelectedObject.GetComponent<EnemyUI>();
                if (EUI != null)
                {
                    EUI.Selected = false;
                }
                SelectedObject = Target;
            }
        }
        if(mousePointer != null)
        {
            mousePointer.transform.position = MousePointWorldSpace();
        }
    }

    public GameObject DetectObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit HitObj;
        if(Physics.Raycast(ray, out HitObj, 1000.0f))
        {
            CharacterStatus CharStatus = HitObj.transform.GetComponent<CharacterStatus>();
            if (CharStatus != null && CharStatus.gameObject.name != this.gameObject.name)
            {
                return CharStatus.gameObject;
            }
            CharStatus = HitObj.transform.GetComponentInParent<CharacterStatus>();
            if (CharStatus != null && CharStatus.gameObject.name != this.gameObject.name)
            {
                return CharStatus.gameObject;
            }
        }
        return null;
    }

    public Vector3 MousePointWorldSpace()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit HitObj;
        if (Physics.Raycast(ray, out HitObj, 1000.0f, 1024))
        {
            return HitObj.point;
        }
        return Vector3.zero;
    }
}
