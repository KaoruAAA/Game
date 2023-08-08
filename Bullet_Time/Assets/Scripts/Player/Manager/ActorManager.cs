using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager : MonoBehaviour
{
    public ActorController ac;
    public BattleManager bm;
    public WeaponManager wm;
    public StateManager sm;
    public DirectorManager dm;
    public InterActionManager im;

    public Vector3 offset;

    // Start is called before the first frame update
    void Awake()
    {
        ac = GetComponent<ActorController>();

        bm = GetComponentInChildren<BattleManager>();

        wm = ac.modle.GetComponent<WeaponManager>();

        sm = GetComponent<StateManager>();

        dm = GetComponent<DirectorManager>();

        im = GetComponentInChildren<InterActionManager>();



        bm.am = this;
        wm.am = this;

        ac.OnAction += OnAction1;
    }

    public void OnAction1()
    {
        //获取第一个碰到的交互物体
        if(im.overLapEcasts.Count != 0)
        {
            Debug.Log("不为0");
            //玩家与交互物体基本面对面的时候
            if (BattleManager.InPlayerAngle(ac.modle, im.overLapEcasts[0].gameObject, 50))
            {
                Debug.Log("在角度内");

               
                //该交互物体可以交互的状态下执行
                if (im.overLapEcasts[0].active == true)
                {
                    ac.modle.transform.forward = -im.overLapEcasts[0].transform.forward;
                    
                    Debug.Log("可执行");
                    //如果交互物体是箱子，则设为只能执行一次
                    if (im.overLapEcasts[0].eventName == "openBox")
                    {
                        ac.gameObject.transform.position = im.overLapEcasts[0].transform.position + offset;
                        im.overLapEcasts[0].active = false;
                    }
                    Debug.Log("执行交互");
                    dm.PlayTimeline(im.overLapEcasts[0].eventName, this, im.overLapEcasts[0].am);
                
                    
                
                } 
            }            
        }
    }
  

    public void DoDamage(WeaponController targetWc)
    {
        
        if(sm.isCounterBack && sm.counterBackEnable)
        {
            bm.targetWc.wm.am.Stunned();
        }
        //无敌状态
        else if (sm.isImmortal)
        {

        }
        //防御状态
        else if (sm.isDefense)
        {
            Blocked();
        }
        //受伤
        else
        {
            sm.TakeDamage(-1 * (targetWc.WATK() + sm.basicATK));
            if(sm.HP > 0)
            {
                Hit(); 
            }
            else
            {
                Die();
            }

        }
    }

    public void Stunned()
    {
        ac.anim.SetTrigger("stunned");
    }

    public void Blocked()
    {
        ac.anim.SetTrigger("blocked");
    }
    public void Hit()
    {
        ac.anim.SetTrigger("hit");
    }

    public void Die()
    {
        ac.anim.SetTrigger("die");

        ac.pi.inputEnable = false;

        if(ac.camCon.lockTarget != null)
        {
            ac.camCon.LockUnLock();
        }
        
    }

    public void LockUnLockAC(string name, bool value)
    {
        ac.anim.SetBool(name, value);

        ac.pi.inputEnable = !value;

        ac.lockPlanar = !value;

    }

}
