using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankHUD : MonoBehaviour
{

    [SerializeField] TMPro.TMP_Text speedometer;

    private float tankVelocity;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        tankVelocity = rb.velocity.magnitude;
        speedometer.SetText(Mathf.Round(tankVelocity).ToString() + " km/h");

    }
}
