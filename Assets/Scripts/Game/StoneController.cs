using UnityEngine;
using System.Collections;
using Core;

namespace JimRunner
{
    public class StoneController : BaseMonoBehaviour
    {
        [SerializeField]
        Transform _shadow;

        [SerializeField]
        float _speed;

        protected float _rotationSpeed;
        private Vector3 _shadowOffset;

        protected override void OnStart()
        {
            base.OnStart();
            CircleCollider2D circleCollider = Transform.GetComponent<CircleCollider2D>();
            _rotationSpeed = (360 * _speed) / (2 * Mathf.PI * circleCollider.radius * circleCollider.transform.localScale.x);
            _shadowOffset = _shadow.localPosition - Transform.localPosition;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            Transform.Rotate(0f, 0f, _rotationSpeed * Time.deltaTime);
            Transform.Translate(new Vector3(-_speed * Time.deltaTime, 0f, 0f), Space.World);
            _shadow.position = Transform.position + _shadowOffset;
        }

        protected override void OnDestroyed()
        {
            base.OnDestroyed();
            Destroy(Transform.parent.gameObject);
        }


    }
}