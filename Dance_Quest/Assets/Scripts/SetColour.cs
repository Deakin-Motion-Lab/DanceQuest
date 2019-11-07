using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace DanceQuest
{
    public class SetColour : MonoBehaviourPunCallbacks
    {
        private Color[] _Colours;
        private int index;
        public GameObject head;
        public GameObject leftHand;
        public GameObject rightHand;
        private bool _ColourUpdated;

        private void Awake()
        {
            _Colours = new Color[4];                    // Currently set to 4 players
            _Colours[0] = new Color(1f, 0.5f, 0.1f);    // Orange
            _Colours[1] = Color.red;
            _Colours[2] = Color.yellow;
            _Colours[3] = Color.white;                  // Observer  TBC: could set to black / hide mesh?

            index = 0;
        }

        // Start is called before the first frame update
        void Start()
        {
            if (photonView.IsMine)
            {
                photonView.RPC("ChangeMyColour", RpcTarget.AllBuffered, photonView.OwnerActorNr - 1);
            }
        }

        // Update is called once per frame
        void Update()
        {
            //if (photonView.IsMine)
            //{
            //    if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickUp) || OVRInput.GetDown(OVRInput.Button.PrimaryThumbstickDown))
            //    {
            //        photonView.RPC("ChangeMyColour", RpcTarget.AllBuffered, index);
            //        index++;

            //        if (index == _Colours.Length)
            //        {
            //            index = 0;
            //        }
            //    }
            //}
        }

        [PunRPC]
        private void ChangeMyColour(int choice)
        {
            head.GetComponent<Renderer>().material.color = _Colours[choice];
            leftHand.GetComponent<Renderer>().material.color = _Colours[choice];
            rightHand.GetComponent<Renderer>().material.color = _Colours[choice];
        }
    }
}
