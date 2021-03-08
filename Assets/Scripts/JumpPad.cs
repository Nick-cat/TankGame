using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBC
{
    public class JumpPad : MonoBehaviour
    {
        [SerializeField] float padForce;
        [SerializeField] Vector3 launchDirection;

        void OnTriggerEnter(Collider other)
        {
            other.gameObject.transform.root.GetComponent<Rigidbody>()
                .AddForce(transform.InverseTransformDirection(launchDirection) * padForce * 100f);
        }
        private void FixedUpdate()
        {
            //displays launch direction
            Debug.DrawRay(transform.position, transform.InverseTransformDirection(launchDirection) * 2f, Color.green);
        }
    }
}
