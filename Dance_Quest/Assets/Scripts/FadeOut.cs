using UnityEngine;

namespace VirtualNowQuest
{
    /// <summary>
    /// This class is responsible for performing a "fade out" effect on the trail prefab
    /// </summary>
    public class FadeOut : MonoBehaviour
    {
        // Private attribtues
        private Color alphaColor;
        private float _Alpha;
        private Material myMaterial;
        private const float _ALPHA_VALUE = 0.05f;    // Higher value reduces fade time [NOTE: was 0.1f]


        private void Start()
        {
            alphaColor = GetComponent<MeshRenderer>().material.color;
            myMaterial = GetComponent<MeshRenderer>().material;
            _Alpha = 1f;
        }

        private void Update()
        {
            if (_Alpha <= 0)
            {
                return;
            }

            // Reduce alpha value and re-set colour (perform fade effect)
            _Alpha -= _ALPHA_VALUE;
            alphaColor.a = _Alpha;
            myMaterial.color = alphaColor;
        }
    }
}