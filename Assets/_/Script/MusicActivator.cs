using UnityEngine;


[RequireComponent (typeof(Collider2D))]
[RequireComponent(typeof(AudioSource))]
public class MusicActivator : MonoBehaviour
{
    public SpriteRenderer musicSprite;
    private void Start()
    {
        musicClip = GetComponent<AudioSource>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!musicClip.isPlaying)
        {
            musicClip.Play();
            musicClip.volume += 0.1f;
        }
        else if(musicClip.volume < 1f)
        {
            musicClip.volume += 0.1f; 
        }
        if (musicSprite.color.a < 1f)
        {
            var color = musicSprite.color;
            color.a += 0.1f;
            musicSprite.color = color;
        }
    }

    private AudioSource musicClip;
}
