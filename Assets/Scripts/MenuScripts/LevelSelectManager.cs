using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    public int level;
    public void LoadLevel(int levels)
    {
        level = levels;
    }
}
