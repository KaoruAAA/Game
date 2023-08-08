using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("�����ת����")]
    public KeyCode keyUp;
    public KeyCode keyDown;
    public KeyCode keyRight;
    public KeyCode keyLeft;
    [Header("����")]
    public KeyCode keyLAttack;
    public KeyCode keyRAttack;
    public KeyCode keyDefense;
    public KeyCode keyCounterBack;
    public KeyCode keyAction;
    [Header("��������")]
    public KeyCode keyLockOn;
    public KeyCode keyLeftChange;
    public KeyCode keyRightChange;
    

    //�ƶ�����
    public float Dup;
    public float Dright;
    public float JUp;
    public float Jright;
    //״̬�ж�
    public bool run;
    public bool jump;
    //public bool attack;//����
    public bool lAttack;
    public bool rAttack;
    public bool defense;//�ٶ�
    public bool counterBack;//����
    public bool lockon;//����
    public bool leftLock;//�л�����
    public bool rightLock;//ͬ��
    public bool onAction;

    public bool inputEnable = true;
    
    //�ƶ��������
    public float Dmag;
    public Vector3 Dvec;

    private float targetDup;
    private float targetDright;
    private float velocityDup;
    private float velocityDright;

    private ActorController ac;
    private CameraController camCon;

    public float adjustValue = 1;//����ֵ��ʱ�������
    
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
        //�ƶ�����
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

        //˿���ƶ�
        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f * adjustValue);
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f * adjustValue);//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        //����õ�α��һ�������ٲ�ת��������
        Vector2 tempDAxis = SquareToCircle(new Vector2(Dright, Dup));
        float Dright2 = tempDAxis.x;
        float Dup2 = tempDAxis.y;

        //���������л�����״̬
        Dmag = Mathf.Sqrt(Dup2 * Dup2 + Dright2 * Dright2);
        Dvec = Dup2 * transform.forward + Dright2 * transform.right;
        

        //�����ת����
        CameraRotate();
    }

    //б���ƶ��ᵼ�����ֱ�����ù�ʽ����α��һ��
    private Vector2 SquareToCircle(Vector2 input)
    {
        Vector2 output = Vector2.zero;

        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);

        return output;
    }

    //�����ת����
    public void CameraRotate()
    {
        
        JUp = (Input.GetKey(keyUp) ? 1.0f : 0.0f) - (Input.GetKey(keyDown) ? 1.0f : 0.0f);
        Jright = (Input.GetKey(keyLeft) ? 1.0f : 0.0f) - (Input.GetKey(keyRight) ? 1.0f : 0.0f);   
    }

    protected void UpdateDmagDvec(float Dup2, float Dright2)
    {
        //���������л�����״̬
        Dmag = Mathf.Sqrt(Dup2 * Dup2 + Dright2 * Dright2);
        Dvec = Dup2 * transform.forward + Dright2 * transform.right;
    }
}

