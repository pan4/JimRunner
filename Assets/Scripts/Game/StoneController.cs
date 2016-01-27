using UnityEngine;
using System.Collections;

namespace JimRunner
{
    public class StoneController : MonoBehaviour
    {
        //[SerializeField]
        //Transform _rotation;

        [SerializeField]
        Transform _shadow;

        Transform _transform;

        [SerializeField]
        float _speed;

        private float _rotationSpeed;
        private Vector3 _shadowOffset;

        void Start()
        {
            _transform = transform;
            CircleCollider2D circleCollider = _transform.GetComponent<CircleCollider2D>();
            _rotationSpeed = (360 * _speed) / (2 * Mathf.PI * circleCollider.radius * circleCollider.transform.localScale.x);
            _shadowOffset = _shadow.localPosition; 
        }

        private void Update()
        {
            _transform.Rotate(0f, 0f, _rotationSpeed * Time.deltaTime);
            _transform.Translate(new Vector3(-_speed * Time.deltaTime, 0f, 0f), Space.World);
            _shadow.position = _transform.position + _shadowOffset;
        }

        private void OnDestroy()
        {
            Destroy(_transform.parent.gameObject);
        }

    }
}