
using System;
using UnityEngine;

[Serializable]
public class SoundClipReference {
    public AudioClip _soundClip;

    public float _volume = 1f;
    public float _startClipAt = 0f;

    public void Play() {
        if (_soundClip == null) return;
        SoundManager.Instance.PlaySound(_soundClip, _volume, _startClipAt);
    }

}
