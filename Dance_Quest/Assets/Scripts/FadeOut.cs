using UnityEngine;

namespace VirtualNowQuest
{
    /// <summary>
    /// This class is responsible for performing a "fade out" effect on the trail prefab, duration (in seconds) can be set by the user.
    /// </summary>
    public class FadeOut : MonoBehaviour
    {
        // Public Attributes
        [Tooltip("Fade duration in seconds")]
        public float fadeDuration = 2;                  // Default value

        // Private attribtues
        private const int _QUEST_FPS = 72;              // Oculus Quest hardware is 72 fps
        private const float _START_ALPHA_VALUE = 1f;    // 1 = Opaque, 0 = Transparent
        private Color _ObjColour;
        private Material _ObjMaterial;
        private float _AlphaValue;
        private float _FadeRate;                        // Fade effect rate of change 

        // Start is called before the first frame update
        private void Start()
        {
            _ObjMaterial = GetComponent<MeshRenderer>().material;
            _ObjColour = _ObjMaterial.color;
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

            // Reduce alpha value and update material colour (perform fade effect)
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