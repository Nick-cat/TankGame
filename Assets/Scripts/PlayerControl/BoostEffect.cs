using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SBC
{
    public class BoostEffect : MonoBehaviour
    {
        [SerializeField] List<Transform> exhaustPipes;
        [SerializeField] GameObject exhaustEffect;
        [SerializeField] Volume postProcess;
        [Range(0f,1f)]
        [SerializeField] float effectStrength;
        [SerializeField] float effectSmoothTime = 1f;
        private ChromaticAberration chromaticAberration;
        private float oldChromValue;
        private float chromValue;
        private float chromChange;

        public void Boost()
        {
            if (postProcess.profile.TryGet(out chromaticAberration))
            {
                oldChromValue = chromValue;
                chromValue = Mathf.SmoothDamp(oldChromValue, effectStrength, ref chromChange, effectSmoothTime);
                chromaticAberration.intensity.value = Mathf.Clamp(chromValue, 0f, 1f);
            }

            foreach (Transform p in exhaustPipes)
            {
                p.GetComponentInChildren<ParticleSystem>().Play();
            }
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
                p.GetComponentInChildren<ParticleSystem>().Stop();
            }

        }
    }
}
