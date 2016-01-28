using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace JimRunner
{
    public class LocationManager : MonoBehaviour
    {

        [SerializeField]
        private SpawnHandler [] _locationSpawners;

        [SerializeField]
        private float _locationSpan = 3;

        [SerializeField]
        private float _transitionDelay = 3f;

        private float _nextLocation;
        private int _index = 0;

        TransitionController _transitionController;
        CameraFade _cameraFade;

        private void Awake()
        {
            //foreach (var locatin in _locationSpawners)
            //locatin.SetActive(false);

            //UpdateLocation(_index);
            _nextLocation = Time.time + _locationSpan;

            _transitionController = FindObjectOfType<TransitionController>();
            _cameraFade = Camera.main.GetComponent<CameraFade>();
        } 

        void Update()
        {
            if(_nextLocation < Time.time)
            {                
                UpdateLocation(_index);
                _index = ++_index % _locationSpawners.Length;
                _nextLocation = Time.time + _locationSpan;
            }
        }

        private void UpdateLocation(int index)
        {
            _locationSpawners[index].TileTransitonGround();
            StartCoroutine(StartTransition(index));
        }

        private IEnumerator StartTransition(int index)
        {
            yield return new WaitForSeconds(_transitionDelay);
            //if (Time.timeSinceLevelLoad > _locationSpan)
            //{
            //    _transitionController.enabled = true;
            //    _cameraFade.StartFade(new Color(0, 0, 0, 0), 1f, 1f, 1f);
            //}
            if (!(index < 0 || index >= _locationSpawners.Length))
            {
                _locationSpawners[index].SetTransparency();

                _nextLocation = Time.time + _locationSpan;
            }

        }
    }
}