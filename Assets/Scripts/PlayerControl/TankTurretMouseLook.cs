using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBC {
    public class TankTurretMouseLook : MonoBehaviour {

        const float MAX_CANNON_ANGLE = 8.5f;

        // Options
        [SerializeField] float mouseXSensitivity = 200f;
        [SerializeField] float mouseYSensitivity = 100f;
        [Space]
        [SerializeField] bool InvertMouseX = false;
        [SerializeField] bool InvertMouseY = false;
        [Space]
        [SerializeField] Transform turretTransform;
        [SerializeField] Transform cannonTransform;
        [SerializeField] Transform cam;

        // Toggle mouse control
        private bool mouseOn = true;

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
            if ( Input.GetKeyDown( KeyCode.Backspace ) ) mouseOn = !mouseOn;
            if ( !mouseOn ) return;

            // Get input.
            GetMouseDelta();

            // Rotate the turret with the mouse X delta.
            //turretTransform.RotateAround( turretTransform.position , Vector3.up , mXd );
            turretTransform.Rotate( new Vector3( 0 , mXd , 0 ) , Space.Self );
           
            // Rotate the cannon and camera with the mouse Y delta.
            cannon_angle = Mathf.Clamp( cannon_angle + mYd , -MAX_CANNON_ANGLE , MAX_CANNON_ANGLE );
            UpdateCannonAngle();
        }

        // Get mouse delta values for this frame.
        void GetMouseDelta() {
            mXd = Time.deltaTime * mouseXSensitivity * Input.GetAxis( "Mouse X" );
            if ( InvertMouseX ) mXd *= -1;
            mYd = Time.deltaTime * mouseYSensitivity * Input.GetAxis( "Mouse Y" );
            if ( !InvertMouseY ) mYd *= -1;
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