using UnityEngine;
using Photon.Pun;

namespace VirtualNowQuest
{
    /// <summary>
    /// This class managers the local player's instance over the PUN network, sending the Transform data of the local player's VR hardware to other
    /// networked players and receiving their data in turn to animate their VR Avatar on the local player's instance
    /// </summary>
    public class PUN_PlayerMgr : MonoBehaviourPun, IPunObservable
    {
        #region Public and Private Attributes
        [Tooltip("The local player instance. Use this to know if local player is represented in the scene")]
        public static GameObject LocalPlayerInstance;
        private const float _MAX_TIME = 2.0f;
        private float _ElapsedTime;
        private FloorControl _Floor;

        // VR Avatar Elements
        [Header("Player Avatar (Displayed to other networked players):")]
        public GameObject Head;
        public GameObject LeftHand;
        public GameObject RightHand;
        private Transform localVRHeadset;
        private Transform localVRControllerLeft;
        private Transform localVRControllerRight;

        // Smoothing Variables For Remote Player's Motion
        [Header("Player Avatar Motion Smoothing:")]
        [Tooltip("0: no smoothing, > 0: increased smoothing \n(note: reduces positional accuracy)")]
        [Range(0, 10)]
        public int smoothingFactor;     // Set to 5 as default (based on CUBE use-case tests)
        [Tooltip("Maximum distance (metres) for which to apply smoothing")]
        [Range(0, 3)]
        public float appliedDistance;   // Set to 2 as default (based on CUBE use-case tests)
        private Vector3 correctPlayerHeadPosition = Vector3.zero;
        private Quaternion correctPlayerHeadRotation = Quaternion.identity;
        private Vector3 correctPlayerLeftHandPosition = Vector3.zero;
        private Quaternion correctPlayerLeftHandRotation = Quaternion.identity;
        private Vector3 correctPlayerRightHandPosition = Vector3.zero;
        private Quaternion correctPlayerRightHandRotation = Quaternion.identity;

        // Oculus Elements
        [Header("Local Player's Oculus VR:")]
        public GameObject _OVRCameraRig;
        [Tooltip("Left Hand Controller")]
        public GameObject _OVRLefthand;
        [Tooltip("Right Hand Controller")]
        public GameObject _OVRRighthand;
        private SkinnedMeshRenderer _handMeshLeft;
        private SkinnedMeshRenderer _handMeshRight;
        #endregion

        private void Awake()
        {
            if (photonView.IsMine)
            {
                // Important:
                // used in RoomManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronised
                LocalPlayerInstance = gameObject;

                // Enable Oculus Camera and controllers for local player
                _OVRCameraRig.SetActive(true);
                _OVRLefthand.SetActive(true);
                _OVRRighthand.SetActive(true);

                localVRHeadset = GameObject.Find("CenterEyeAnchor").transform;                 // Get transform data from local VR Headset
                localVRControllerLeft = GameObject.Find("CustomHandLeft").transform;
                localVRControllerRight = GameObject.Find("CustomHandRight").transform;
                _handMeshLeft = GameObject.Find("hands:Lhand").GetComponent<SkinnedMeshRenderer>();
                _handMeshRight = GameObject.Find("hands:Rhand").GetComponent<SkinnedMeshRenderer>();

                // Don't display our own "player" avatar to ourselves 
                _handMeshLeft.enabled = false;
                _handMeshRight.enabled = false;
                Head.SetActive(false);
                LeftHand.SetActive(true);
                RightHand.SetActive(true);

                _Floor = GetComponent<FloorControl>();
            }

            // Critical
            // Don't Destroy on load to prevent player from being destroyed when another player joins / leaves the room
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            _ElapsedTime = 0f;
        }


        // Update each frame
        private void Update()
        {
            if (photonView.IsMine)
            {
                if (OVRInput.Get(OVRInput.Button.One))      // GetDown = Pressed this frame, Button.One = 'A'
                {
                    _ElapsedTime += Time.deltaTime;

                    if (_ElapsedTime >= _MAX_TIME)
                    {
                        // Disable Oculus Camera and controllers for local player
                        _OVRCameraRig.SetActive(false);
                        _OVRLefthand.SetActive(false);
                        _OVRRighthand.SetActive(false);
                        _Floor.ToggleFloor(false);

                        PUN_RoomMgr.LeaveRoom();
                    }
                }

                // Animate "saber" hands
                LeftHand.transform.position = localVRControllerLeft.position;
                LeftHand.transform.rotation = localVRControllerLeft.rotation;
                RightHand.transform.position = localVRControllerRight.position;
                RightHand.transform.rotation = localVRControllerRight.rotation;
            }
            else
            {
                // Smooth Remote player's motion on local machine
                SmoothPlayerMotion(ref Head, ref correctPlayerHeadPosition, ref correctPlayerHeadRotation);
                SmoothPlayerMotion(ref LeftHand, ref correctPlayerLeftHandPosition, ref correctPlayerLeftHandRotation);
                SmoothPlayerMotion(ref RightHand, ref correctPlayerRightHandPosition, ref correctPlayerRightHandRotation);
            }
        }

        /// <summary>
        /// Applies LERP interpolation to smooth the remote player's game object motion over the network. 
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="gameObjectCorrectTransformPosition"></param>
        /// <param name="gameObjectCorrectTransformRotation"></param>
        private void SmoothPlayerMotion(ref GameObject gameObject, ref Vector3 gameObjectCorrectTransformPosition, ref Quaternion gameObjectCorrectTransformRotation)
        {
            // Smoothing variables
            float distance = Vector3.Distance(gameObject.transform.position, gameObjectCorrectTransformPosition);

            if (distance < appliedDistance)
            {
                gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, gameObjectCorrectTransformPosition, Time.deltaTime * smoothingFactor);
                gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, gameObjectCorrectTransformRotation, Time.deltaTime * smoothingFactor);
            }
            else
            {
                gameObject.transform.position = gameObjectCorrectTransformPosition;
                gameObject.transform.rotation = gameObjectCorrectTransformRotation;
            }
        }

        /// <summary>
        /// Controls the exchange of data between local and remote player's VR data
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="info"></param>
        void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // Send local VR Headset position and rotation data to other networked players
                stream.SendNext(localVRHeadset.position);
                stream.SendNext(localVRHeadset.rotation);
                stream.SendNext(localVRControllerLeft.position);
                stream.SendNext(localVRControllerLeft.rotation);
                stream.SendNext(localVRControllerRight.position);
                stream.SendNext(localVRControllerRight.rotation);
            }
            else if (stream.IsReading)
            {
                // Receive other networked players' VR Headset position and rotation data
                correctPlayerHeadPosition = (Vector3)stream.ReceiveNext();
                correctPlayerHeadRotation = (Quaternion)stream.ReceiveNext();
                correctPlayerLeftHandPosition = (Vector3)stream.ReceiveNext();
                correctPlayerLeftHandRotation = (Quaternion)stream.ReceiveNext();
                correctPlayerRightHandPosition = (Vector3)stream.ReceiveNext();
                correctPlayerRightHandRotation = (Quaternion)stream.ReceiveNext();
            }
        }
    }
}
