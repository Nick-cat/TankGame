using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBC
{
    public class PauseMenu : MonoBehaviour
    {

        public static bool isPaused = false;

        [SerializeField] GameObject pauseUI;
        [SerializeField] GameObject crosshair;

        //get tank controller to disable on pause
        public GameObject tank;
        private TankController control;
        private TankTurretMouseLook look;


        void Start()
        {
            look = tank.GetComponent<TankTurretMouseLook>();
            control = tank.GetComponent<TankController>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButtonDown("Pause"))
            {
                if (isPaused) Resume();
                else Pause();
            }
        }

        public void Resume()
        {
            pauseUI.SetActive(false);
            crosshair.SetActive(true);
            Time.timeScale = 1f;
            look.enabled = true;
            control.enabled = true;
            isPaused = false;
            AudioListener.pause = false;
        }

        public void Pause()
        {
            pauseUI.SetActive(true);
            crosshair.SetActive(false);
            Time.timeScale = 0f;
            look.enabled = false;
            control.enabled = false;
            isPaused = true;
            AudioListener.pause = true;

        }
    }
}
