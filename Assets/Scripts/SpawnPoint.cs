using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject[] tankList;
    private int tankIndex;

    void Start()
    {
        //how many tanks
        tankIndex = PlayerPrefs.GetInt("Tank");

        //Spawn in saved tank
        Instantiate(tankList[tankIndex], transform.position, transform.rotation);
    }


}
