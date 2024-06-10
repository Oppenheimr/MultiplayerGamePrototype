using DG.Tweening;
using UnityEngine;

namespace UnityUtils.Helpers
{
    public class AudioSourceHelper
    {
        public static AudioSource PlayOnceAndDestroy(AudioClip clip, float volume = 1, float delay = 0.1f)
        {
            var audioObject = new GameObject("Single Audio " + clip.name);
            var audioSource = audioObject.AddComponent<AudioSource>();

            audioSource.clip = clip;
            audioSource.Play();
            audioSource.volume = 0;
            audioSource.DOFade(1, delay);
            Object.Destroy(audioObject, audioSource.clip.length);
            return audioSource;
        }
        
        public static AudioSource PlayOnceAndDeActive(AudioSource audioSource, AudioClip clip, float volume = 1, float delay = 0.1f)
        {
            audioSource.clip = clip;
            audioSource.Play();
            audioSource.volume = 0;
            audioSource.DOFade(1, delay);
            GameObjectHelper.SetActive(audioSource.gameObject, audioSource.clip.length, false);
            return audioSource;
        }
    }
}