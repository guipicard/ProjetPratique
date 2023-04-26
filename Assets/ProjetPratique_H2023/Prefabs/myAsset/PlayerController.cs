using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_Speed;
    [SerializeField] private float m_RotationSpeed;
    [SerializeField] private string m_DamageTag;
    [SerializeField] private Transform m_Bullet;
    [SerializeField] private Transform m_BulletSpawner;
    
    private Vector3 m_Direction;
    private Vector3 m_CurrentVelocity;

    private Rigidbody m_RigidBody;
    private Animator m_Animator;

    private Quaternion targetRotation;

    private bool m_Ground;

    private static readonly int Running = Animator.StringToHash("Running");
    private static readonly int Jumping = Animator.StringToHash("Jumping");
    private static readonly int JumpId = Animator.StringToHash("Jump");
    private static readonly int Falling = Animator.StringToHash("Falling");

    private Camera m_MainCamera;
    private Ray m_MouseRay;
    private RaycastHit m_HitInfo;

    private Vector3 m_Destination;
    private float m_StoppingDistance;
    private float m_Distance;

    private GameObject m_TargetCrystal;
    
    public float HP = 100;
    public Canvas m_PlayerCanvas;
    [SerializeField] private Slider m_HealthBar;

    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_Animator = GetComponent<Animator>();
        
        m_Direction = Vector3.zero;

        targetRotation = Quaternion.identity;

        m_Ground = true;
        
        m_MainCamera = Camera.main;

        UpdateHealthBar();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_MouseRay = m_MainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(m_MouseRay, out m_HitInfo))
            {
                if (m_HitInfo.collider.gameObject.layer == 7)
                {
                    Attack();
                }

                if (m_HitInfo.collider.gameObject.layer == 8)
                {
                    m_Destination = m_HitInfo.point;
                }

                if (m_HitInfo.collider.gameObject.layer == 6)
                {
                    m_StoppingDistance = 3.0f;
                    m_Destination = m_HitInfo.point;
                    m_TargetCrystal = m_HitInfo.collider.gameObject;
                }
                else
                {
                    m_TargetCrystal = null;
                    m_StoppingDistance = 0;
                }
            }
        }

        GetDirection();
        if (m_TargetCrystal != null)
        {
            if (Vector3.Distance(transform.position, m_TargetCrystal.transform.position) <= 2.5f)
            {
                Mine();
            }
        }
    }

    private void FixedUpdate()
    {
        m_CurrentVelocity = m_RigidBody.velocity;
        if (m_Direction != Vector3.zero)
        {
            Move();
            Rotate();
        }
        else
        {
            m_CurrentVelocity.x = 0;
            m_CurrentVelocity.z = 0;
        }
        m_RigidBody.velocity = m_CurrentVelocity;
        Animate();
        m_PlayerCanvas.transform.LookAt(m_MainCamera.transform.position);
    }

    private void GetDirection()
    {
        if (Vector3.Distance(transform.position, m_Destination) > m_StoppingDistance + 0.25f)
        {
            m_Direction = (m_Destination - transform.position).normalized;
            m_Direction.y = 0;
        }
        else
        {
            m_Direction = Vector3.zero;
        }
    }

    private void Move()
    {
        m_CurrentVelocity.x = m_Direction.x * m_Speed;
        m_CurrentVelocity.z = m_Direction.z * m_Speed;
    }

    private void Rotate()
    {
        if (m_Direction != Vector3.zero) targetRotation = Quaternion.LookRotation(m_Direction, Vector3.up);

        if (transform.rotation != targetRotation)
        {
            // Slerp looks smoother than Lerp
            transform.rotation = Quaternion.Slerp(m_RigidBody.rotation, targetRotation, Time.fixedDeltaTime * m_RotationSpeed);
        }
    }

    private void Animate()
    {
        // run
        m_Animator.SetBool(Running, m_Direction != Vector3.zero);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(m_DamageTag))
        {
            TakeDmg(20.0f);
            UpdateHealthBar();
            Destroy(other.gameObject);
        }
    }

    private void UpdateHealthBar()
    {
        m_HealthBar.value = HP / 100;
    }
    
    public void TakeDmg(float damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            Death();
        }
    }
    
    private void Death()
    {
        HP = 0;
    }
    
    private void Mine()
    {
        m_HitInfo.collider.GetComponent<CrystalEvents>().GetMined();
    }
    
    private void Attack()
    {
        m_Destination = transform.position;
        transform.LookAt(m_HitInfo.collider.transform.position);
        m_Animator.SetTrigger("Attack");
    }
    
    private void LaunchBasicAttack()
    {
        Transform newBullet = Instantiate(m_Bullet);
        newBullet.position = m_BulletSpawner.position;
        newBullet.rotation = transform.rotation;
    }
}