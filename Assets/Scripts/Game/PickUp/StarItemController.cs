using UnityEngine;

namespace JimRunner.Game.PickUp
{
    class StarItemController : PickupItemController
    {
        [SerializeField]
        private float _amplitude = 1f;

        [SerializeField]
        private float _speed = 1f;

        private float _time;
        private float _startPos_y;
        private float _endPos_y;

        protected override void OnStart()
        {
            base.OnStart();
            _startPos_y = Transform.position.y;
            _endPos_y = _startPos_y + _amplitude;
            _time = 0;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            Transform.position = new Vector3(Transform.position.x, (float)BackEaseInOut(_time, _startPos_y, _endPos_y, _speed), Transform.position.z);
            _time = Mathf.PingPong(Time.time, _speed);
        }

        /// <summary>
        /// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>

        public static double BackEaseInOut(double t, double b, double c, double d)
        {
            double s = 1.70158;
            if ((t /= d / 2) < 1)
                return c / 2 * (t * t * (((s *= (1.525)) + 1) * t - s)) + b;
            return c / 2 * ((t -= 2) * t * (((s *= (1.525)) + 1) * t + s) + 2) + b;
        }
    }
}
