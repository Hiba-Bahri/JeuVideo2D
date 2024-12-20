using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }
    private AudioSource source;
    private AudioSource musicSource;

    private void Awake()
    {
        instance = this;
        source = GetComponent<AudioSource>();
        musicSource = transform.GetChild(0).GetComponent<AudioSource>();

        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }else if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        ChangeMusicVolume(0);
        ChangeSoundVolume(0);
    }

    public void PlaySound(AudioClip _sound)
    {
       source.PlayOneShot(_sound);
    }
    public void ChangeSoundVolume(float _change)
    {
        ChangeSourceVolume(1, "soundVolume", source, _change);
    }
    private void ChangeSourceVolume(float baseVolume, string volumeName, AudioSource _source, float _change)
    {
        float currentVolume = PlayerPrefs.GetFloat(volumeName, 1);
        currentVolume+= _change;
        if(currentVolume > 1)
            currentVolume = 0;
        else if (currentVolume < 0)
            currentVolume = 1;
        
        float finalVolume = currentVolume * baseVolume;
        _source.volume = finalVolume;

        PlayerPrefs.SetFloat(volumeName, currentVolume);
    }
    public void ChangeMusicVolume(float _change)
    {
        ChangeSourceVolume(0.3f, "musicVolume", musicSource, _change);
    }
}
