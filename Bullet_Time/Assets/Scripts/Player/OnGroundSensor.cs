using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundSensor : MonoBehaviour
{
    public CapsuleCollider coll;

    private Vector3 point1;
    private Vector3 point2;
    private float radius;
    // Start is called before the first frame update
    void Start()
    {
        radius = coll.radius;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        

        point1 = transform.position + transform.up * radius;
        point2 = transform.position + transform.up * coll.height - transform.up * radius;
        Collider[] onGroundColl = Physics.OverlapCapsule(point1, point2, radius + 0.3f, LayerMask.GetMask("Ground"));
        

        if (onGroundColl.Length != 0)
        {
            SendMessageUpwards("IsGround");
        }
        else
        {
            SendMessageUpwards("IsNotGround");
        }

       
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(point1, radius + 0.3f);
    //    Gizmos.DrawSphere(point2, radius + 0.3f);
    //}


}
