using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsCameraVisible : MonoBehaviour
{

    public GameObject cameraPoint;

    private StateManager sm;
    private CameraController camCon;
    private Vector3 viewPos;
    private GameObject currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        camCon = cameraPoint.GetComponent<CameraController>();
        sm = GetComponent<StateManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
        //�������Ƿ����������
        viewPos = Camera.main.WorldToViewportPoint(transform.position);
        if (viewPos.x > 0 && viewPos.x < 1 && viewPos.y > 0 && viewPos.y < 1 && viewPos.z < 30 && (sm == false || !sm.isDie))
        {
            
            //���û����ӵ��б������
            if (!camCon.targetList.Find(s => s.gameObject == this.gameObject))
            {
                camCon.targetList.Add(this.gameObject);
                //�������������Ŀ�꣬��¼����
                currentTarget = (camCon.targetIndex == -1) ? null : camCon.targetList[camCon.targetIndex].gameObject;
                camCon.targetList = camCon.EnemySort(camCon.targetList);
                //��������ҵ�ԭ������Ŀ����±�
                for (int i = 0; i < camCon.targetList.Count; i++)
                {
                    if(camCon.targetList[i] == currentTarget)
                    {
                        camCon.targetIndex = i;
                        break;
                    }
                }
            }
        }
        
        else
        {
            if(camCon.targetList.Find(s=> s.gameObject == this.gameObject))
            { 
                //�������������Ŀ�꣬��¼����
                currentTarget = (camCon.targetIndex == -1) ? null : camCon.targetList[camCon.targetIndex].gameObject;
                camCon.targetList.Remove(gameObject);
                camCon.targetList = camCon.EnemySort(camCon.targetList);
                //��������ҵ�ԭ������Ŀ����±�
                for (int i = 0; i < camCon.targetList.Count; i++)
                {
                    if (camCon.targetList[i] == currentTarget)
                    {
                        camCon.targetIndex = i;
                        break;
                    }
                }


                if (camCon.lockTarget == this.gameObject)
                {
                    camCon.lockTarget = null;
                    camCon.lockPoint.enabled = false;
                    camCon.targetIndex = -1;
                }
            }
            
            if (sm != null && sm.isDie)
            {
                this.enabled = false;
            }

        }
    }

    private void OnDestroy()
    {
        if (camCon.targetList.Find(s => s.gameObject == this.gameObject))
        {
            //�������������Ŀ�꣬��¼����
            currentTarget = (camCon.targetIndex == -1) ? null : camCon.targetList[camCon.targetIndex].gameObject;
            camCon.targetList.Remove(gameObject);
            camCon.targetList = camCon.EnemySort(camCon.targetList);
            //��������ҵ�ԭ������Ŀ����±�
            for (int i = 0; i < camCon.targetList.Count; i++)
            {
                if (camCon.targetList[i] == currentTarget)
                {
                    camCon.targetIndex = i;
                    break;
                }
            }


            if (camCon.lockTarget == this.gameObject)
            {
                camCon.lockTarget = null;
                camCon.lockPoint.enabled = false;
                camCon.targetIndex = -1;
            }
        }
    }
}
