using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public ActorManager am;

    public float HP = 20;
    public float HPMax = 20;
    public float basicATK = 5;

    [Header("State")]
    public bool isJump;
    public bool isFall;
    public bool isGround;
    public bool isRoll;
    public bool isAttack;
    public bool isHit;
    public bool isDie;
    public bool isDefense;
    public bool isBlocked;
    public bool isCounterBack;
    public bool counterBackEnable;
    public bool isJab;
    public bool isAction;

    public bool isAllowDefense;
    public bool isImmortal;

    // Start is called before the first frame update
    private void Start()
    {
        am = GetComponent<ActorManager>();

        HPMax = HP;

    }

    private void Update()
    {
        isJump = am.ac.CheckState("Jump");
        isFall = am.ac.CheckState("Fall");
        isGround = am.ac.CheckState("Ground");
        isRoll = am.ac.CheckState("Roll");
        isAttack = am.ac.CheckTag("attackR") || am.ac.CheckTag("attackL");
        isHit = am.ac.CheckState("hit");
        isDie = am.ac.CheckState("die");
        isBlocked = am.ac.CheckState("blocked");
        isCounterBack = am.ac.CheckState("counterBack");
        isJab = am.ac.CheckState("Jab");
        isAction = am.ac.CheckState("lock");

        isAllowDefense = isGround || isBlocked;
        isDefense =isAllowDefense && am.ac.CheckState("defense1h", "defence");

        isImmortal = isRoll || isJab;
    }


    public void TakeDamage(float value)
    {
        HP += value;
        HP = Mathf.Clamp(HP, 0, HPMax); 
    }

   

    
}
