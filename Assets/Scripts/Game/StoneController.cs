using UnityEngine;
using System.Collections;

namespace JimRunner
{
    public class StoneController : MonoBehaviour
    {

        Rigidbody2D _rigidbody2D;
        Transform _transform;
        
        [SerializeField]
        float _speed;

        float _rotateSpeed;

        void Start()
        {
            Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
            rigidbody2D.velocity = new Vector2(-_speed, 0f);

            _transform = transform;

            CircleCollider2D circleCollider = GetComponent<CircleCollider2D>();
            _rotateSpeed = (360 * _speed)/ (2 * Mathf.PI * circleCollider.radius);
        }

        private void Update()
        {
            _transform.Rotate(0f, 0f, _rotateSpeed * Time.deltaTime);
        }

    }
}