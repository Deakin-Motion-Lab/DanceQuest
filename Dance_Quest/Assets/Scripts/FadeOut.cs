using UnityEngine;

namespace VirtualNowQuest
{
    /// <summary>
    /// This class is responsible for performing a "fade out" effect on the trail prefab, duration set by the user
    /// </summary>
    public class FadeOut : MonoBehaviour
    {
        // Private attribtues
        private const int _QUEST_FPS = 72;               // Oculus Quest hardware is 72 fps
        private const float _START_ALPHA_VALUE = 1f;

        [Tooltip("Fade duration in seconds")]
        public float fadeDuration = 2;                  // Default value

        private Color _ObjColour;
        private float _AlphaValue;
        private Material _ObjMaterial;
        private float _FadeRate;                        // Fade effect rate of change 

        // Start is called before the first frame update
        private void Start()
        {
            _ObjColour = GetComponent<MeshRenderer>().material.color;
            _ObjMaterial = GetComponent<MeshRenderer>().material;
            _AlphaValue = _START_ALPHA_VALUE;
            _FadeRate = CalculateFadeRate();
        }

        // Update is called once per frame
        private void Update()
        {
            if (_AlphaValue <= 0)
            {
                return;
            }

            // Reduce alpha value and re-set colour (perform fade effect)
            _AlphaValue -= _FadeRate;
            _ObjColour.a = _AlphaValue;
            _ObjMaterial.color = _ObjColour;
        }

        /// <summary>
        /// Calculates and returns the rate of fade (per frame) for the alpha channel
        /// </summary>
        /// <returns></returns>
        private float CalculateFadeRate()
        {
            return 1f / (_QUEST_FPS * fadeDuration);
        }
    }
}