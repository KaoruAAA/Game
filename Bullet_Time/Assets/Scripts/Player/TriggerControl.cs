using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerControl : MonoBehaviour
{
    public void ResetTrigger(string trigger)
    {
        GetComponent<Animator>().ResetTrigger(trigger);
    }
}
