using UnityEngine;

namespace DanceQuest
{
    public class FadeOut : MonoBehaviour
    {
        private Color alphaColor;
        private float _Alpha;
        private float _Red;
        private Material myMaterial;
        private const float _ALPHA_VALUE = 0.1f;


        public void Start()
        {
            alphaColor = GetComponent<MeshRenderer>().material.color;
            myMaterial = GetComponent<MeshRenderer>().material;
            _Alpha = 1f;
        }

        public void Update()
        {
            if (_Alpha <= 0)
            {
                return;
            }

            _Alpha -= _ALPHA_VALUE;
            alphaColor.a = _Alpha;
            myMaterial.color = alphaColor;
        }
    }
}