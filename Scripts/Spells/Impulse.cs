using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class Impulse : MonoBehaviour
{
    public OnExplosion wave;

    private List<CharacterStatus> TargetsAffected;
    private ParticleSystem PS;

    // Start is called before the first frame update
    void Start()
    {
        PS = GetComponentInChildren<ParticleSystem>();
        TargetsAffected = new List<CharacterStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PS.time <= 1.0f)
        {
            float currentTimePlayTime = PS.time;
            Collider[] victims = Physics.OverlapSphere(transform.position, 20 * currentTimePlayTime, 512);
            Status[] status = new Status[2];
            status[0] = Status.Stunned;
            status[1] = Status.Burning;
            foreach (Collider v in victims)
            {
                CharacterStatus CS = v.GetComponent<CharacterStatus>();
                if (!CS.RecentlyImpacted)
                {
                    if (!wave.Invoke(v.gameObject, this.gameObject))
                        continue;
                    CS.ReactionToHit(this.gameObject, status);
                    CS.RecentlyImpacted = true;
                    TargetsAffected.Add(CS);
                }
            }
        }
        else if(PS.time >= 4.5f)
        {
            foreach(CharacterStatus CS in TargetsAffected)
            {
                CS.RecentlyImpacted = false;
            }
            Object.Destroy(this.gameObject);
        }
    }
}
