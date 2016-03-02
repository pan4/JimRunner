using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace JimRunner
{
    class WeelController : StoneController
    {
        [SerializeField]
        Transform _back;

        Vector3 _backOffset;

        protected override void OnStart()
        {
            base.OnStart();
            _backOffset = _back.localPosition - Transform.localPosition;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            _back.position = Transform.position + _backOffset;
            _back.Rotate(0f, 0f, _rotationSpeed * Time.deltaTime);
        }
    }
}
