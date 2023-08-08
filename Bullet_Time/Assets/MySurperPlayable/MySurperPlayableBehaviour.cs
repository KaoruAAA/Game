using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class MySurperPlayableBehaviour : PlayableBehaviour
{
    public ActorManager am;
    public float MyFloat;

    PlayableDirector pd;

    public override void OnPlayableCreate (Playable playable)
    {
        
    }

    public override void OnGraphStart(Playable playable)
    {
         
        //pd = (PlayableDirector)playable.GetGraph().GetResolver();
        //foreach (var track in pd.playableAsset.outputs)
        //{
        //    if (track.streamName == "Attacker Script" || track.streamName == "Victim Script")
        //    {
        //        ActorManager am = (ActorManager)pd.GetGenericBinding(track.sourceObject);
        //        am.LockUnLockAC("lock", true);
        //    }
        //}

    }

    public override void OnGraphStop(Playable playable)
    {

        //pd = (PlayableDirector)playable.GetGraph().GetResolver();
        //pd.playableAsset = null;
        //foreach (var track in pd.playableAsset.outputs)
        //{
        //    if (track.streamName == "Attacker Script" || track.streamName == "Victim Script")
        //    {
        //        ActorManager am = (ActorManager)pd.GetGenericBinding(track.sourceObject);
        //        am.LockUnLockAC("lock", false);
        //    }
        //}
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        
        
      
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        
        am.LockUnLockAC("lock", false);
            
       
    }

    public override void PrepareFrame(Playable playable, FrameData info)
    {
        am.LockUnLockAC("lock", true);


    }
}
