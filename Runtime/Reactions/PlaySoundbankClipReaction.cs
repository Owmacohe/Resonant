using System;
using System.Collections;

namespace Resonant.Runtime
{
    /// <summary>
    /// If the source is a ResonantSoundbank: Plays a single specified AudioClip
    /// </summary>
    [Serializable]
    public class PlaySoundbankClipReaction : ResonantReaction
    {
        /// <summary>
        /// the index in the soundbank for the clip to be played
        /// </summary>
        public int ClipIndex;
        
        public override IEnumerator OnReact(ResonantSource source)
        {
            var soundbank = source.GetComponent<ResonantSoundbank>();
            soundbank.PlayOnce(ClipIndex);
            
            yield return null;
        }
    }
}