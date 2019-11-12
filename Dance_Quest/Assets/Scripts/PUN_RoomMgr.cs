using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

namespace VirtualNowQuest
{
    public class PUN_RoomMgr : MonoBehaviourPunCallbacks
    {
        [Tooltip("The prefab to use for representing the player")]
        public GameObject playerPrefab;
        [Tooltip("The player's spawn location in the scene")]
        public Transform spawnPoint;

        #region Photon Callbacks
        /// <summary>
        /// Photon callback advises that local player has left room so reload launcher scene
        /// </summary>
        public override void OnLeftRoom()
        {
            // Load 'first' scene (Launcher.unity)
            SceneManager.LoadScene(0);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);

                LoadArena();
            }
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);

                LoadArena();
            }
        }

        #endregion

        #region Public and Private Methods
        private void Start()
        {
            if (playerPrefab == null)
            {
                Debug.LogError("Missing playerPrefab reference...please set it up in GameObject 'Room Manager", this);
            }
            else
            {
                if (PUN_PlayerMgr.LocalPlayerInstance == null)
                {
                    Debug.LogFormat("Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);

                    // Spawn a character for the local player
                    // This gets synced by using PhotonNetwork.Instantiate
                    PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(spawnPoint.position.x, spawnPoint.position.y, spawnPoint.position.z), Quaternion.identity, 0);
                }
                else
                {
                    Debug.LogFormat("ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }
            }
        }

        /// <summary>
        /// Make local player leave the room on Photon server
        /// </summary>
        public static void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        /// <summary>
        /// Loads the VR room Scene after a connection is made to the Photon Server (via the Launcher)
        /// </summary>
        private void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork: Trying to load a level but we are not the master client");
            }

            Debug.LogFormat("PhotonNetwork: Loading Level {0}", PhotonNetwork.CurrentRoom);
            PhotonNetwork.LoadLevel(PUN_Launcher.ROOM_NAME);
        }
        #endregion
    }
}
