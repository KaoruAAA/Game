using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.990566f, 0f, 0.06779619f)]
[TrackClipType(typeof(MySurperPlayableClip))]
[TrackBindingType(typeof(ActorManager))]
public class MySurperPlayableTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<MySurperPlayableMixerBehaviour>.Create (graph, inputCount);
    }
}
