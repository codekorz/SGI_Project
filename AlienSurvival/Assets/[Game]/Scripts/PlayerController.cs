﻿using UnityEngine;
using System.Collections;

enum PlayerStatus {Idle, Move, Punch};

public class PlayerController : MonoBehaviour {
	public float m_defaultSpeed = 20f;
	public float m_runSpeed = 40f;
	public float m_gravity = -9.8f;
	private Animator m_animator;
	private CharacterController m_charController;
	private Vector3 m_moveDirection = Vector3.zero;
	private float m_speed;
    private PlayerStatus m_status;
	// Use this for initialization
	void Start () {
        m_status = PlayerStatus.Idle;
		m_speed = m_defaultSpeed;
	   	m_animator = GetComponent<Animator>();
		m_charController = GetComponent<CharacterController> ();
	}

	void FixedUpdate () {
        transform.Rotate(0, Input.GetAxisRaw("Horizontal") * 60 * Time.deltaTime, 0);
        float horizontal = Input.GetAxisRaw ("Horizontal");
		float vertical = Input.GetAxisRaw ("Vertical");

        if (vertical != 0) {
            m_status = PlayerStatus.Move;
            if(vertical > 0 && Input.GetKeyDown(KeyCode.LeftShift))
            {
                m_speed = m_runSpeed;
            }
            else if(Input.GetKeyUp(KeyCode.LeftShift) || vertical < 0)
            {
                m_speed = m_defaultSpeed;
            }
        }
        else m_status = PlayerStatus.Idle;
        if (Input.GetKey(KeyCode.Space) && !m_animator.GetCurrentAnimatorStateInfo(0).IsName("Punch"))
            m_status = PlayerStatus.Punch;
        
		Animation(horizontal, vertical);
		Move (horizontal, vertical);

        Debug.Log("V: " + vertical + "Status: " + m_status.ToString());
    }

	void Animation(float h, float v)
	{
        m_animator.SetFloat("speed", m_speed);
        m_animator.SetFloat("walkDirection", v);
        m_animator.SetBool("move", (m_status == PlayerStatus.Move));
		m_animator.SetBool("punch", (m_status == PlayerStatus.Punch));

	}

	void Move(float h, float v) {
        if (m_charController.isGrounded) {
            if(h != 0 && v != 0)
            {
                m_moveDirection = new Vector3(h, 0, v);
            }
            else { m_moveDirection = new Vector3(0, 0, v); }
            m_moveDirection = transform.TransformDirection(m_moveDirection);
            m_moveDirection *= m_speed;
        }
		m_moveDirection.y += m_gravity * Time.deltaTime;
		m_charController.Move (m_moveDirection * Time.deltaTime);
    }
}