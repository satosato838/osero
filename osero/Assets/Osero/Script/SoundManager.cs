using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _seSource;
    [SerializeField] private List<BGMSoundData> musicSourceList;
    [SerializeField] private List<SESoundData> seSourceList;
    public static SoundManager Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBGM(BGMSoundData.BGM bgm)
    {
        if (_musicSource.isPlaying)
        {
            _musicSource.Stop();
        }
        BGMSoundData data = musicSourceList.Find(data => data.bgm == bgm);
        _musicSource.clip = data.audioClip;
        _musicSource.loop = true;
        _musicSource.volume = data.volume;
        _musicSource.Play();
    }

    public void PlaySE(SESoundData.SE se, float delay = 0.0f)
    {
        StartCoroutine(PlaySoundEffect(se, delay));
    }


    private IEnumerator PlaySoundEffect(SESoundData.SE se, float delay = 0.0f)
    {
        if (_seSource.isPlaying)
        {
            _seSource.Stop();
        }
        SESoundData data = seSourceList.Find(data => data.se == se);
        _seSource.volume = data.volume;
        yield return new WaitForSeconds(delay);
        _seSource.PlayOneShot(data.audioClip);
    }

    public void StopBGM()
    {
        if (_musicSource.isPlaying)
        {
            _musicSource.Stop();
        }
    }
}

[System.Serializable]
public class BGMSoundData
{
    public enum BGM
    {
        title,
        main,
        gameover,
    }

    public BGM bgm;
    public AudioClip audioClip;
    [Range(0, 1)]
    public float volume = 1;
}

[System.Serializable]
public class SESoundData
{
    public enum SE
    {
        gamestart,
        gameend,
        buttonpush,
        diskpush,
        skip
    }

    public SE se;
    public AudioClip audioClip;
    [Range(0, 2)]
    public float volume = 1;
}