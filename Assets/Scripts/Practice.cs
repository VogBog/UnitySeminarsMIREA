using UnityEngine;

public class Practice : MonoBehaviour
{
    [SerializeField] private AudioClip[] _musics;
    [SerializeField] private AudioSource _music;
    
    public void ChangeMusic(int index)
    {
        _music.Stop();
        _music.clip = _musics[index];
        _music.Play();
    }

    public void PlaySound(AudioSource source)
    {
        source.Play();
    }

    public void StopMusic()
    {
        _music.Stop();
    }
}
