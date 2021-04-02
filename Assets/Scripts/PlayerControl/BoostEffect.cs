using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.VFX;

namespace SBC
{
    public class BoostEffect : MonoBehaviour
    {
        [SerializeField] List<Transform> exhaustPipes;
        [SerializeField] Volume postProcess;
        [SerializeField] TankTurretMouseLook turret;
        [SerializeField] float boostFOV = 20f;
        [Range(0f,1f)]
        [SerializeField] float effectStrength;
        [SerializeField] float effectSmoothTime = 1f;
        private ChromaticAberration chromaticAberration;
        private float oldChromValue;
        private float chromValue;
        private float chromChange;

        private float oldFOVMod;
        private float fovChange;

        public void Boost()
        {
            //Chromatic Abberation Effect
            if (postProcess.profile.TryGet(out chromaticAberration))
            {
                oldChromValue = chromValue;
                chromValue = Mathf.SmoothDamp(oldChromValue, effectStrength, ref chromChange, effectSmoothTime);
                chromaticAberration.intensity.value = Mathf.Clamp(chromValue, 0f, 1f);
            }

            //Exhaust Effect
            foreach (Transform p in exhaustPipes)
            {
                p.GetComponentInChildren<VisualEffect>().Play();
            }

            oldFOVMod = turret.fovMod;
            turret.fovMod = Mathf.SmoothDamp(oldFOVMod, boostFOV, ref fovChange, effectSmoothTime);
        }

        public void NoBoost()
        {
            if (postProcess.profile.TryGet(out chromaticAberration))
            {
                oldChromValue = chromValue;
                chromValue = Mathf.SmoothDamp(oldChromValue, 0, ref chromChange, effectSmoothTime);
                chromaticAberration.intensity.value = Mathf.Clamp(chromValue, 0f, 1f);
            }
            
            foreach (Transform p in exhaustPipes)
            {
                p.GetComponentInChildren<VisualEffect>().Stop();
            }

            oldFOVMod = turret.fovMod;
            turret.fovMod = Mathf.SmoothDamp(oldFOVMod, 0f, ref fovChange, effectSmoothTime);
        }
    }
}
