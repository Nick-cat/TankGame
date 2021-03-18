using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SBC
{
    public class QuitManager : MonoBehaviour
    {
        [SerializeField] PauseMenu pauseMenu;
        public void QuitGame()
        {
            Debug.Log("Quit");
            Application.Quit();
        }

        public void QuitToMenu()
        {
            //if (pauseMenu != null) { pauseMenu.Resume(); }
            Time.timeScale = 1f;
            SceneManager.LoadScene(0);
        }
    }
}
