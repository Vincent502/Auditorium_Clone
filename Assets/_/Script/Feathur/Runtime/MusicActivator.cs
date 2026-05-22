using UnityEngine;


[RequireComponent (typeof(Collider2D))]
[RequireComponent(typeof(AudioSource))]
public class MusicActivator : MonoBehaviour
{

    #region Public Variable

    public SpriteRenderer musicSprite;

    #endregion


    #region Unity API

    private void Start()
    {
        _musicClip = GetComponent<AudioSource>();
        _musicClip.Play();
    }

    private void FixedUpdate()
    {
        if (_musicClip.volume > 0f && _musicEnabled == false)
        {
            DecrementVolumeSound();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _musicEnabled = true;
        ActivateMusic();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        _musicEnabled = false;
    }


    #endregion


    #region Main API
    private void ActivateMusic()
    {
        if (_musicClip.volume < 1f)
        {
            _musicClip.volume += 0.01f;
            var color = musicSprite.color;
            color.a += 0.01f;
            musicSprite.color = color;
        }
    }

    private void DecrementVolumeSound()
    {
        _musicClip.volume -= 0.01f;
        var color = musicSprite.color;
        color.a -= 0.01f;
        musicSprite.color = color;
    }

    #endregion


    #region Private & Protected

    private AudioSource _musicClip;
    private bool _musicEnabled;

    #endregion

}