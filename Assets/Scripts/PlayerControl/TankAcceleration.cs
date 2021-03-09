using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAcceleration : MonoBehaviour
{
    [SerializeField] float accelerationTime = 0.05f;
    
    private float oldAcceleration;
    [HideInInspector] public float acceleration;

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(nameof(Acceleration));
    }

    IEnumerator Acceleration()
    {
        float gas = Input.GetAxis("Vertical");
        oldAcceleration = acceleration;
        acceleration = Mathf.Lerp(oldAcceleration, gas, accelerationTime);

        yield return new WaitForSeconds(accelerationTime);

    }
}
