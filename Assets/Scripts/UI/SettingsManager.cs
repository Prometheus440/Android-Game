using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SettingsManager : MonoBehaviour
{

    public Toggle vibrationToggle;
    private const string VibrateKey = "VibrationEnabled";

    [SerializeField] private Slider volumeSlider;
    // Start is called before the first frame update
    void Start()
    {
        bool isEnabled = PlayerPrefs.GetInt(VibrateKey,1) == 1;
        vibrationToggle.isOn = isEnabled;

        vibrationToggle.onValueChanged.AddListener(OnVibrationToggleChanged);

		AudioListener.volume = PlayerPrefs.GetFloat("AudioLevel",1);
		volumeSlider.value = PlayerPrefs.GetFloat("AudioLevel",1);
    }

    void OnVibrationToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt(VibrateKey, isOn ? 1:0);
        PlayerPrefs.Save();

        if (isOn)
        {
            TriggerVibration();
        }
    }

    public void TriggerVibration()
    {
        if (PlayerPrefs.GetInt(VibrateKey,1) == 1)
        {
            Handheld.Vibrate();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void AudioValueChange()
	{
		PlayerPrefs.SetFloat("AudioLevel",volumeSlider.value);
		AudioListener.volume = volumeSlider.value;
	}
}
