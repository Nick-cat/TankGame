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
        [SerializeField] Image boostBar;
        [SerializeField] Image slowBoostBar;
        [SerializeField] Image barBase;
        [Tooltip("health lower than this percent triggers warnings")] 
        [SerializeField] float danger = .1f;
        [SerializeField] float easeTime = 0.1f;

        private float tankVelocity;

        private Rigidbody rb;
        private HealthManager tankHealth;
        private TankController tank;

        //variables for health bar
        private float healthPercent;
        private float oldHealth;
        private float health;
        private float oldSlowHealth;
        private float slowHealth;

        //variables for boost bar
        private float boostPercent;
        private float oldBoost;
        private float boost;
        private float oldSlowBoost;
        private float slowBoost;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            tankHealth = GetComponent<HealthManager>();
            tank = GetComponent<TankController>();
        }

        void Update()
        {
            //Get and display tank speed
            TankSpeed();
            //Get and display tank health
            StartCoroutine(nameof(TankHealth));
            StartCoroutine(nameof(TankBoost));
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
            healthPercent = Mathf.Clamp(tankHealth.currentHealth / tankHealth.maxHealth, 0f, 1f);

            //set healthbar
            health = Mathf.Lerp(oldHealth, healthPercent, easeTime);
            healthBar.fillAmount = health;

            //set slow healthbar
            slowHealth = Mathf.Lerp(oldSlowHealth, healthPercent, easeTime / 4f);
            slowHealthBar.fillAmount = slowHealth;

            //change the base colour
            if (healthPercent <= danger) barBase.color = Color.red;
            else barBase.color = Color.white;

            yield return new WaitForSeconds(easeTime);
        }

        private IEnumerator TankBoost()
        {
            oldBoost = boost;
            oldSlowBoost = slowBoost;

            boostPercent = Mathf.Clamp(tank.boostRemaining / tank.boostTimer, 0f, 1f);

            boost = Mathf.Lerp(oldBoost, boostPercent, easeTime);
            boostBar.fillAmount = boost;

            slowBoost = Mathf.Lerp(oldSlowBoost, boostPercent, easeTime / 4f);
            slowBoostBar.fillAmount = slowBoost;

            yield return new WaitForSeconds(easeTime);
        }
        
    }
}
