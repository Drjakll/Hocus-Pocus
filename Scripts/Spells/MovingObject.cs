using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class MovingObject : MonoBehaviour
{
    private CharacterController SpellCtrl;
    private float LifeTime = 0;

    public string CasterName;
    public OnExplosion ExplosionEffect;
    public bool Go = false;
    public float InitVelocity = 0;
    public float MaxDamage = 0;
    public float MinDamage = 0;
    public float Duration = 0;
    
    [SerializeField] float Acceleration = 100;
    [SerializeField] GameObject Explosion;

    // Start is called before the first frame update
    void Start()
    {
        SpellCtrl = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Go)
        {
            LifeTime += Time.deltaTime;
            float CurrentSpeed = LifeTime * Acceleration + InitVelocity;
            Vector3 Displacement = transform.forward * CurrentSpeed * Time.deltaTime;
            SpellCtrl.Move(Displacement);
            if (Mathf.Abs(transform.position.y) > 20)
                Object.Destroy(this.gameObject);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        CharacterStatus Caster = hit.gameObject.GetComponentInParent<CharacterStatus>();
        if (Caster != null && Caster.gameObject.name == CasterName)
        {
            return;
        }
        GameObject Explode = Instantiate(Explosion);
        Explode.transform.position = this.transform.position;
        ExplosionEffect.Invoke(hit.gameObject, this.gameObject);
        Object.Destroy(this.gameObject);
    }
}
