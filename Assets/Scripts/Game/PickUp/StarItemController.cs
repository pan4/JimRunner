using UnityEngine;

namespace JimRunner.Game.PickUp
{
    class StarItemController : PickupItemController
    {
        [SerializeField]
        private float _amplitude = 1f;

        [SerializeField]
        private float _speed = 1f;

        private float _position;
        private float _startPos_y;

        protected override void OnStart()
        {
            base.OnStart();
            //_position = _amplitude;
            //_startPos_y = Transform.position.y - _amplitude;
            _startPos_y = Transform.position.y;
            _position = 0;
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            //float pos_y = Mathf.PingPong(_position, _amplitude * 2f) + _startPos_y;
            //_position += Time.deltaTime * _speed;
            //Transform.position = new Vector3(Transform.position.x, pos_y, Transform.position.z);

            Transform.position = new Vector3(Transform.position.x, (float)BackEaseInOut(_position, _startPos_y, _startPos_y + 2 * _amplitude, 3), Transform.position.z);
            //_position += Time.deltaTime; 
            //_position = Mathf.Repeat(Time.time, 3f);
            _position = Mathf.PingPong(Time.time, 3);
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
        public static double BounceEaseInOut(double t, double b, double c, double d)
        {
            if (t < d / 2)
                return BounceEaseIn(t * 2, 0, c, d) * .5 + b;
            else
                return BounceEaseOut(t * 2 - d, 0, c, d) * .5 + c * .5 + b;
        }

        public static double BounceEaseIn(double t, double b, double c, double d)
        {
            return c - BounceEaseOut(d - t, 0, c, d) + b;
        }

        public static double BounceEaseOut(double t, double b, double c, double d)
        {
            if ((t /= d) < (1 / 2.75))
                return c * (7.5625 * t * t) + b;
            else if (t < (2 / 2.75))
                return c * (7.5625 * (t -= (1.5 / 2.75)) * t + .75) + b;
            else if (t < (2.5 / 2.75))
                return c * (7.5625 * (t -= (2.25 / 2.75)) * t + .9375) + b;
            else
                return c * (7.5625 * (t -= (2.625 / 2.75)) * t + .984375) + b;
        }

        public static double BackEaseInOut(double t, double b, double c, double d)
        {
            double s = 1.70158;
            if ((t /= d / 2) < 1)
                return c / 2 * (t * t * (((s *= (1.525)) + 1) * t - s)) + b;
            return c / 2 * ((t -= 2) * t * (((s *= (1.525)) + 1) * t + s) + 2) + b;
        }
    }
}
