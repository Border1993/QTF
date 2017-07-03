using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public AudioClip buttonPress;

    private AudioSource source;

    public static Audio instance
    {
        get;
        private set;
    }

    public void PlayButtonSound()
    {
        Debug.Log("Click");
        source.PlayOneShot(buttonPress, 1.0f);
    }

    public void PlaySoundOnce(AudioClip clip)
    {
        source.PlayOneShot(clip, 1.0f);
    }

	void Start ()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        source = instance.GetComponent<AudioSource>();
	}

}
