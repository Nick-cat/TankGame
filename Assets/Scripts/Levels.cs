using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Level", menuName = "Level")]
public class Levels : ScriptableObject
{
    public string levelName;
    public Sprite levelThumb;
    public int levelIndex;
}
