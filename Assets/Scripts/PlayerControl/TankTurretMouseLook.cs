using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBC {
    public class TankTurretMouseLook : MonoBehaviour {

        const float MAX_CANNON_ANGLE = 8.5f;

        [SerializeField] float mouseXSensitivity = 200f;
        [SerializeField] float mouseYSensitivity = 100f;

        [SerializeField] Transform turretTransform;
        [SerializeField] Transform cannonTransform;
        [SerializeField] Transform cam;

        private float mXd;
        private float mYd;

        private float cannon_angle = 0f;

        private Quaternion orig_CameraQuaternion;

        // Start is called before the first frame update
        void Start () {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            orig_CameraQuaternion = cam.localRotation;
        }

        // Update is called once per frame
        void Update () {
            // Get input.
            GetMouseDelta();

            // Rotate the turret with the mouse X delta.
            turretTransform.RotateAround( turretTransform.position , Vector3.up , mXd );

            // Rotate the cannon and camera with the mouse Y delta.
            cannon_angle = Mathf.Clamp( cannon_angle + mYd , -MAX_CANNON_ANGLE , MAX_CANNON_ANGLE );
            UpdateCannonAngle();
        }

        // Get mouse delta values for this frame.
        void GetMouseDelta() {
            mXd = Time.deltaTime * mouseXSensitivity * Input.GetAxis( "Mouse X" );
            mYd = Time.deltaTime * mouseYSensitivity * Input.GetAxis( "Mouse Y" );
		}

        // Take arbitrary float angle value and apply it to quaternion rotation, apply it to cannon and camera.
        void UpdateCannonAngle() {
            // Cannon angles.
            cannonTransform.localRotation = Quaternion.identity;
            cannonTransform.Rotate( new Vector3( cannon_angle , 0 , 0 ) , Space.Self );
            // Camera angles.
            cam.localRotation = orig_CameraQuaternion;
            cam.RotateAround( turretTransform.position , turretTransform.right , cannon_angle );
		}
    }
}