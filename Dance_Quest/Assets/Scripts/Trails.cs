using System.Collections;
using UnityEngine;

namespace DanceQuest
{
    public class Trails : MonoBehaviour
    {
        public GameObject trailPrefab;

        private const float _INTERVAL = 0.01f;
        private const int _MAXTRAILS = 15;
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
                GameObject tmp = Instantiate(trailPrefab, transform);       // TBC: May need PhotonNetwork.Instantiate for networking
                tmp.transform.localScale = new Vector3(1f, 1f, 1f);
                tmp.transform.localPosition = Vector3.zero;
                tmp.transform.localRotation = Quaternion.identity;
                tmp.SetActive(true);
                tmp.GetComponent<MeshRenderer>().material.color = parentColour;
                tmp.transform.SetParent(null);
                _TrailsQueue.Enqueue(tmp);
                _ElapsedTime = 0f;
            }

            if (_TrailsQueue.Count == _MAXTRAILS)
            {
                GameObject tmp = _TrailsQueue.Dequeue() as GameObject;
                Destroy(tmp);
            }
        }
    }
}