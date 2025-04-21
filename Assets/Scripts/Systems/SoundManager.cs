using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : PersistentSignleton<SoundManager> {
    [SerializeField] private AudioSource musicSource;
    private int effectSourceArray = 0;
    
    [SerializeField] private AudioMixerGroup _musicMixer;
    [SerializeField] private AudioMixerGroup _effectMixer;


    [SerializeField] private int maxEffectSources = 10;

    public void PlayMusicTrack(AudioClip clip) {
        musicSource.Stop();

        musicSource.outputAudioMixerGroup = _musicMixer;
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlaySound(AudioClip clip, float volume = 1, float startClipAt = 0) {
        // check if max amount of sources has been reached
        if (effectSourceArray < maxEffectSources) {
            // create new source
            AudioSource newSource = gameObject.AddComponent<AudioSource>();

            effectSourceArray += 1;
            
            newSource.clip = clip;
            newSource.time = startClipAt;
            newSource.outputAudioMixerGroup = _effectMixer;
            newSource.loop = false;
            newSource.volume = volume;
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
