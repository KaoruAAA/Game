using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CameraController : MonoBehaviour
{
    private GameObject mainCamera;
    private PlayerInput pi;
    private GameObject modleA;
    public Image lockPoint;

    public float cameraDemp = 0.1f;
    public float rotateSpeedx = 50;
    public float rotateSpeedy = 30;

    public List<GameObject> targetList = new List<GameObject>();

    private Transform playerHandle;
    private Transform cameraHandle;
    
    public GameObject lockTarget;//������Ŀ��
    public int targetIndex = 0;

    public bool isAI;//�Ƿ�ai����


    private float offsetTime;

    private float vertical;//�����ֱ��ת

    private Vector3 cameraDempVelocity;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main.gameObject;
        cameraHandle = transform.parent.GetComponent<Transform>();
        playerHandle = cameraHandle.parent.GetComponent<Transform>();
        modleA = playerHandle.GetComponent<ActorController>().modle.gameObject;
        if (!isAI)
        {
            pi = playerHandle.GetComponent<PlayerInput>();
            lockPoint.enabled = false;
            Time.timeScale = 1;
        }
        else
        {
            pi = playerHandle.GetComponent<DummyInput>();
        }
        targetList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pi.lockon)
        {
            LockUnLock();
        }

        

        if (lockTarget == null)
        {
            //��¼������ת
            Vector3 modEuler = modleA.transform.eulerAngles;
            offsetTime = playerHandle.GetComponent<ActorController>().offsetTime;
            
            //��ת��Ϸ����
            playerHandle.Rotate(Vector3.up, -pi.Jright * rotateSpeedx * offsetTime );

            //***************************��תע��
            vertical -= pi.JUp * rotateSpeedy * offsetTime;
            vertical = Mathf.Clamp(vertical, -40f, 30f);

            //cameraHandle.transform.localEulerAngles = new Vector3(vertical, 0f, 0f);

            //��ԭ������ת
            if(!isAI)
            modleA.transform.eulerAngles = modEuler;
        }
        else
        {
            //�л�����
            if (pi.rightLock)
            {
                targetIndex = (++targetIndex) % targetList.Count;
                lockTarget = targetList[targetIndex];
            }

            if(pi.leftLock)
            {
                if (targetIndex == 0) targetIndex = targetList.Count - 1;
                else targetIndex--;

                lockTarget = targetList[targetIndex];
            }
            //lockPoint.transform.position =Camera.main.ViewportToWorldPoint( Camera.main.WorldToViewportPoint(targetList[targetIndex].transform.position)); 
            if (!isAI)
            {
                lockPoint.rectTransform.position = Camera.main.WorldToScreenPoint(targetList[targetIndex].transform.position );
            }
            Vector3 dir = lockTarget.transform.position - modleA.transform.position;        
            dir.y = 0;
            
            playerHandle.transform.forward = Vector3.Lerp(playerHandle.transform.forward, dir, 0.03f);
            //modleA.transform.forward = dir;
        }
        
        
        if (lockTarget == null)
        {
            cameraHandle.transform.localEulerAngles = new Vector3(vertical * Time.timeScale, 0f, 0f);
        }
        //�����λ���������ͬ��---��������������ӳٸ���
        if(!isAI)
        {
            mainCamera.transform.eulerAngles = transform.eulerAngles;
            mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, transform.transform.position, ref cameraDempVelocity, cameraDemp * Time.timeScale);

        }
    }

 

    //��������
    public void LockUnLock()
    {
        
        if (lockTarget == null && targetList.Count > 0)
        {
            //��¼�����±�
            targetIndex = targetList.Count / 2;
            lockTarget = targetList[targetIndex];
            if (!isAI)
            {
                lockPoint.enabled = true;
            }
            
        }
        else
        {
            targetIndex = -1;
            lockTarget = null;
            if (!isAI)
            {
                lockPoint.enabled = false;
            }
        }
    }

    //�����б�����
   public List<GameObject> EnemySort(List<GameObject> list)
    {
        int flag = 1;
        
        for(int i = 0; i < list.Count; i++)
        {
            for(int j = 0; j < list.Count - i - 1; j++)
            {
                if(Camera.main.WorldToViewportPoint(list[j].transform.position).x > Camera.main.WorldToViewportPoint(list[j+1].transform.position).x)
                {
                    GameObject A = list[j];
                    list[j] = list[j + 1];
                    list[j + 1] = A;
                    flag = 0;
                }
            }

            if(flag == 1){
                 break;
            }
        }
        return list;
    }

    

}
