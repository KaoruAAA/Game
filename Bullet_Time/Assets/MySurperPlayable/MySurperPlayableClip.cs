using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class MySurperPlayableClip : PlayableAsset, ITimelineClipAsset
{
    public MySurperPlayableBehaviour template = new MySurperPlayableBehaviour ();
    public ExposedReference<ActorManager> am;

    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<MySurperPlayableBehaviour>.Create (graph, template);
        MySurperPlayableBehaviour clone = playable.GetBehaviour ();
        //am.exposedName = GetInstanceID().ToString();
        clone.am = am.Resolve (graph.GetResolver ());
        return playable;
    }
}
