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
        [SerializeField]
        private float lookAheadFactor = 3;
        [SerializeField]
        private float lookAheadReturnSpeed = 0.5f;
        [SerializeField]
        private float lookAheadMoveThreshold = 0.1f;



        private float m_OffsetZ;
        private Vector3 m_LastTargetPosition;
        private Vector3 m_CurrentVelocity;
        private Vector3 m_LookAheadPos;

        [SerializeField]
        private int pixelToUnits = 100;

        // Use this for initialization
        private void Start()
        {
            m_LastTargetPosition = target.position + offset;
            m_OffsetZ = (transform.position - target.position).z;
            transform.parent = null;

            Application.targetFrameRate = 20;
        }


        // Update is called once per frame
        private void Update()
        {
            // only update lookahead pos if accelerating or changed direction
            float xMoveDelta = (target.position + /*offset*/ - m_LastTargetPosition).x;

            bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

            if (updateLookAheadTarget)
            {
                m_LookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
            }
            else
            {
                m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
            }

            Vector3 aheadTargetPos = target.position + offset + m_LookAheadPos /*+ Vector3.forward * m_OffsetZ*/;
            Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

            //float rounded_x = RoundToNearestPixel(newPos.x);
            //float rounded_y = RoundToNearestPixel(newPos.y);

            //transform.position = new Vector3(rounded_x, Mathf.Clamp(rounded_y, 3f, 4.2f), newPos.z);
            transform.position = new Vector3(newPos.x, Mathf.Clamp(newPos.y, 3f, 4.2f), newPos.z);

            m_LastTargetPosition = target.position /*+ offset*/;
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