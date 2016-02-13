using UnityEngine;
using System.Collections;

namespace JimRunner
{
    public class JimController : MonoBehaviour
    {
        [SerializeField]
        private float m_Speed = 0.5f;
        [SerializeField]
        private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
        [SerializeField]
        private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
        [Range(0, 1)]
        [SerializeField]
        private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
        [SerializeField]
        private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
        [SerializeField]
        private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

        private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
        const float k_GroundedRadius = 0.2f; // Radius of the overlap circle to determine if grounded

        private bool _grounded;
        private bool Grounded            // Whether or not the player is grounded.
        {
            get
            {
                return _grounded;
            }
            set
            {
                if (value != _grounded)
                {
                    if (value )
                    {
                        if (JumpCounter != 0)
                            JumpCounter = 0;
                                                
                       // m_Rigidbody2D.velocity = new Vector2(m_Speed * m_MaxSpeed, 0f);
                    }
                    _grounded = value;
                }
            }
        }

        [SerializeField]
        private int jumpCount = 1;

        private int jumpCounter = 0;
        private int JumpCounter
        {
            get
            {
                return jumpCounter;
            }
            set
            {
                jumpCounter = value;
            }
        }

        private Transform m_CeilingCheck;   // A position marking where to check for ceilings
        const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        private Animator m_Anim;            // Reference to the player's animator component.
        private Rigidbody2D m_Rigidbody2D;
        private bool m_FacingRight = true;  // For determining which way the player is currently facing.

        private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_CeilingCheck = transform.Find("CeilingCheck");
            m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (_damaged)
                OnDamaged();
        }

        private void FixedUpdate()
        {
            //Grounded = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);

            if (colliders.Length == 0)
                Grounded = false;

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    Grounded = true;
                    break;
                }
                if (i == (colliders.Length - 1))
                    Grounded = false;
            }
            m_Anim.SetBool("Ground", Grounded);

            // Set the vertical animation
            m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
            //Debug.Log(m_Rigidbody2D.velocity.y);
        }

        
        public void Move(bool crouch, bool jump)
        {
            // If crouching, check to see if the character can stand up
            if (!crouch && m_Anim.GetBool("Crouch"))
            {
                // If the character has a ceiling preventing them from standing up, keep them crouching
                if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
                {
                    crouch = true;
                }
            }

            // Set whether or not the character is crouching in the animator
            m_Anim.SetBool("Crouch", crouch);

            //only control the player if grounded or airControl is turned on
            if (Grounded || m_AirControl)
            {
                // Reduce the speed if crouching by the crouchSpeed multiplier
                m_Speed = (crouch ? m_Speed * m_CrouchSpeed : m_Speed);

                // The Speed animator parameter is set to the absolute value of the horizontal input.
                m_Anim.SetFloat("Speed", Mathf.Abs(m_Speed));

                // Move the character
                m_Rigidbody2D.velocity = new Vector2(m_Speed * m_MaxSpeed, m_Rigidbody2D.velocity.y);

                // If the input is moving the player right and the player is facing left...
                if (m_Speed > 0 && !m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
                // Otherwise if the input is moving the player left and the player is facing right...
                else if (m_Speed < 0 && m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
            }
            // If the player should jump...
            if (/*m_Grounded &&*/ jump && (jumpCounter < jumpCount )/*&& m_Anim.GetBool("Ground")*/)
            {
                // Add a vertical force to the player.
                //Grounded = false;
                if (JumpCounter != 0 || (Grounded && m_Anim.GetBool("Ground")))
                {
                    m_Anim.SetBool("Ground", false);
                    m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0f);
                    m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
                    JumpCounter++;
                }
            }
        }
                
        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        private bool _damaged = false;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("EnemyLayer"))
            {                
                _damaged = true;
            }
        }


        private float _nextBlink = 0;
        private float _blinkInterval = 0.1f;
        private const float _blinkCount = 8;
        private float _blinkCounter = 0;
        private void OnDamaged()
        {
            Color color = _spriteRenderer.color;
            if (Time.time > _nextBlink)
            {
                if (_blinkCounter % 2 == 0)
                    color.a = color.a * 0.25f;
                else
                    color.a = color.a * 4f;
                _spriteRenderer.color = color;
                _nextBlink = Time.time + _blinkInterval;
                _blinkCounter++;
                if (_blinkCounter == _blinkCount)
                {
                    _damaged = false;
                    _blinkCounter = 0f;
                }
            }               
        }
    }
}