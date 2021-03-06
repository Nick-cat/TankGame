using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SBC
{
    public class TankHUD : MonoBehaviour
    {

        [SerializeField] TMPro.TMP_Text speedometer;
        [Header("Health")]
        [SerializeField] Image healthBar;
        [SerializeField] Image slowHealthBar;
        [SerializeField] Image barBase;
        [Tooltip("health lower than this percent triggers warnings")] 
        [SerializeField] float danger = .1f;
        [SerializeField] float easeTime = 0.1f;

        private float tankVelocity;

        private Rigidbody rb;
        private HealthManager tank;

        //variables for health bar
        private float healthPercent;
        private float oldHealth;
        private float health;
        private float oldSlowHealth;
        private float slowHealth;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            tank = GetComponent<HealthManager>();
        }

        // Update is called once per frame
        void Update()
        {
            //Get and display tank speed
            TankSpeed();
            //Get and display tank health
            StartCoroutine(nameof(TankHealth));
        }

        void TankSpeed()
        {
            tankVelocity = rb.velocity.magnitude;
            speedometer.SetText(Mathf.Round(tankVelocity).ToString() + " km/h");
        }

        private IEnumerator TankHealth()
        {
            oldHealth = health;
            oldSlowHealth = slowHealth;

            //Get Tank Health percentage
            healthPercent = Mathf.Clamp(tank.currentHealth / tank.maxHealth, 0f, 1f);

            //set healthbar
            health = Mathf.Lerp(oldHealth, healthPercent, easeTime);
            healthBar.fillAmount = health;

            //set slow healthbar
            slowHealth = Mathf.Lerp(oldSlowHealth, healthPercent, easeTime / 4);
            slowHealthBar.fillAmount = slowHealth;

            //change the base colour
            if (healthPercent <= danger) barBase.color = Color.red;
            else barBase.color = Color.white;

            yield return new WaitForSeconds(easeTime);
        }
        
    }
}
