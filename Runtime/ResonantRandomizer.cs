using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Resonant.Runtime
{
    /// <summary>
    /// A more complicated ResonantSource, used to randomize the playing of AudioClips
    /// </summary>
    public class ResonantRandomizer : ResonantSource
    {
        [SerializeField, Tooltip("All of the clips that this source can play")] List<AudioClip> clips;
        
        [Header("Frequency")]
        [SerializeField, Tooltip("Whether to start looping the AudioClips when the game starts")] bool loopOnStart;
        [SerializeField, Tooltip("Whether to allow the the same AudioClip to be played twice in a row")] bool allowRepeats;
        
        [Header("Modulation")]
        [SerializeField, Tooltip("The minimum and maximum for the range of time between looping AudioClips")]
        Vector2 delayModulation = new(3, 6);
        [SerializeField, Tooltip("The minimum and maximum for the range that AudioClips' volume can be changed by each time they are played (leave at 0,0 for no modulation)")]
        Vector2 volumeModulation;
        [SerializeField, Tooltip("The minimum and maximum for the range that AudioClips' pitch can be changed by each time they are played (leave at 0,0 for no modulation)")]
        Vector2 pitchModulation;
        
        bool hasPlayed; // Whether this source has played a clip at least once
        AudioClip lastPlayedClip; // The AudioClip that was last played by this source
        bool looping; // Whether to loop the playing of AudioClips

        [HideInInspector] public float VolumeScale; // An independent volume amount used to uniformly scale the source's volume
        [HideInInspector] public float PitchScale; // An independent pitch amount used to uniformly scale the source's pitch

        Vector2 ClampVector2(Vector2 v, float min, float max) => new(Mathf.Clamp(v.x, min, max), Mathf.Clamp(v.y, min, max));
        
        new void Start()
        {
            base.Start();
            
            delayModulation = ClampVector2(delayModulation, 0, float.PositiveInfinity); // Ensuring that the delay isn't less than 0
            
            VolumeScale = 1;
            PitchScale = 1;

            if (loopOnStart) PlayLoop();
        }
        
        /// <summary>
        /// Starts the randomized AudioClip loop
        /// </summary>
        /// <param name="exitTime">The amount of time after which the loop should stop (leave at -1 to loop indefinitely)</param>
        public void PlayLoop(float exitTime = -1) => StartCoroutine(ProcessLoop(exitTime));
        
        /// <summary>
        /// Stops the randomized AudioClip loop
        /// </summary>
        public void StopLoop() => looping = false;

        /// <summary>
        /// The looping randomized AudioClip coroutine
        /// </summary>
        /// <param name="exitTime">The amount of time after which the loop should stop (leave at -1 to loop indefinitely)</param>
        IEnumerator ProcessLoop(float exitTime = -1)
        {
            if (clips.Count == 0) yield return null;

            float timeStarted = Time.time;
            looping = true;
            
            while (looping)
            {
                PlayOnce();
                
                // Waiting for a random amount of time before playing the next AudioClip
                yield return new WaitForSeconds(Random.Range(delayModulation.x, delayModulation.y));
                
                // Stopping the loop if we've reached the end of the allotted time
                if (exitTime >= 0 && Time.time - timeStarted >= exitTime) StopLoop();
            }
        }

        /// <summary>
        /// Plays a single randomized AudioClip
        /// </summary>
        public void PlayOnce()
        {
            if (clips.Count == 0) return;

            // If we don't allow repeats, make sure the last played AudioCLip isn't in the list
            var validClips = hasPlayed && !allowRepeats && clips.Count > 1
                ? clips.Where(clip => clip != lastPlayedClip).ToList()
                : clips;
            
            lastPlayedClip = validClips[Random.Range(0, validClips.Count)]; // Getting the next clip
            hasPlayed = true;

            Source.Stop();
            Source.clip = lastPlayedClip;
            
            // Randomly modulating the volume and pitch of the source
            Source.volume = (DefaultVolume + Random.Range(volumeModulation.x, volumeModulation.y)) * VolumeScale;
            Source.pitch = (DefaultPitch + Random.Range(pitchModulation.x, pitchModulation.y)) * PitchScale;
            
            Source.Play();
        }
    }
}