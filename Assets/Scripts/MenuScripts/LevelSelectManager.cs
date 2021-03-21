using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectManager : MonoBehaviour
{
    public List<Levels> levels = new List<Levels>();
    public GameObject levelSelectObject;

    public int level;

    private void Start()
    {
        foreach(Levels level in levels)
        {
            SpawnLevelSelectObject(level);
        }
    }

    private void SpawnLevelSelectObject(Levels l)
    {
        GameObject levelCell = Instantiate(levelSelectObject, transform);

        Image thumbnail = levelCell.transform.Find("levelThumb").GetComponent<Image>();
        TMPro.TMP_Text name = levelCell.transform.Find("LevelName").GetComponentInChildren<TMPro.TMP_Text>();
        Button cell = levelCell.transform.GetComponent<Button>();

        thumbnail.sprite = l.levelThumb;
        name.text = l.levelName;
        cell.onClick.AddListener(delegate { level = l.levelIndex; });
    }
}
