using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponManager wm;
    public WeaponData wdt;
    // Start is called before the first frame update
    private void Start()
    {
        wdt = GetComponentInChildren<WeaponData>();
    }

    public float WATK()
    {
        return wdt.ATK;
    }
}
