using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public ActorManager am;

    public Collider weaponColL;
    public Collider weaponColR;

    public WeaponController wcL;
    public WeaponController wcR;

    public GameObject whL;
    public GameObject whR;

    private void Start()
    {
        weaponColL = whL.GetComponentInChildren<Collider>();
        weaponColL.enabled = false;

        weaponColR = whR.GetComponentInChildren<Collider>();
        weaponColR.enabled = false;

        wcL = BindWC(whL);
        wcR = BindWC(whR);
        wcL.wm = this;
        wcR.wm = this;
    }

    public WeaponController BindWC(GameObject targetObj)
    {
        WeaponController wc;
        wc = targetObj.GetComponent<WeaponController>();
        if(wc == null)
        {
            wc = targetObj.AddComponent<WeaponController>();
        }
        return wc;
    }
    public void WeaponEnable()
    {
        if(am.ac.CheckTag("attackL"))
        {
            weaponColL.enabled = true;
        }
        else
        {
            weaponColR.enabled = true;
        }
    }

    public void WeaponDisable()
    {
        weaponColL.enabled = false;
        weaponColR.enabled = false;
    }

    public void CounterBackEnable()
    {
        am.sm.counterBackEnable = true;
    }

    public void CounterBackUnEnable()
    {
        am.sm.counterBackEnable = false;
    }
}
