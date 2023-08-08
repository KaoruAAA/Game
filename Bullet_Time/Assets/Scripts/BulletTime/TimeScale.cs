using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeScale : MonoBehaviour
{
    public Transform cube;
    public Transform sphere;

    public Transform c_point;
    public Transform s_point;

    public Slider slider;
    public Text text;

    public float speed;

    bool s_isforward = true;
    bool c_isforward = true;



    float offsetRealTime = 0;

    float realTime =0;


    // Start is called before the first frame update
    void Start()
    {
        realTime = Time.realtimeSinceStartup;
    }

    // Update is called once per frame
    void Update()
    {
        //人工Time.deltaTime
        offsetRealTime = Time.realtimeSinceStartup - realTime;
        realTime = Time.realtimeSinceStartup;


        if (Vector3.Distance(cube.position, c_point.position) <= 1)
            c_isforward = false;
        if (Vector3.Distance(cube.position, new Vector3(0, 2, 0)) <= 1)
            c_isforward = true;


        if (Vector3.Distance(sphere.position, s_point.position) <= 1)
            s_isforward = false;
        if (Vector3.Distance(sphere.position, new Vector3(0, 0, 0)) <= 1)
            s_isforward = true;



        Move();
        RealTimeMove();

        //Debug.Log(Time.deltaTime + "   " + offsetRealTime);
    }

    //利用time。deltatime的移动
    void Move()
    {
        if (s_isforward)
        {
            sphere.Translate(0, 0, Time.deltaTime * speed);
        }
        if (!s_isforward)
        {
            sphere.Translate(0, 0, -Time.deltaTime * speed);
        }
    }

    //利用realTime的移动
    void RealTimeMove()
    {
        if(c_isforward)
        {
            cube.Translate(0, 0, offsetRealTime * speed);
        }
        if(!c_isforward)
        {
            cube.Translate(0, 0, -offsetRealTime * speed);
        }
    }

    //滑动改变时间尺度
    public void TimeScaleChange()
    {
        Time.timeScale = slider.value;
        text.text = slider.value.ToString();
    }

    //点击--子弹时间
    public void BulletTime()
    {
        StartCoroutine(ShowTime());
    }

    IEnumerator ShowTime()
    {
        float timer = 0;

        Time.timeScale = 0.3f;
        slider.value = 0.3f;
        Debug.Log(timer);

        
        yield return new WaitForSeconds(3*Time.timeScale);

        Debug.Log("0");

       
            Time.timeScale = 1;
            slider.value = 1;
            yield return 1;
        
    }
}
