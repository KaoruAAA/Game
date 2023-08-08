using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public GameObject modle;
    public PhysicMaterial frictionOne;
    public PhysicMaterial frictionZero;

    //组件
    public Animator anim;
    public PlayerInput pi;
    private Rigidbody rig;
    public CameraController camCon;
    public  CapsuleCollider col;

    [Header("===== 属性 =====")]
    public float walkSpeed = 1.5f;
    public float runSpeed = 2.7f;
    public float jumpVelocity = 5f;
    public float rollVelocity = 3f;
    public float jabVelocity = 3f;

    private float lerpTarget = 1.0f;
    public bool lockPlanar = false;
    private bool trackDirection = false;//锁定时的动画方向
    private bool canAttack = true;
    [SerializeField]
    public bool leftIsShield = true;
    private Vector3 deltaPos;

    private Vector3 movingVec;
    private Vector3 thrustVec = new Vector3();

    public float timeScale = 0.5f;

    //人工Time.DeltaTime
    [HideInInspector]
    public float offsetTime;
    [HideInInspector]
    public float realTime;

    public delegate void OnActionDelegate();
    public OnActionDelegate OnAction;

    // Start is called before the first frame update
    void Awake()
    {
        anim = modle.GetComponent<Animator>();
        pi = GetComponent<PlayerInput>();
        rig = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        camCon = GetComponentInChildren<CameraController>();

        if(camCon.isAI)
        {
            pi = GetComponent<DummyInput>();
        }

        realTime = Time.realtimeSinceStartup;
        if (camCon.isAI) offsetTime = Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        

        //pi.defense = true;
        //子弹时间
        if (!camCon.isAI)
        {
            
            if(Input.GetKeyDown(KeyCode.B))
            {
                StartCoroutine(Bullet());
            }
            Time.timeScale = timeScale;
            //人工Time.DeltaTime
            offsetTime = Time.realtimeSinceStartup - realTime;
            realTime = Time.realtimeSinceStartup;
        }
        

        //人物移动切换
        if (camCon.lockTarget == null)
        {
            anim.SetFloat("forward", pi.Dmag * Mathf.Lerp(anim.GetFloat("forward"), ((pi.run) ? 2.0f : 1.0f), 0.3f));
            anim.SetFloat("right", 0);
            
        }
        else
        {
            Vector3 localDvec = transform.InverseTransformVector(pi.Dvec); 
            anim.SetFloat("forward", localDvec.z * ((pi.run) ? 2.0f : 1.0f));
            anim.SetFloat("right", localDvec.x * ((pi.run) ? 2.0f : 1.0f));
        }


        //防御
        if (leftIsShield)
        {
            if (CheckState("Ground") || CheckState("defense1h", "defence"))
            {
                anim.SetBool("defense", pi.defense);

                if (pi.defense) anim.SetLayerWeight(anim.GetLayerIndex("defence"), 1.0f);
                else anim.SetLayerWeight(anim.GetLayerIndex("defence"), 0);
            }
            else
            {
                anim.SetBool("defense", false);
            }

        }
        else
        {
            anim.SetLayerWeight(anim.GetLayerIndex("defence"), 0);
        }

        //盾反
        if((CheckState("Ground") || CheckState("defense1h", "defence") || CheckTag("attackR")) && pi.counterBack)
        {
            anim.SetTrigger("counterBack");
        }

        //翻滚
        if (rig.velocity.magnitude > 0f){ 
            anim.SetTrigger("Roll");
        }

        //跳跃
        if (pi.jump) { 
            anim.SetTrigger("Jump");
            canAttack = false;
        }

        //攻击
        if ((pi.lAttack || pi.rAttack) && canAttack && (CheckState("Ground") || CheckTag("attackR") || CheckTag("attackL")) && anim.GetBool("IsGround")) {
            if (pi.lAttack && !leftIsShield)
            {
                anim.SetBool("R0L1", true);
                anim.SetTrigger("attack");
            }
            
            if (pi.rAttack)
            {
                anim.SetBool("R0L1", false);
                anim.SetTrigger("attack");
            }
            
        }
       
        //移动数值
        if (camCon.lockTarget == null)
        {
            if(pi.inputEnable == true)
            {
                if(pi.Dmag > 0.1f)
                    modle.transform.forward = Vector3.Slerp(modle.transform.forward, pi.Dvec, 0.2f * pi.adjustValue / timeScale);

            }
                //跳跃锁定方向
                if(lockPlanar == false)
                    movingVec = pi.Dmag * modle.transform.forward * walkSpeed * ((pi.run)? runSpeed : 1.0f);
            
        }
        else
        {
            if (trackDirection)
            {
                modle.transform.forward = movingVec.normalized;
            }
            else
                modle.transform.forward = transform.forward;

            if (lockPlanar == false)
                movingVec = pi.Dvec  * walkSpeed * ((pi.run) ? runSpeed : 1.0f);

            
        }

        //执行交互
        if (pi.onAction)
        {
            OnAction();
        }

        //角色移动方向
        Vector3 angle = Quaternion.Euler(0, -camCon.transform.eulerAngles.y , 0) * new Vector3(movingVec.x, rig.velocity.y, movingVec.z);
        transform.Translate(deltaPos);
        
       //角色移动
        if(camCon.isAI)
            rig.velocity = new Vector3(movingVec.x, rig.velocity.y, movingVec.z) + thrustVec;
        else
        {
            //rig.velocity = (new Vector3(movingVec.x, rig.velocity.y * timeScale, movingVec.z)) / timeScale + thrustVec;
            Vector3 target = Quaternion.Euler(0, -transform.eulerAngles.y, 0) * (new Vector3(movingVec.x, rig.velocity.y / timeScale, movingVec.z) + thrustVec);
            transform.Translate(target * offsetTime);
        }
       
        thrustVec = Vector3.zero;
        deltaPos = Vector3.zero;
    
        
        //if (camCon.isAI) offsetTime = Time.deltaTime;
    }
    

    public bool CheckState(string stateName, string layerMask = "Base Layer")
    {
        return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerMask)).IsName(stateName);
    }

    public bool CheckTag(string stateName, string layerMask = "Base Layer")
    {
        return anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex(layerMask)).IsTag(stateName);
    }

    /// <summary>
    /// Message Processing block 信息处理模块
    /// </summary>
    public void OnJumpEnter()
    {
        canAttack = false;
        pi.inputEnable = false;
        lockPlanar = true;
        trackDirection = true;

        //if(camCon.isAI)
        //dif(camCon.isAI)
            thrustVec = new Vector3(0, jumpVelocity*1.8f, 0);

    }

    public void OnJumpUpdate()
    {
        if (!camCon.isAI)
            thrustVec = new Vector3(0, jumpVelocity  , 0) * 1.5f;
    }
    
    public void IsGround()
    {
        anim.SetBool("IsGround",true);
       
    }

    public void IsNotGround()
    {
        anim.SetBool("IsGround", false);
    }

    public void OnGroundEnter()
    {
        pi.inputEnable = true;
        lockPlanar = false;
        trackDirection = false;

        canAttack = true;

        col.material = frictionOne;
    }

    public void OnGroundExit()
    {
        col.material = frictionZero;
    }

    public void OnFallEnter()
    {
        pi.inputEnable = false;
        lockPlanar = true;
        
    }

    public void OnRollEnter()
    {
        pi.inputEnable = false;
        lockPlanar = true;
        trackDirection = true;
        thrustVec = new Vector3(0, rollVelocity,0);
    }

    public void OnRollUpdate()
    {
        if (camCon.isAI)
            rig.velocity += modle.transform.forward * 2;
        else
            thrustVec = modle.transform.forward * anim.GetFloat("rollHeight") * 3 ;
        //transform.position += modle.transform.forward * offsetTime * 2;
    }

    public void OnJabEnter()
    {
        pi.inputEnable = false;
        lockPlanar = true;
        
    }

    public void OnJabUpdate()
    {
        thrustVec = modle.transform.forward * anim.GetFloat("JabVelocity") * 2;
    }

    public void OnAttack1hAEnter()
    {
        pi.inputEnable = false;
        //anim.SetLayerWeight(anim.GetLayerIndex("attack"), 1.0f);
        lerpTarget = 1.0f;
    }

    public void OnAttack1hAUpdate()
    {
        
        thrustVec = modle.transform.forward * anim.GetFloat("attack1hAVelocity");
        //float currentWeight = anim.GetLayerWeight(anim.GetLayerIndex("attack"));
        //currentWeight = Mathf.Lerp(currentWeight, lerpTarget, 0.4f);
        //anim.SetLayerWeight(anim.GetLayerIndex("attack"), currentWeight);

        
    }

    public void OnAttackExit()
    {
        modle.SendMessage("WeaponDisable");
    }

    public void OnAttackIdleEnter()
    {
        pi.inputEnable = true;
        //anim.SetLayerWeight(anim.GetLayerIndex("attack"), 0.0f);
        //lerpTarget = 0;
    }

    public void OnHitEnter()
    {
        pi.inputEnable = false;
        movingVec = Vector3.zero;
    }

    public void OnDieEnter()
    {
        movingVec = Vector3.zero;
    }

    public void OnBlockedEnter()
    {
        pi.inputEnable = false;
    }

    public void OnStunnedEnter()
    {
        movingVec = Vector3.zero;
        pi.inputEnable = false;
    }

    public void OncounterBackEnter()
    {
        movingVec = Vector3.zero;
        pi.inputEnable = false;
    }

    public void OncounterBackUpdate()
    {
        pi.defense = false;
        anim.SetBool("defense", false);
        
    }

    public void OnUpDateRM(object _rootPos)
    {
        if(CheckState("attack1hC"))
        {
            deltaPos += (Vector3)_rootPos;

            deltaPos = Quaternion.Euler(0, -transform.eulerAngles.y  , 0) * (Vector3)_rootPos;
        }
    }

    public void IssueTrigger(string triggerName)
    {
        anim.SetTrigger(triggerName);
    }

    //Test
    IEnumerator  Bullet()
    {
        timeScale = 0.2f;
        yield return new WaitForSeconds(3 * timeScale);
        timeScale = 1;
    }
}


