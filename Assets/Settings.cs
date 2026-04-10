using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider sensitivitySlider;
    public Slider volumeSlider;

    public static float mouseSensitivity = 100f;

    void Start()
    {
        mouseSensitivity = PlayerPrefs.GetFloat("Sensitivity", 100f);
        float volume = PlayerPrefs.GetFloat("Volume", 1f);

        sensitivitySlider.value = mouseSensitivity;
        volumeSlider.value = volume;

        AudioListener.volume = volume;
    }

    public void SetSensitivity(float value)
    {
        mouseSensitivity = value;
        PlayerPrefs.SetFloat("Sensitivity", value);
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
    }
}