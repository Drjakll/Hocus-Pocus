using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    public float Radius;

    [SerializeField] protected GameObject AnimationEffect;

    private float MaxUp = 1f;
    private float MaxDown = .5f;
    private float CurrentLoc = 0;

    public virtual GameObject DetectNearbyCharacter() {
        Collider[] NearbyCharacter = Physics.OverlapSphere(transform.position, Radius, 0b100000011111111111);

        if(NearbyCharacter.Length != 0)
        {
            return NearbyCharacter[0].gameObject;
        }
        return null;
    }

    public void PlayAnimation(GameObject GO)
    {
        GameObject Animation = Instantiate(AnimationEffect);
        Animation.transform.parent = GO.transform;
        Animation.transform.localPosition = Vector3.zero + (new Vector3(0, 2.1f, 0));
    }

    public void UpAndDown()
    {
        CurrentLoc = (CurrentLoc + Time.deltaTime) % Mathf.PI;
        float Delta = Mathf.Cos(CurrentLoc);
        transform.Translate(new Vector3(0, Delta * Time.deltaTime, 0));
    }

    public abstract void TakeEffect(GameObject CS);

}
