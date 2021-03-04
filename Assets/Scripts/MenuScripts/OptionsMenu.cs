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

        [SerializeField] Slider mainVolume;
        [SerializeField] Slider musicVolume;
        [SerializeField] Slider effectsVolume;
        [Space]
        [SerializeField] TMPro.TMP_Dropdown quality;
        [Space]
        [SerializeField] Slider fov;
        [SerializeField] TMPro.TMP_Text fovValue;


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

            //set initial volume settings
            mainVolume.value = PlayerPrefs.GetFloat("mainVolume", .75f);
            musicVolume.value = PlayerPrefs.GetFloat("musicVolume", .75f);
            effectsVolume.value = PlayerPrefs.GetFloat("effectsVolume", .75f);

            //set initial quality settings
            quality.value = PlayerPrefs.GetInt("quality", 2);

            //set initial FOV settings
            fov.value = PlayerPrefs.GetFloat("fov", 60f);

        }

        public void SetResolution(int resolutionIndex)
        {
            Resolution resolution = resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }

        public void SetMainVolume(float mainVolume)
        {
            PlayerPrefs.SetFloat("mainVolume", mainVolume);
            audioMixer.SetFloat("mainVolume", Mathf.Log10(PlayerPrefs.GetFloat("mainVolume", 0.75f)) * 20);
        }
        public void SetMusicVolume(float musicVolume)
        {
            PlayerPrefs.SetFloat("musicVolume", musicVolume);
            audioMixer.SetFloat("musicVolume", Mathf.Log10(PlayerPrefs.GetFloat("musicVolume", 0.75f)) * 20);
        }
        public void SetEffectsVolume(float effectsVolume)
        {
            PlayerPrefs.SetFloat("effectsVolume", effectsVolume);
            audioMixer.SetFloat("effectsVolume", Mathf.Log10(PlayerPrefs.GetFloat("effectsVolume", 0.75f)) * 20);
        }
        public void SetQualityLevel(int qualityIndex)
        {
            PlayerPrefs.SetInt("quality", qualityIndex);
            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("quality"));
        }

        public void ToggleFullscreen(bool fullscreen)
        {
            Screen.fullScreen = fullscreen;
            Debug.Log(Screen.fullScreenMode);
        }

        public void FOV(float fov)
        {
            PlayerPrefs.SetFloat("fov", Mathf.Round(Mathf.Clamp(fov, 60f, 120f)));
            fovValue.SetText(PlayerPrefs.GetFloat("fov").ToString());
            Camera.main.fieldOfView = PlayerPrefs.GetFloat("fov");
        }
    }
}
