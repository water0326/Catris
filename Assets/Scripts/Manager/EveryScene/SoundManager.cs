using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public enum AudioType {
    BG,
    Effect
}

[System.Serializable]
public class Audio {
    public string audioName;
    public AudioClip clip;
    public AudioType audioType;
    [Range(0, 1)]
    public float volume;
}



public class SoundManager : Singleton<SoundManager>
{
    
    [SerializeField]
    Audio[] audios;

    AudioSource currentBGM;
    Audio currentAudio;

    [SerializeField]
    [ReadOnly]
    Queue<AudioSource> audioSources;
    [SerializeField]
    int init_count = 8;
    [SerializeField]
    [ReadOnly]
    List<AudioSource> activedAS;

    protected override void OnAwake() {
        audioSources = new Queue<AudioSource>();
        activedAS = new List<AudioSource>();
        currentBGM = null;
        for(int i = 0 ; i < init_count ; i++) {
            CreateAudioSource();
        }
    }

    void CreateAudioSource() {
        audioSources.Enqueue(gameObject.AddComponent<AudioSource>());
    }

    public AudioSource GetAudioSource() {
        if(audioSources.Count == 0) {
            CreateAudioSource();
        }
        return audioSources.Dequeue();
    }

    public void Play(string audioName) {
        
        Audio audio = GetAudio(audioName);
        if(audio == null) return;

        if(audio.audioType == AudioType.BG && currentBGM != null) {
            StopBGM();
        }

        AudioSource audioSource = GetAudioSource();
        
        audioSource.clip = audio.clip;
        if(audio.audioType == AudioType.BG) {
            currentBGM = audioSource;
            currentAudio = audio;
            audioSource.loop = true;
            audioSource.volume = audio.volume * StaticDataManager.Instance.data.BGMVolume * 0.01f;
        }
        else {
            audioSource.loop = false;
            audioSource.volume = audio.volume * StaticDataManager.Instance.data.EffectVolume * 0.01f;
        }
        audioSource.Play();
        activedAS.Add(audioSource);

    }

    public void StopBGM() {
        currentBGM.Stop();
        currentBGM = null;
    }

    private void Update() {
        for(int i = 0 ; i < activedAS.Count ; i++) {
            if(!activedAS[i].isPlaying) {
                audioSources.Enqueue(activedAS[i]);
                activedAS.RemoveAt(i);
                i--;
            }
        }
    }

    Audio GetAudio(string name) {
        for(int i = 0 ; i < audios.Length ; i++) {
            if(audios[i].audioName == name) {
                return audios[i];
            }
        }
        return null;
    }

    public void ChangeBGMVolume() {
        if(currentBGM == null) return;
        currentBGM.volume = currentAudio.volume * StaticDataManager.Instance.data.BGMVolume * 0.01f;
    }
}
