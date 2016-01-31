using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace JimRunner
{
    public enum LocationType
    {
        Day,
        Evening,
        Size,
        Night,
        Winter,

        Transition
    }

    public class LocationManager : MonoBehaviour
    {
        private SpawnHandler _spawnHandler;

        [SerializeField]
        private float _locationSpan = 3;

        [SerializeField]
        private float _transitionDelay = 3f;

        private float _nextLocation;
        private int _index = 0;


        private static LocationType _currentLocation;
        public static LocationType CurrentLocation
        {
            get
            {
                return _currentLocation;
            }
        }

        private static LocationType _previousLocation;
        public static LocationType PrevioustLocation
        {
            get
            {
                return _previousLocation;
            }
            set
            {
                _previousLocation = value;
            }
        }

        private void Awake()
        {
            _spawnHandler = FindObjectOfType<SpawnHandler>();
            _nextLocation = Time.time + _locationSpan;
            _previousLocation = LocationType.Day;
            _currentLocation = LocationType.Day;
        } 

        void Update()
        {
            if(_nextLocation < Time.time)
            {                
                StartNewLocation(_index);
                _index = ++_index % (int)LocationType.Size;
                _nextLocation = Time.time + _locationSpan;
                _currentLocation = (LocationType)_index;
            }
        }

        private void StartNewLocation(int index)
        {
            _spawnHandler.SpawnTransitionGround((LocationType)index);
            StartCoroutine(StartTransition());
        }

        private IEnumerator StartTransition()
        {
            yield return new WaitForSeconds(_transitionDelay);
            
            //_previousLocation = _currentLocation;
            _spawnHandler.SetTransparency();

            _nextLocation = Time.time + _locationSpan;
            
        }
    }
}