using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Resonant.Runtime
{
    [RequireComponent(typeof(AudioSource))]
    public class ResonantRandomizer : MonoBehaviour
    {
        public string ID;
        [SerializeField] List<AudioClip> clips;
        
        [Header("Frequency")]
        [SerializeField] bool loopOnStart;
        [SerializeField] bool allowRepeats;
        
        [Header("Modulation")]
        [SerializeField] Vector2 delayModulation = new(3, 6);
        [SerializeField] Vector2 volumeModulation;
        [SerializeField] Vector2 pitchModulation;

        AudioSource source;
        
        bool hasPlayed;
        AudioClip lastPlayedClip;
        bool looping;

        float defaultVolume;
        [HideInInspector] public float volumeScale;
        
        float defaultPitch;
        [HideInInspector] public float pitchScale;

        void Start()
        {
            source = GetComponent<AudioSource>();

            delayModulation = ClampVector2(delayModulation, 0, float.PositiveInfinity);
            
            defaultVolume = source.volume;
            volumeScale = 1;
            
            defaultPitch = source.pitch;
            pitchScale = 1;

            if (loopOnStart) PlayLoop();
        }

        void Reset()
        {
            source = GetComponent<AudioSource>();
            source.playOnAwake = false;
            source.volume = 0.5f;
            source.pitch = 1;
        }

        Vector2 ClampVector2(Vector2 v, float min, float max) => new(Mathf.Clamp(v.x, min, max), Mathf.Clamp(v.y, min, max));

        public void PlayLoop(float time = -1) => StartCoroutine(ProcessLoop(time));
        public void StopLoop() => looping = false;

        IEnumerator ProcessLoop(float time = -1)
        {
            if (clips.Count == 0) yield return null;

            float timeStarted = Time.time;
            looping = true;
            
            while (looping)
            {
                PlayOnce();
                yield return new WaitForSeconds(Random.Range(delayModulation.x, delayModulation.y));
                
                if (time >= 0 && Time.time - timeStarted >= time) StopLoop();
            }
        }

        public void PlayOnce()
        {
            if (clips.Count == 0) return;

            var validClips = hasPlayed && !allowRepeats && clips.Count > 1
                ? clips.Where(clip => clip != lastPlayedClip).ToList()
                : clips;

            lastPlayedClip = validClips[Random.Range(0, validClips.Count)];
            hasPlayed = true;

            source.Stop();
            source.clip = lastPlayedClip;
            source.volume = (defaultVolume + Random.Range(volumeModulation.x, volumeModulation.y)) * volumeScale;
            source.pitch = (defaultPitch + Random.Range(pitchModulation.x, pitchModulation.y)) * pitchScale;
            source.Play();
        }
    }
}