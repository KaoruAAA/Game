using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private WeaponData weaponData;
    public DataBase data;
    // Start is called before the first frame update
    void Awake()
    {
        CheckTag();
    }

    private void Start()
    {
        data = new DataBase();

        //weaponData
        
        
    }

    private void CheckTag()
    {
        if(tag == "GM")
        {
            if(instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);

                return;
            }

            Destroy(this);
        }
        Destroy(this);
    }



    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
