using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using AppsMinistry.Core.Input;

namespace JimRunner
{
    [RequireComponent(typeof(JimController))]
    public class JimUserControll : MonoBehaviour
    {
        private JimController m_Character;
        private bool m_Jump;

        private void Awake()
        {
            m_Character = GetComponent<JimController>();
        }

        private void OnEnable()
        {
            InputManager.Instance.OnTouchStart += OnJump;
        }


        private void OnJump(Vector3 position)
        {
            m_Jump = true;
        }

        private void FixedUpdate()
        {
            // Read the inputs.
            bool crouch = Input.GetKey(KeyCode.LeftControl);
            // Pass all parameters to the character control script.
            m_Character.Move(crouch, m_Jump);
            m_Jump = false;
        }
    }
}