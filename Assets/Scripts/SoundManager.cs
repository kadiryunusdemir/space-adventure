using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;

public class SoundManager : Singleton<SoundManager>
{
     [Serializable]
     public class Sound
     {
          public Enums.Sound sound; 
          public AudioClip audioClip;
     }

     [SerializeField] private AudioSource audioSource;
     [SerializeField] private AudioSource thrusterAudioSource;
     [SerializeField] private List<Sound> sounds;

     public void PlaySound(Enums.Sound soundEnum, Vector3 position = default)
     {
          var sound = sounds.FirstOrDefault(x => x.sound == soundEnum);

          if (sound == default)
          {
               Debug.LogError($"Sound is not found [{soundEnum}]");
               return;
          }

          if (sound.audioClip == null)
          {
               Debug.LogError($"SoundClip is not found [{soundEnum}]");
               return;
          }

          audioSource.transform.position = position;
          audioSource.clip = sound.audioClip;
          audioSource.Play();
          // UniTask.WaitWhile((() => audioSource.isPlaying));
     }

     public void StopGameSound()
     {
          thrusterAudioSource.Pause();
     } 
     
     public void StartGameSound()
     {
          thrusterAudioSource.UnPause();
     } 
}
