using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Minibonk.Management
{
    public class AudioManager : MonoBehaviour
    {
        #region Variables
        
        public  AudioSource globalAudioSource;
        
        public enum AudioClips
        {
            // Player Sounds
            PlayerFootsteps,
            PlayerSprint,
            PlayerJump,
            // Weapon Sounds
            Fireball,
            Wave,
            Wind,
            Ice,
            IceImpact,
            // Enemy Sounds
            MeleeFootSteps,
            MeleeAttacks,
            RangedFootSteps,
            RangedAttacks,
            RangedImpact,
            // UI Sounds
            UIHover,
            UIConfirm,
        }

        // Sound Clips
        [SerializeField] private SoundClips[] Sounds;
        
        #endregion

        private void OnEnable()
        {
            GameManager.Instance.EventHandler.PlayAudio += PlayAudio;
            GameManager.Instance.EventHandler.PlayCustomAudio += PlayCustomAudio;
            GameManager.Instance.EventHandler.StartLoopingAudio += StartLoopingAudio;
            GameManager.Instance.EventHandler.StopLoopingAudio += StopLoopingAudio;
        }
        
        private void OnDisable()
        {
            GameManager.Instance.EventHandler.PlayAudio -= PlayAudio;
            GameManager.Instance.EventHandler.PlayCustomAudio -= PlayCustomAudio;
            GameManager.Instance.EventHandler.StartLoopingAudio -= StartLoopingAudio;
            GameManager.Instance.EventHandler.StopLoopingAudio -= StopLoopingAudio;
        }
        private void OnValidate()
        {
            string[] names = Enum.GetNames(typeof(AudioClips));
            Array.Resize(ref Sounds, names.Length);
            for (int i = 0; i < names.Length; i++)
            {
                Sounds[i].name = names[i];
            }
        }
        private void Start()
        {
            globalAudioSource = GetComponent<AudioSource>();
        }

        public void PlayCustomAudio(AudioClip clip, float volume, AudioSource source = null)
        {
            AudioSource activeSource = source != null ? source : globalAudioSource;
    
            if (activeSource != null && clip != null)
            {
                activeSource.PlayOneShot(clip, volume);
            }
        }
        public void PlayAudio(AudioClips clips, AudioSource source = null)
        {
            AudioSource activeSource = source != null ? source : globalAudioSource;
            
            // Check if the sound clips array exists and has elements
            if (Sounds == null || (int)clips >= Sounds.Length || Sounds[(int)clips].clips == null || Sounds[(int)clips].clips.Length == 0)
            {
                Debug.LogWarning($"Audio clip array for {clips} is empty or not configured!");
                return;
            }
            
            var clipIndex = Random.Range(0, Sounds[(int)clips].clips.Length);
            Debug.Log($"Clip{Sounds[(int)clips].clips[clipIndex]} at index {clipIndex} played on {Sounds[(int)clips].name}");
            PlayCustomAudio(Sounds[(int)clips].clips[clipIndex], Sounds[(int)clips].volume, source);
        }

        public void StartLoopingAudio(AudioClip sound, AudioSource source)
        {
            AudioSource activeSource = source != null ? source : globalAudioSource;
            
            activeSource.clip = sound;
            activeSource.loop = true;
            activeSource.Play();
            Debug.Log("Assigned Looping Audio");
        }
        
        public void StopLoopingAudio(AudioSource source)
        {
            AudioSource activeSource = source != null ? source : globalAudioSource;
            activeSource.Stop();
        }
    }
    
    [System.Serializable]
    public struct SoundClips
    {
        [HideInInspector] public string name;
        public AudioClip[] clips;
        public float volume;
    }
}
