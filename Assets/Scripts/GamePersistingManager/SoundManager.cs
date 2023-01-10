using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Variables

    public static SoundManager instance = null;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource sfxShotSource;
    [SerializeField] private RangedNum<float> pitchRange = new(0.95f, 1.05f);

    private float startVolume;

    #endregion

    #region Awake

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        startVolume = sfxSource.volume;
    }

    #endregion

    #region Play Sfx

    public void PlaySingle(AudioClip clip)
    {
        sfxSource.clip = clip;
        sfxSource.volume = startVolume;
        sfxSource.Play();
    }

    public void PlaySingleShot(AudioClip clip)
    {
        sfxShotSource.PlayOneShot(clip);
    }

    //params keyword will allow us to send the clips array as a coma seperated parameter list
    public void RandomizeSfx(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(pitchRange.Min, pitchRange.Max);

        sfxSource.pitch = randomPitch;
        sfxSource.volume = startVolume;
        sfxSource.clip = clips[randomIndex];
        sfxSource.Play();
    }

    public void RandomizeSfx(float volume, params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(pitchRange.Min, pitchRange.Max);

        sfxSource.pitch = randomPitch;
        sfxSource.volume = volume;
        sfxSource.clip = clips[randomIndex];
        sfxSource.Play();
    }

    #endregion

    #region Stop Music

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void StartMusic()
    {
        if(!musicSource.isPlaying)
            musicSource.Play();
    }

    #endregion
}
