using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Managers.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : Singleton<AudioManager>
    {
        public List<AudioStage> Audio;
        private AudioSource audioSource;
        public Stage Stage = Stage.One;
        private float minVolume = 0.0f;
        private float maxVolume = 1.0f;
        private Coroutine Coroutine;

        void Start()
        {
            StartMusic();
        }

        private AudioSource GetAudioSource()
        {
            if (audioSource is null)
                audioSource = GetComponent<AudioSource>();
            return audioSource;
        }

        private void StartMusic()
        {
            Coroutine = StartCoroutine(nameof(PlayAudio));
        }

        public void ResetMusic()
        {
            StopCoroutine(Coroutine);
            GetAudioSource().Stop();
            StartMusic();
        }

        private IEnumerator PlayAudio()
        {
            while(true)
            {
                AudioClip clip = GetAudioClip(Stage);
                if (clip == null)
                    break;
                GetAudioSource().clip = clip;
                GetAudioSource().Play();
                yield return new WaitForSeconds(clip.length);
            }
            yield return null;
        }

        private AudioClip GetAudioClip(Stage stage)
        {
            for (int i = 0; i < Audio.Count; i++)
                if (Audio[i].Stage == stage)
                    return Audio[i].Clip;
            return null;
        }

        private IEnumerator FadeIn(AudioSource audioSource, float duration, float targetVolume)
        {
            float timer = 0f;
            float currentVolume = audioSource.volume;
            float targetValue = Mathf.Clamp(targetVolume, minVolume, maxVolume);
            
            while (timer < duration)
            {
                timer += Time.deltaTime;
                var newVolume = Mathf.Lerp(currentVolume, targetValue, timer / duration);
                audioSource.volume = newVolume;
                yield return null; 
            }
        }
        private IEnumerator FadeOut(AudioSource audioSource, float duration, float targetVolume)
        {
            float timer = 0f;
            float currentVolume = audioSource.volume;
            float targetValue = Mathf.Clamp(targetVolume, minVolume, maxVolume);

            while (audioSource.volume > 0.0f)
            {
                timer += Time.deltaTime;
                var newVolume = Mathf.Lerp(currentVolume, targetValue, timer / duration);
                audioSource.volume = newVolume;
                yield return null;
            }
        }
    }



    [Serializable]
    public class AudioStage
    {
        public Stage Stage;
        public AudioClip Clip;
    }

    public enum Stage
    {
        Menu,
        One,
        Two,
        Three,
        GameOver
    }
}
