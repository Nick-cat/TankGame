using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBC {
    public class TankTurretMouseLook : MonoBehaviour {

        //const float MAX_CANNON_ANGLE = 8.5f;

        // Options
        [SerializeField] float mouseXSensitivity = 200f;
        [SerializeField] float mouseYSensitivity = 100f;
        [Space]
        [SerializeField] bool InvertMouseX = false;
        [SerializeField] bool InvertMouseY = false;
        [Space]
        [SerializeField] float MAX_CANNON_ANGLE = 8.5f;
        [Space]
        [SerializeField] TankRound ammo;
        [Space]
        [SerializeField] Transform turretTransform;
        [SerializeField] Transform cannonTransform;
        [SerializeField] Transform cannonTipTransform;
        [SerializeField] Transform camParent;
        [SerializeField] Camera cam;
        [SerializeField] RectTransform crosshair;
        [SerializeField] FireSoundHandler fireSoundHandler;

        // Toggle mouse control
        private bool mouseOn = true;

        private float mXd;
        private float mYd;
        private bool fire;

        private float cannon_angle = 0f;

        private Quaternion orig_CameraQuaternion;

        private Rigidbody rb;

        // Start is called before the first frame update
        void Start () {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            orig_CameraQuaternion = camParent.localRotation;
            rb = GetComponent<Rigidbody>();
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

            // Player has fired this frame. Instantiate ammo and call shoot.
            if (fire) {
                TankRound round = Instantiate(ammo);
                round.Shoot( cannonTipTransform.position , cannonTipTransform.forward , ammo.projectileSpeed ) ;
                //rb.AddExplosionForce( 100f , cannonTipTransform.localPosition, 50f );
                rb.AddForceAtPosition( -cannonTipTransform.forward * ammo.projectileForce, cannonTipTransform.position );
                if ( fireSoundHandler != null ) fireSoundHandler.Fire();
			}

            RaycastHit hit;
            if (Physics.Raycast(cannonTipTransform.position, cannonTipTransform.forward, out hit, 500f, ~(1 << 11))) {
                crosshair.position = cam.WorldToScreenPoint( hit.point );
                crosshair.localScale = Vector3.one * Mathf.Lerp( 0.3f , 1f , 200f / hit.distance );
			}
        }

        // Get mouse delta values for this frame.
        void GetMouseDelta() {
            // Mouse movement.
            mXd = Time.deltaTime * mouseXSensitivity * Input.GetAxis( "Mouse X" );
            if ( InvertMouseX ) mXd *= -1;
            mYd = Time.deltaTime * mouseYSensitivity * Input.GetAxis( "Mouse Y" );
            if ( !InvertMouseY ) mYd *= -1;

            // Mouse click.
            fire = Input.GetButtonDown( "Fire1" );
		}

        // Take arbitrary float angle value and apply it to quaternion rotation, apply it to cannon and camera.
        void UpdateCannonAngle() {
            // Cannon angles.
            cannonTransform.localRotation = Quaternion.identity;
            cannonTransform.Rotate( new Vector3( cannon_angle , 0 , 0 ) , Space.Self );
            // Camera angles.
            camParent.localRotation = orig_CameraQuaternion;
            camParent.RotateAround( turretTransform.position , turretTransform.right , cannon_angle );
		}
    }
}