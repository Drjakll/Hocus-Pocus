using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class Stationary : MonoBehaviour
{
    public float duration;
    public float maxDuration;
    public OnExplosion OnVictim;

    private Transform[] Children;
    private Vector3 maxScale;
    private float MaxRadius = 30;
    private float DmgCycle = 1f;
    
    void Start()
    {
        Children = GetComponentsInChildren<Transform>();
        maxScale = Children[1].localScale;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(duration <= 0)
        {
            Object.Destroy(this.gameObject);
        }
        else
        {
            duration -= Time.deltaTime;
            float scale = (duration / maxDuration);
            foreach (Transform t in Children)
            {
                t.localScale = maxScale * scale;
            }
            Collider[] victims = Physics.OverlapSphere(transform.position, MaxRadius * scale, 512);
            foreach(Collider vic in victims)
            {
                CharacterStatus CharStats = vic.GetComponent<CharacterStatus>();
                if(CharStats != null)
                {
                    CharStats.CurrentSlowedDuration += 1.75f * Time.deltaTime;
                    if (duration % DmgCycle > .99f)
                    {
                        OnVictim.Invoke(vic.gameObject, this.gameObject);
                    }
                }
            }
        }
    }
}
