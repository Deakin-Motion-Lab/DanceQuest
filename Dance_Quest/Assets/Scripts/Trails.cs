using System.Collections;
using UnityEngine;

namespace VirtualNowQuest
{
    /// <summary>
    /// This class is responsible for generating "trails" for each player's arm and head.
    /// </summary>
    public class Trails : MonoBehaviour
    {
        // Public Attributes
        [Tooltip("Delay between each trail element spawning (seconds)")]
        public float interval = 0.01f;              // Default value for arms     
        [Tooltip("Total number of trail elements (higher values increase trail length)\nNOTE: Too many trails impacts performance")]
        public int maximumTrails = 20;              // Default value for arms [NOTE: Too many trails impacts performance]
        [Tooltip("Trail object, typically a copy of the main object")]
        public GameObject trailPrefab;

        // Private Attributes
        private float _ElapsedTime;
        private Queue _TrailsQueue;
        private Material _ParentMateraial;

        // Start is called before the first frame update
        void Start()
        {
            _ElapsedTime = 0f;
            _TrailsQueue = new Queue();
            _ParentMateraial = GetComponent<MeshRenderer>().material;
        }

        // Update is called once per frame
        void Update()
        {
            _ElapsedTime += Time.deltaTime;

            if (_ElapsedTime >= interval)
            {
                // Instantiate trail game object (prefab)
                GameObject tmp = Instantiate(trailPrefab, transform);       
                tmp.transform.localScale = new Vector3(1f, 1f, 1f);
                tmp.transform.localPosition = Vector3.zero;
                tmp.transform.localRotation = Quaternion.identity;
                tmp.SetActive(true);
                tmp.GetComponent<MeshRenderer>().material.color = _ParentMateraial.color; 
                tmp.transform.SetParent(null);

                // Place into queue and reset timer
                _TrailsQueue.Enqueue(tmp);
                _ElapsedTime = 0f;
            }

            if (_TrailsQueue.Count == maximumTrails)
            {
                // Remove FIFO object from queue and destroy
                GameObject tmp = _TrailsQueue.Dequeue() as GameObject;
                Destroy(tmp);
            }
        }
    }
}