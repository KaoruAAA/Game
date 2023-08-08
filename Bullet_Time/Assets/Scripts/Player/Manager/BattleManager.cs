using System.Collections;
using System.Collections.Generic;
using UnityEditor.AnimatedValues;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class BattleManager : MonoBehaviour
{
    public WeaponController targetWc;

    public ActorManager am;

    public CapsuleCollider defCol;

   

    private void Start()
    {
        am = GetComponentInParent<ActorManager>();

        defCol = GetComponent<CapsuleCollider>();
        defCol.center = Vector3.up;
        defCol.radius = 0.25f;
        defCol.height = 2;
        defCol.isTrigger = true;
    }
    private void OnTriggerEnter(Collider other)
    {


        if(other.tag == "Weapon" && am.sm.HP>0)
        {
            targetWc = other.GetComponentInParent<WeaponController>();
            GameObject attacker = targetWc.wm.am.gameObject;
            GameObject player = am.ac.modle;
            
            if(InTargetAngle(player, attacker, 50))
                am.DoDamage(targetWc);
        }
    }

    //判断是否在玩家规定角度内
    public static bool InPlayerAngle(GameObject player, GameObject target, float targetAngle)
    {
        
        Vector3 dir = target.transform.position - player.transform.position;
        dir.y = player.transform.forward.y;
        float angle = Vector3.Angle(player.transform.forward, dir);
        return (angle < targetAngle);
    }

    //判断是否在目标规定角度内
    public static bool InTargetAngle(GameObject player, GameObject target, float targetAngle)
    {
        Vector3 dir = player.transform.position - target.transform.position;

        float angle = Vector3.Angle(target.transform.forward, dir);

        return (angle < targetAngle);
    }
}
