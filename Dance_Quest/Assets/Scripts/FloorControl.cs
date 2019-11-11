using UnityEngine;
using Photon.Pun;

namespace VirtualNowQuest
{
    /// <summary>
    /// This class is responsible for enabling user to toggle the "dance floor" on / off
    /// </summary>
    public class FloorControl : MonoBehaviourPun
    {
        private MeshRenderer _Floor;
        private bool _Toggle;

        // Start is called before the first frame update
        private void Start()
        {
            _Floor = GameObject.FindGameObjectWithTag("Dance Floor").GetComponent<MeshRenderer>();
            _Toggle = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (photonView.IsMine)
            {
                if (OVRInput.GetDown(OVRInput.Button.One) || OVRInput.GetDown(OVRInput.Button.Two))
                {
                    _Toggle = !_Toggle;
                    ToggleFloor();
                }
            }
        }

        /// <summary>
        /// Toggles the floor game object on / off
        /// </summary>
        private void ToggleFloor()
        {
            _Floor.enabled = _Toggle;
        }
    }
}
