using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("相机旋转输入")]
    public KeyCode keyUp;
    public KeyCode keyDown;
    public KeyCode keyRight;
    public KeyCode keyLeft;
    [Header("攻击")]
    public KeyCode keyLAttack;
    public KeyCode keyRAttack;
    public KeyCode keyDefense;
    public KeyCode keyCounterBack;
    public KeyCode keyAction;
    [Header("敌人锁定")]
    public KeyCode keyLockOn;
    public KeyCode keyLeftChange;
    public KeyCode keyRightChange;
    

    //移动数字
    public float Dup;
    public float Dright;
    public float JUp;
    public float Jright;
    //状态判断
    public bool run;
    public bool jump;
    //public bool attack;//攻击
    public bool lAttack;
    public bool rAttack;
    public bool defense;//举盾
    public bool counterBack;//弹反
    public bool lockon;//锁定
    public bool leftLock;//切换锁定
    public bool rightLock;//同上
    public bool onAction;

    public bool inputEnable = true;
    
    //移动处理变量
    public float Dmag;
    public Vector3 Dvec;

    private float targetDup;
    private float targetDright;
    private float velocityDup;
    private float velocityDright;

    private ActorController ac;
    private CameraController camCon;

    public float adjustValue = 1;//调整值（时间比例）
    
    // Start is called before the first frame update
    void Start()
    {
        ac = GetComponent<ActorController>();
        camCon = GetComponentInChildren<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!camCon.isAI)
            adjustValue = Time.timeScale;
        else
            adjustValue = 1;
        //移动输入
        targetDup = Input.GetAxisRaw("Vertical");
        targetDright = Input.GetAxisRaw("Horizontal");

        if (inputEnable == false)
        {
            targetDup = 0;
            targetDright = 0;
            lockon = false;
            leftLock = false;
            rightLock = false;
            jump = false;
            run = false;
            counterBack = false;
        }

        onAction = Input.GetKeyDown(keyAction);
        
        lockon = Input.GetKeyDown(keyLockOn);

        leftLock = Input.GetKeyDown(keyLeftChange);

        rightLock = Input.GetKeyDown(keyRightChange);

        jump = Input.GetKeyDown(KeyCode.Space);

        run = Input.GetKey(KeyCode.LeftShift);

        counterBack = Input.GetKeyDown(keyCounterBack);
        

        rAttack = Input.GetKey(keyRAttack);

        if (!ac.leftIsShield) 
            lAttack = Input.GetKeyDown(keyLAttack);
        else
            lAttack = Input.GetKey(keyLAttack);
        //defense = Input.GetKey(keyDefense);
        defense = lAttack;

        //丝滑移动
        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f * adjustValue);
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f * adjustValue);//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        //将获得的伪归一化向量再拆开转化成数字
        Vector2 tempDAxis = SquareToCircle(new Vector2(Dright, Dup));
        float Dright2 = tempDAxis.x;
        float Dup2 = tempDAxis.y;

        //根据数字切换动画状态
        Dmag = Mathf.Sqrt(Dup2 * Dup2 + Dright2 * Dright2);
        Dvec = Dup2 * transform.forward + Dright2 * transform.right;
        

        //相机旋转输入
        CameraRotate();
    }

    //斜线移动会导致数字变大，利用公式将其伪归一化
    private Vector2 SquareToCircle(Vector2 input)
    {
        Vector2 output = Vector2.zero;

        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);

        return output;
    }

    //相机旋转输入
    public void CameraRotate()
    {
        
        JUp = (Input.GetKey(keyUp) ? 1.0f : 0.0f) - (Input.GetKey(keyDown) ? 1.0f : 0.0f);
        Jright = (Input.GetKey(keyLeft) ? 1.0f : 0.0f) - (Input.GetKey(keyRight) ? 1.0f : 0.0f);   
    }

    protected void UpdateDmagDvec(float Dup2, float Dright2)
    {
        //根据数字切换动画状态
        Dmag = Mathf.Sqrt(Dup2 * Dup2 + Dright2 * Dright2);
        Dvec = Dup2 * transform.forward + Dright2 * transform.right;
    }
}

