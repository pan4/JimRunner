using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace JimRunner
{
    public class LocationManager : MonoBehaviour
    {

        [SerializeField]
        private GameObject [] _locationSpawners;

        [SerializeField]
        private float _locationSpan = 3;
        private float LocationSpan
        {
            get
            {
                return _locationSpan * 60f;
            }
        }

        [SerializeField]
        private float _transitionDelay = 3f;

        private float _nextLocation;
        private int _index = 0;

        TransitionController _transitionController;
        CameraFade _cameraFade;

        private void Awake()
        {
            foreach (var locatin in _locationSpawners)
                locatin.SetActive(false);

            UpdateLocation(_index);

            _transitionController = FindObjectOfType<TransitionController>();
            _cameraFade = Camera.main.GetComponent<CameraFade>();
        } 

        void Update()
        {
            if(_nextLocation < Time.time)
            {
                _index = ++_index % _locationSpawners.Length ;
                
                UpdateLocation(_index);
                _nextLocation = Time.time + LocationSpan;
            }
        }

        private void UpdateLocation(int index)
        {

            if (index < 0 || index >= _locationSpawners.Length)
                return;

            if (index != 0)
                _locationSpawners[index - 1].SetActive(false);
            else
                _locationSpawners[_locationSpawners.Length - 1].SetActive(false);

            _locationSpawners[index].SetActive(true);
                        
            StartCoroutine(StartTransition());

            _nextLocation = Time.time + LocationSpan;
        }

        private IEnumerator StartTransition()
        {
            yield return new WaitForSeconds(_transitionDelay);
            if (Time.timeSinceLevelLoad > LocationSpan)
            {
                _transitionController.enabled = true;
                _cameraFade.StartFade(new Color(0, 0, 0, 0), 1f, 1f, 1f);
            }
        }
    }
}