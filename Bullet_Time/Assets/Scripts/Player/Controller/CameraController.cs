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
    
    public GameObject lockTarget;//锁定的目标
    public int targetIndex = 0;

    public bool isAI;//是否ai控制


    private float offsetTime;

    private float vertical;//相机竖直旋转

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
            //记录人物旋转
            Vector3 modEuler = modleA.transform.eulerAngles;
            offsetTime = playerHandle.GetComponent<ActorController>().offsetTime;
            
            //旋转游戏物体
            playerHandle.Rotate(Vector3.up, -pi.Jright * rotateSpeedx * offsetTime );

            //***************************旋转注意
            vertical -= pi.JUp * rotateSpeedy * offsetTime;
            vertical = Mathf.Clamp(vertical, -40f, 30f);

            //cameraHandle.transform.localEulerAngles = new Vector3(vertical, 0f, 0f);

            //还原人物旋转
            if(!isAI)
            modleA.transform.eulerAngles = modEuler;
        }
        else
        {
            //切换锁定
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
        //摄像机位置随摄像点同步---方便设置摄像机延迟跟随
        if(!isAI)
        {
            mainCamera.transform.eulerAngles = transform.eulerAngles;
            mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, transform.transform.position, ref cameraDempVelocity, cameraDemp * Time.timeScale);

        }
    }

 

    //锁定敌人
    public void LockUnLock()
    {
        
        if (lockTarget == null && targetList.Count > 0)
        {
            //记录敌人下标
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

    //敌人列表排序
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
