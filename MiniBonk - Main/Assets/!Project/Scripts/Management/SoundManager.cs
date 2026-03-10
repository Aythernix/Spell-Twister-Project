using System;
using UnityEngine;

namespace Minibonk.Management
{
    public class SoundManager : MonoBehaviour
    {
        private AudioSource _audioSource;

        // AudioClips
        
        // UI Clips
        [SerializeField] private AudioClip _UiSelect;
        [SerializeField] private AudioClip _UiConfirm;
        [SerializeField] private AudioClip _UiCancel;

        public enum AudioClips
        {
            UISelect,
            UIConfirm,
            UICancel
        }
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        #region Public API

        public void PlayCustomAudio(AudioClip clip)
        {
            _audioSource.PlayOneShot(clip);
        }

        public void PlayAudio(AudioClips clip)
        {
            switch (clip)
            {
                case AudioClips.UISelect:
                    PlayCustomAudio(_UiSelect);
                    break;
                case  AudioClips.UIConfirm:
                    PlayCustomAudio(_UiConfirm);
                    break;
                case  AudioClips.UICancel:
                    PlayCustomAudio(_UiCancel);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(clip), clip, null);
            }
        }

        #endregion
    }
}
