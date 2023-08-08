using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionControl : MonoBehaviour
{
    private Animator anim;
    private ActorController ac;
    public Vector3 a = Vector3.zero;
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        ac = GetComponentInParent<ActorController>();
    }

    void OnAnimatorMove()
    {
        SendMessageUpwards("OnUpDateRM", (object)anim.deltaPosition);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if((ac.leftIsShield && anim.GetBool("defense") || ac.CheckState("blocked")) == false)
        {
            Transform leftLowerArm = anim.GetBoneTransform(HumanBodyBones.LeftLowerArm);
            leftLowerArm.localEulerAngles += a;
            anim.SetBoneLocalRotation(HumanBodyBones.LeftLowerArm, Quaternion.Euler(leftLowerArm.localEulerAngles));
        }
    }
}
