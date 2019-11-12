using System.Collections;
using UnityEngine;

namespace VirtualNowQuest
{
    /// <summary>
    /// This class is responsible for generating "trails" for each player's arm
    /// </summary>
    public class Trails : MonoBehaviour
    {
        private const float _INTERVAL = 0.01f; 
        private const int _MAXTRAILS = 20;          // NOTE: Too many trails impacts performance

        public GameObject trailPrefab;
        private float _ElapsedTime;
        private Queue _TrailsQueue;
        private Color parentColour;

        // Start is called before the first frame update
        void Start()
        {
            _ElapsedTime = 0f;
            _TrailsQueue = new Queue();
            parentColour = GetComponent<MeshRenderer>().material.color;
        }

        // Update is called once per frame
        void Update()
        {
            _ElapsedTime += Time.deltaTime;

            if (_ElapsedTime >= _INTERVAL)
            {
                // Instantiate trail game object (prefab)
                GameObject tmp = Instantiate(trailPrefab, transform);       
                tmp.transform.localScale = new Vector3(1f, 1f, 1f);
                tmp.transform.localPosition = Vector3.zero;
                tmp.transform.localRotation = Quaternion.identity;
                tmp.SetActive(true);
                tmp.GetComponent<MeshRenderer>().material.color = parentColour;
                tmp.transform.SetParent(null);

                // Place into queue and reset timer
                _TrailsQueue.Enqueue(tmp);
                _ElapsedTime = 0f;
            }

            if (_TrailsQueue.Count == _MAXTRAILS)
            {
                // Remove FIFO object from queue and destroy
                GameObject tmp = _TrailsQueue.Dequeue() as GameObject;
                Destroy(tmp);
            }
        }
    }
}