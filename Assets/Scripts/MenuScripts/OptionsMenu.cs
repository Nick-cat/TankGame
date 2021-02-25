using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace SBC
{
    public class OptionsMenu : MonoBehaviour
    {
        public AudioMixer audioMixer;
        public TMPro.TMP_Dropdown resolutionDropdown;

        Resolution[] resolutions;

        private void Start()
        {

            //Resolution dropdown setup
            resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();

            int currentResolutionIndex = 0;
            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height;
                options.Add(option);

                if (resolutions[i].width == Screen.width &&
                    resolutions[i].height == Screen.height)
                {
                    currentResolutionIndex = i;
                }
            }
            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }

        public void SetResolution(int resolutionIndex)
        {
            Resolution resolution = resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        public void SetMainVolume(float volume)
        {
            audioMixer.SetFloat("mainVolume", Mathf.Log10(volume) * 20);
        }
        public void SetMusicVolume(float volume)
        {
            audioMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20);
        }
        public void SetEffectsVolume(float volume)
        {
            audioMixer.SetFloat("effectsVolume", Mathf.Log10(volume) * 20);
        }
        public void SetQualityLevel(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
        }

        public void ToggleFullscreen(bool fullscreen)
        {
            Screen.fullScreen = fullscreen;
            Debug.Log(Screen.fullScreenMode);
        }
    }
}
