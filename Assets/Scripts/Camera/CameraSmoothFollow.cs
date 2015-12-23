using UnityEngine;
using System.Collections;

namespace JimRunner
{
    public class CameraSmoothFollow : MonoBehaviour
    {
        [SerializeField]
        private Transform target;
        [SerializeField]
        private Vector3 offset = Vector3.zero;
        [SerializeField]
        private float damping = 1;

        private Vector3 m_LastTargetPosition;
        private Vector3 m_CurrentVelocity;

        [SerializeField]
        private int pixelToUnits = 100;

        // Use this for initialization
        private void Start()
        {
            m_LastTargetPosition = target.position + offset;
            transform.parent = null;

            Application.targetFrameRate = 20;
        }


        // Update is called once per frame
        private void Update()
        {
            Vector3 newPos = Vector3.SmoothDamp(transform.position, target.position + offset, ref m_CurrentVelocity, damping);

            float rounded_x = RoundToNearestPixel(newPos.x);
            float rounded_y = RoundToNearestPixel(newPos.y);

            transform.position = new Vector3(rounded_x, Mathf.Clamp(rounded_y, 3f, 4.2f), newPos.z);
            //transform.position = new Vector3(newPos.x, Mathf.Clamp(newPos.y, 3f, 4.2f), newPos.z);

            m_LastTargetPosition = target.position;
        }

        public float RoundToNearestPixel(float unityUnits)
        {
            float valueInPixels = unityUnits * pixelToUnits;
            valueInPixels = Mathf.Round(valueInPixels);
            float roundedUnityUnits = valueInPixels  / pixelToUnits;
            return roundedUnityUnits;
        }
    }

}