using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyInput : PlayerInput
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        while(true)
        {
            Dup = 1f;
            Dright = 0;
            JUp = 0;
            Jright = 1;
            run = true;
            yield return new WaitForSeconds(3);
            Dup = 0f;
            Dright = 0;
            JUp = 0;
            Jright = 0;
            run = false;
            yield return new WaitForSeconds(1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateDmagDvec(Dup, Dright); 
        transform.forward = Vector3.forward;
        rAttack = true;
    }
}
