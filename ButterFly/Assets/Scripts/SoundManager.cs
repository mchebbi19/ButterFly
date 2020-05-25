using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{

    public enum Sound
    {
        BirdJump,
        Score,
        Lose,
        Mute,
    }
   
    // Start is called before the first frame update
    public static void PlaySound (Sound sound)
    {
        if (StaticValue.bEnableSound)
        {
            GameObject gameObject = new GameObject("Sound", typeof(AudioSource));

            AudioSource audioSource = gameObject.GetComponent<AudioSource>();

            audioSource.PlayOneShot(GetAudioClip(sound));
        }
        
    }

   

    public static AudioClip GetAudioClip (Sound sound)
    {
        foreach(GameAssets.SoundAudioClip sounAudioClip in GameAssets.GetInstance().soundAudioClipArray)
        {
            if(sounAudioClip.sound == sound)
            {
                return sounAudioClip.audioClip;
            }
        }
        Debug.LogError("Sound " + sound + "not found");
        return null;
    }

    //public static void PlaySound()
    //{
    //    GameObject gameObject = new GameObject("Sound", typeof(AudioSource));

    //    AudioSource audioSource = gameObject.GetComponent<AudioSource>();

    //    audioSource.PlayOneShot(GameAssets.GetInstance().birdJump);
    //}


}
