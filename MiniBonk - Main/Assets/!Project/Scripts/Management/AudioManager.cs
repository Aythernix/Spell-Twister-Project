using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Minibonk.Management
{
    public class AudioManager : MonoBehaviour
    {
        #region Variables
        
        private AudioSource _audioSource;
        public  AudioSource globalAudioSource;
        
        public enum AudioClips
        {
            // Player Sounds
            PlayerFootsteps,
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
        }
        
        private void OnDisable()
        {
            GameManager.Instance.EventHandler.PlayAudio -= PlayAudio;
            GameManager.Instance.EventHandler.PlayCustomAudio -= PlayCustomAudio;
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
            if (source == null) source = globalAudioSource;
            _audioSource = source;
            _audioSource.PlayOneShot(clip, volume);
        }
        public void PlayAudio(AudioClips clips, AudioSource source = null)
        {
            if (source == null) source = globalAudioSource;
            _audioSource = source;
            var clipIndex = Random.Range(0, Sounds[(int)clips].clips.Length);
            PlayCustomAudio(Sounds[(int)clips].clips[clipIndex], Sounds[(int)clips].volume, source);
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
