using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using UnityEditor.Experimental.GraphView;


[RequireComponent(typeof(PlayableDirector))]
public class DirectorManager : MonoBehaviour
{
    public ActorManager am;
    public PlayableDirector pd;


    [Header("=== Timeline Assets ===")]
    public TimelineAsset frontStab;
    public TimelineAsset openBox;

    //[Header("=== Assets Settings ===")]
    //public ActorManager attacker;
    //public ActorManager victim;

    
    // Start is called before the first frame update
    void Start()
    {
        am = GetComponent<ActorManager>();
        pd = GetComponent<PlayableDirector>();

        pd.playOnAwake = false;
    
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(am.ac.pi.keyAction) && gameObject.layer == LayerMask.NameToLayer("Player"))
        //{
        //    pd.Play();
        //}
    }

    public void PlayTimeline(string eventName, ActorManager attacker, ActorManager victim)
    {
        if(pd.state == PlayState.Playing)
        {
            return;
        }

        if(eventName == "frontStab")
        {
            pd.playableAsset = Instantiate(frontStab);
            
            TimelineAsset timeline = (TimelineAsset)pd.playableAsset;

            

            foreach (var track in timeline.GetOutputTracks())
            {
                if (track.name == "Attacker Animation")
                {
                    pd.SetGenericBinding(track, attacker.ac.anim);
                }
                else if (track.name == "Victim Animation")
                {
                    pd.SetGenericBinding(track, victim.ac.anim);
                }               
                
                else if (track.name == "Victim Script")
                {
                    pd.SetGenericBinding(track, victim);
                    foreach (var clip in track.GetClips())
                    {
                        MySurperPlayableClip myclip = (MySurperPlayableClip)clip.asset;
                        MySurperPlayableBehaviour myBehav = myclip.template;
                        myBehav.MyFloat = 666;
                        myclip.am.exposedName = System.Guid.NewGuid().ToString();
                        pd.SetReferenceValue(myclip.am.exposedName, victim);
     
                    }
                }
                else if (track.name == "Attacker Script")
                {
                    pd.SetGenericBinding(track, attacker);
                    foreach (var clip in track.GetClips())
                    {
                        MySurperPlayableClip myclip = (MySurperPlayableClip)clip.asset;
                        MySurperPlayableBehaviour myBehav = myclip.template;
                        myBehav.MyFloat = 666;
                        myclip.am.exposedName = System.Guid.NewGuid().ToString();
                        pd.SetReferenceValue(myclip.am.exposedName, attacker);
                        
                    }
                }
            }
            pd.Evaluate();
            //foreach (var track in pd.playableAsset.outputs)
            //{
            //    if (track.streamName == "Attacker Animation")
            //    {
            //        pd.SetGenericBinding(track.sourceObject, attacker.ac.anim);
            //    }
            //    else if (track.streamName == "Victim Animation")
            //    {
            //        pd.SetGenericBinding(track.sourceObject, victim.ac.anim);
            //    }
            //    else if (track.streamName == "Victim Script")
            //    {
            //        pd.SetGenericBinding(track.sourceObject, victim);
            //    }
            //    else if (track.streamName == "Attacker Script")
            //    {
            //        pd.SetGenericBinding(track.sourceObject, attacker);
            //    }
            //}
        }
        else if(eventName == "openBox")
        {
            
            pd.playableAsset = Instantiate(openBox);

            TimelineAsset timeline = (TimelineAsset)pd.playableAsset;

            foreach (var track in timeline.GetOutputTracks())
            {
                if (track.name == "Player Animation")
                {
                    pd.SetGenericBinding(track, attacker.ac.anim);
                }
                else if (track.name == "Box Animation")
                {
                    pd.SetGenericBinding(track, victim.ac.anim);
                }

                else if (track.name == "Box Script")
                {
                    pd.SetGenericBinding(track, victim);
                    foreach (var clip in track.GetClips())
                    {
                        MySurperPlayableClip myclip = (MySurperPlayableClip)clip.asset;
                        MySurperPlayableBehaviour myBehav = myclip.template;
                        myclip.am.exposedName = System.Guid.NewGuid().ToString();
                        pd.SetReferenceValue(myclip.am.exposedName, victim);

                    }
                }
                else if (track.name == "Player Script")
                {
                    pd.SetGenericBinding(track, attacker);
                    foreach (var clip in track.GetClips())
                    {
                        MySurperPlayableClip myclip = (MySurperPlayableClip)clip.asset;
                        MySurperPlayableBehaviour myBehav = myclip.template;
                        myclip.am.exposedName = System.Guid.NewGuid().ToString();
                        pd.SetReferenceValue(myclip.am.exposedName, attacker);

                    }
                }
            }
            pd.Evaluate();
        }
        pd.Play();
    }
}
