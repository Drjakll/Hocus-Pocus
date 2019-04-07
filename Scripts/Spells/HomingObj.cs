using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class HomingObj : MonoBehaviour
{
    private CharacterController SpellCtrl;

    public string Caster;
    public float Duration;
    public OnExplosion onExplosion;
    public CharacterStatus Target;
    public bool Go = false;

    [SerializeField] float Speed = 25;
    [SerializeField] GameObject Explosion;
    [SerializeField] float TurningSpeed = 50;

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
            Alignment();
            Vector3 MoveForward = transform.forward * Speed * Time.deltaTime;
            SpellCtrl.Move(MoveForward);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        GameObject Explode = Instantiate(Explosion);
        Explode.transform.position = this.transform.position;
        onExplosion.Invoke(hit.gameObject, this.gameObject);
        Object.Destroy(this.gameObject);
    }

    public bool Alignment()
    {
        Vector3 ThisObjPos = transform.position;
        Vector3 TargetPos = Target.transform.position;
        Vector2 DirTowardTarget = new Vector2(TargetPos.x - ThisObjPos.x, TargetPos.z - ThisObjPos.z).normalized;
        Vector2 ThisObjDir = new Vector2(transform.forward.x, transform.forward.z).normalized;
        float DirDifference = (DirTowardTarget - ThisObjDir).magnitude;
        if (DirDifference < .0075f)
            return true;

        float Angle = Mathf.Acos(ThisObjDir.x);
        Angle = ThisObjDir.y >= 0 ? Angle : 2 * Mathf.PI - Angle;
        float PlusAngle = (Angle + .017f) % (2 * Mathf.PI);
        float MinusAngle = (Angle - .017f) % (2 * Mathf.PI);

        Vector2 Plus = new Vector2(Mathf.Cos(PlusAngle), Mathf.Sin(PlusAngle));
        Vector2 Minus = new Vector2(Mathf.Cos(MinusAngle), Mathf.Sin(MinusAngle));

        if ((Plus - DirTowardTarget).magnitude < (Minus - DirTowardTarget).magnitude)
        {
            transform.Rotate(new Vector3(0, -TurningSpeed * Time.deltaTime, 0), Space.Self);
        }
        else
        {
            transform.Rotate(new Vector3(0, TurningSpeed * Time.deltaTime, 0), Space.Self);
        }
        return false;
    }
}
