using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Minibonk.Player
{
    public class PlayerAudioController : MonoBehaviour
    {
        #region Variables
        
        private AudioSource _audioSource;
        
        public enum AudioClips
        {
            Footsteps,
            Jump,
        }

        // Sound Clips
        [SerializeField] private List<AudioClip> footsteps;
        [SerializeField] private AudioClip Jump;
        #endregion
        
        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayCustomAudio(AudioClip clip)
        {
            _audioSource.PlayOneShot(clip);
        }
        public void PlayAudio(AudioClips clips)
        {
            switch (clips)
            {
                case AudioClips.Footsteps:
                    var footStepIndex = Random.Range(0, footsteps.Count);
                    PlayCustomAudio(footsteps[footStepIndex]);
                    break;
                case  AudioClips.Jump:
                    PlayCustomAudio(Jump);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(clips), clips, null);
            }
        }
    }
}
