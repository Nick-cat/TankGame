using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBC {
    public class TankTurretMouseLook : MonoBehaviour {

        [SerializeField] float mouseSensitivity = 1.0f;

        [SerializeField] Transform turretTransform;
        [SerializeField] Transform cannonTransform;

        private float mXd;
        private float mYd;

        // Start is called before the first frame update
        void Start () {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // Update is called once per frame
        void Update () {
            // Get input.
            GetMouseDelta();

            turretTransform.RotateAround( turretTransform.position , Vector3.up , mXd );
        }

        // Get mouse delta values for this frame.
        void GetMouseDelta() {
            mXd = Time.deltaTime * mouseSensitivity * Input.GetAxis( "Mouse X" );
            mYd = Time.deltaTime * mouseSensitivity * Input.GetAxis( "Mouse Y" );
		}
    }
}