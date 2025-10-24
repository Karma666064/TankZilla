using UnityEngine;

public class Settings : MonoBehaviour
{
    //[SerializeField] 

    void OnMasterVolumeChanged(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("MasterVolume", value);
    }

    void OnMusicVolumeChanged(float value)
    {
        // À implémenter avec votre système audio
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    void OnSFXVolumeChanged(float value)
    {
        // À implémenter avec votre système audio
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    void OnMuteToggled(bool isMuted)
    {
        AudioListener.pause = isMuted;
        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);
    }
}
