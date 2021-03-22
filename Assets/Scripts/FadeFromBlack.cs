using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeFromBlack : MonoBehaviour
{
    [SerializeField] Animator fadeToBlack;
    // Start is called before the first frame update
    void Awake()
    {
        fadeToBlack.SetTrigger("FadeFromBlack");
    }
}
