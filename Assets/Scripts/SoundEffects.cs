using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundEffects
{
    public static void FadeOut(AudioSource audio, float time)
    {
        if (audio.volume > 0 && Time.timeScale == 1.0f)
        {
            audio.volume -= Time.deltaTime / (time + 1);
        }
        if (audio.volume == 0)
        {
            audio.enabled = false;
        }
    }

    public static void FadeIn(AudioSource audio, float time)
    {
        if (audio.enabled == true && audio.isPlaying == false && Time.timeScale  == 1.0f)
        {
            audio.Play();
        }
        if (audio.volume < 1.0f)
        {
            audio.volume += Time.deltaTime / (time + 1);
        }
    }
}
