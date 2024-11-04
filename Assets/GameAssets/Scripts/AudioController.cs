using UnityEngine;

public class AudioController : GenericSingletonClass<AudioController>
{


    private AudioSource GetRootAudioSource()
    {
        //GameObject rootObject = GameObject.FindWithTag("rootController");
        return this.GetComponent<AudioSource>();   
    }

    private AudioSource GetMusicAudioSource()
    {
        Camera mainCamera = Camera.main;
        return mainCamera.GetComponent<AudioSource>();
    }

    public void OnButtonClick()
    {
        //if (GameController.Instance.isAudioMute) return;
        string soundName = "button";
        AudioClip clip = Resources.Load<AudioClip>("SoundAssets/" + soundName);
        AudioSource rootAudioSource = GetRootAudioSource();
        rootAudioSource.clip = clip;
        rootAudioSource.Play();
    }

    public void OnError()
    {
       // if (GameController.Instance.isAudioMute) return;

        string soundName = "error";
        AudioClip clip = Resources.Load<AudioClip>("SoundAssets/" + soundName);
        AudioSource rootAudioSource = GetRootAudioSource();
        rootAudioSource.clip = clip;
        rootAudioSource.Play();
    }

    public void OnSetComplete()
    {
        //if (GameController.Instance.isAudioMute) return;

        string soundName = "set";
        AudioClip clip = Resources.Load<AudioClip>("SoundAssets/" + soundName);
        AudioSource rootAudioSource = GetRootAudioSource();
        rootAudioSource.clip = clip;
        rootAudioSource.Play();
    }

    public void OnToggleBackgroundMusic()
    {
        AudioSource musicAudioSource = GetMusicAudioSource();
        musicAudioSource.mute = !musicAudioSource.mute;
        //GameController.Instance.isAudioMute = musicAudioSource.mute;
    }

    public void OnPlayBackgroundMusic()
    {
        GetMusicAudioSource().mute = false;
        //GameController.Instance.isAudioMute = false;
    }

    public void OnPauseBackgroundMusic()
    {
        AudioSource globalAudioSource = GetRootAudioSource();
        globalAudioSource.enabled = false;
        //GameController.Instance.isAudioMute = true;
        globalAudioSource.Pause();
    }

    public void OnComplete()
    {
        //if (GameController.Instance.isAudioMute) return;

        string soundName = "win";
        AudioClip clip = Resources.Load<AudioClip>("SoundAssets/" + soundName);
        AudioSource rootAudioSource = GetRootAudioSource();
        rootAudioSource.clip = clip;
        rootAudioSource.Play();
    }

    public void OnAchievement()
    {
       // if (GameController.Instance.isAudioMute) return;

        string soundName = "win";
        AudioClip clip = Resources.Load<AudioClip>("SoundAssets/" + soundName);
        AudioSource rootAudioSource = GetRootAudioSource();
        if (!rootAudioSource.isPlaying)
        {
            rootAudioSource.clip = clip;
            rootAudioSource.Play();
        }
    }


    public void MuteGlobalAudio()
    {
        //GameController.Instance.isAudioMute = true;
    }

    public void UnmuteGlobalAudio()
    {
       // GameController.Instance.isAudioMute = false;
    }
}
