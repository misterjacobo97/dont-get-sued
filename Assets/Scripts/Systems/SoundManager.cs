using System.Linq;
using UnityEngine;

public class SoundManager : PersistentSignleton<SoundManager> {
    [SerializeField] private AudioSource musicSource;
    private int effectSourceArray = 0;

    [SerializeField] private int maxEffectSources = 10;

    public void PlayMode(AudioClip clip) {
        musicSource.Stop();

        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlaySound(AudioClip clip) {
        // check if max amount of sources has been reached
        if (effectSourceArray < maxEffectSources) {
            // create new source
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            effectSourceArray += 1;
            
            newSource.loop = false;
            newSource.clip = clip;
            newSource.Play();

            // destroy after length of time
            DestroyAudioSource(newSource, newSource.clip.length);
        }
    }

    private async void DestroyAudioSource(AudioSource source, float delay) {
        await Awaitable.WaitForSecondsAsync(delay);

        Destroy(source);
        effectSourceArray -= 1;
    }
}
