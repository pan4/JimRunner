using UnityEngine;
using System.Collections.Generic;

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

        private float _nextLocation;
        private int _index = 0;

        private void Awake()
        {
            foreach (var locatin in _locationSpawners)
                locatin.SetActive(false);

            UpdateLocation(_index);
            _nextLocation = Time.time + LocationSpan;
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
        }
    }
}