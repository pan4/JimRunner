using UnityEngine;
using System.Collections;
namespace JimRunner
{
    public class TransitionController : MonoBehaviour
    {
        [SerializeField]
        private float _transitionTime = 10f;

        [SerializeField]
        private Transform _player;

        [SerializeField]
        private Transform _mainCloud;
        [SerializeField]
        private Transform _firstRock;
        [SerializeField]
        private Transform _cloud;
        [SerializeField]
        private Transform _secondRock;
        [SerializeField]
        private Transform _sky;

        [HideInInspector]
        public GameObject transitionMainCloud;
        [HideInInspector]
        public GameObject transitionFirstRock;
        [HideInInspector]
        public GameObject transitionCloud;
        [HideInInspector]
        public GameObject transitionSecondRock;
        [HideInInspector]
        public GameObject transitionSky;

        private float _mainCloudSpeed;
        private float _firstRockSpeed;
        private float _cloudSpeed;
        private float _secondRockSpeed;
        private float _skySpeed;

        void OnEnable()
        {
            _mainCloudSpeed     = (transitionMainCloud.transform.position.x - _player.position.x + 3) / _transitionTime;
            _firstRockSpeed     = (transitionFirstRock.transform.position.x - _player.position.x + 3) / _transitionTime;
            _cloudSpeed         = (transitionCloud.transform.position.x - _player.position.x + 3) / _transitionTime;
            _secondRockSpeed    = (transitionSecondRock.transform.position.x - _player.position.x + 3) / _transitionTime;
            _skySpeed           = (transitionSky.transform.position.x - _player.position.x + 3) / _transitionTime;
        

            StartCoroutine(DisableThis());
        }

        private IEnumerator DisableThis()
        {
            yield return new WaitForSeconds(_transitionTime);
            this.enabled = false;
        }

        void Update()
        {
            _mainCloud.Translate(new Vector3(-_mainCloudSpeed,0) * Time.deltaTime, Space.World);
            _firstRock.Translate(new Vector3(-_firstRockSpeed, 0) * Time.deltaTime, Space.World);
            _cloud.Translate(new Vector3(-_cloudSpeed, 0) * Time.deltaTime, Space.World);
            _secondRock.Translate(new Vector3(-_secondRockSpeed, 0) * Time.deltaTime, Space.World);
            _sky.Translate(new Vector3(-_skySpeed, 0) * Time.deltaTime, Space.World);
        }

        
    }
}
