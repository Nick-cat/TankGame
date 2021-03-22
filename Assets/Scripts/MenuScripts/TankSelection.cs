using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SBC
{
    public class TankSelection : MonoBehaviour
    {

        private GameObject[] tankList;
        private int tankIndex;
        [SerializeField] LevelSelectManager levelSelect;
        [SerializeField] Animator elevator;
        [SerializeField] Animator fadeToBlack;

        void Start()
        {
            tankList = new GameObject[transform.childCount];

            //fill tank array
            for (int i = 0; i < transform.childCount; i++)
                tankList[i] = transform.GetChild(i).gameObject;

            //toggle off tank renderers
            foreach (GameObject go in tankList) go.SetActive(false);

            //toggle on first 
            if (tankList[0])
                tankList[0].SetActive(true);
        }

        public void ToggleLeft()
        {
            //toggle off current model
            tankList[tankIndex].SetActive(false);

            tankIndex--;
            if (tankIndex < 0)
                tankIndex = tankList.Length - 1;

            //toggle on new model
            tankList[tankIndex].SetActive(true);
        }

        public void ToggleRight()
        {
            //toggle off current model
            tankList[tankIndex].SetActive(false);

            tankIndex++;
            if (tankIndex == tankList.Length)
                tankIndex = 0;

            //toggle on new model
            tankList[tankIndex].SetActive(true);
        }

        public void Confirm()
        {
            PlayerPrefs.SetInt("Tank", tankIndex);
            if (PlayerPrefs.HasKey("Tank"))
                Debug.Log(PlayerPrefs.GetInt("Tank"));
            elevator.SetTrigger("Lift");
            fadeToBlack.SetTrigger("FadeToBlack");
        }

        public void LoadLevel()
        {
            SceneManager.LoadScene(levelSelect.level);
        }
    }
}
