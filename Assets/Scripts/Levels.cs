using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Level", menuName = "Level")]
public class Levels : ScriptableObject
{
    public string LevelName { get { return levelName; } }
    public Sprite LevelThumb { get { return levelThumb; } }
    public int LevelIndex { get { return levelIndex; } }
    [SerializeField] string levelName;
    [SerializeField] Sprite levelThumb;
    [SerializeField] int levelIndex;
}
