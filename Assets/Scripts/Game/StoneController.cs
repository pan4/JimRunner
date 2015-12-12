using UnityEngine;
using System.Collections;

namespace JimRunner
{
    public class StoneController : MonoBehaviour
    {
        Transform _transform;

        [SerializeField]
        float _speed;

        private float _rotateSpeed;

        void Start()
        {
            _transform = transform;
            CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
            _rotateSpeed = (360 * _speed) / (2 * Mathf.PI * circleCollider.radius * circleCollider.transform.localScale.x);
            
        }

        private void Update()
        {
            //Debug.Log(_rotateSpeed);
            _transform.Translate(new Vector3(-_speed * Time.deltaTime, 0f, 0f), Space.World);
            _transform.Rotate(0f, 0f, _rotateSpeed * Time.deltaTime);
        }

    }
}