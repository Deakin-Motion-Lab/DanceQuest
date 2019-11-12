using UnityEngine;
using Photon.Pun;

namespace VirtualNowQuest
{
    /// <summary>
    /// This class is responsible for enabling user to toggle the "dance floor" on / off
    /// </summary>
    public class FloorControl : MonoBehaviour
    {
        public MeshRenderer floor;
        private bool _Toggle;

        // Start is called before the first frame update
        private void Start()
        {
            _Toggle = true;
            floor.gameObject.SetActive(true);
            floor.gameObject.transform.SetParent(null);
            floor.gameObject.transform.position = Vector3.zero;
        }

        // Update is called once per frame
        void Update()
        {
            if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick) || OVRInput.GetDown(OVRInput.Button.SecondaryThumbstick))
            {
                _Toggle = !_Toggle;
                ToggleFloor();
            }
        }

        /// <summary>
        /// Toggles the floor game object on / off
        /// </summary>
        private void ToggleFloor()
        {
            floor.enabled = _Toggle;
        }
    }
}
