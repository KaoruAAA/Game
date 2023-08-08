using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterActionManager : MonoBehaviour
{
    private CapsuleCollider cColl;
    
    public List<EventCasterManager> overLapEcasts = new List<EventCasterManager>();
   

    private void Start()
    {
        cColl = GetComponent<CapsuleCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        EventCasterManager[] em = other.GetComponents<EventCasterManager>();

        foreach (var cast in em)
        {
            if(!overLapEcasts.Contains(cast))
            {
                overLapEcasts.Add(cast);
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        EventCasterManager[] em = other.GetComponents<EventCasterManager>();

        foreach (var cast in em)
        {
            if (overLapEcasts.Contains(cast))
            {
                overLapEcasts.Remove(cast);
            }
        }

    }
}
