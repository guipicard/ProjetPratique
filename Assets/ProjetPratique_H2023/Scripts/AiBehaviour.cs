using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

public class AiBehaviour : MonoBehaviour
{
    public float HP = 100;

    private Camera m_MainCamera;
    
    private GameObject player;
    [SerializeField] private Transform m_Bullet;
    [SerializeField] private Transform m_BulletSpawner;
    [SerializeField] private string m_DamageTag;
    private NavMeshAgent m_NavmeshAgent;
    private Animator m_Animator;

    private float m_PlayerDistance;
    [SerializeField] private float m_TriggerDistance;
    [SerializeField] private float m_attackDistance;

    private bool m_IsStabbing;
    private bool m_OutOfRange;

    [SerializeField] private Canvas m_AiCanvas;
    [SerializeField] private Slider m_HealthBar;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        m_NavmeshAgent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
        m_IsStabbing = false;
        m_OutOfRange = true;
        m_HealthBar.value = HP / 100;
        m_MainCamera = Camera.main;
        transform.parent.GetComponent<CrystalsBehaviour>().m_AiAlive++;
    }

    // Update is called once per frame
    void Update()
    {
        m_PlayerDistance = Vector3.Distance(player.transform.position, transform.position);
        StateToggler();
        if (HP <= 0)
        {
            transform.parent.GetComponent<CrystalsBehaviour>().m_AiAlive--;
            Destroy(gameObject);
        }
        
        m_AiCanvas.transform.rotation = player.GetComponent<PlayerController>().m_PlayerCanvas.transform.rotation;
        
    }

    private void StateToggler()
    {
        if (!m_IsStabbing)
        {
            if (m_PlayerDistance < m_attackDistance)
            {
                Attack();
            }
            else if (m_PlayerDistance < m_TriggerDistance)
            {
                Move();
            }
            else
            {
                if (!m_OutOfRange)
                {
                    m_OutOfRange = true;
                    m_NavmeshAgent.SetDestination(transform.position);
                }
            }
            Animate();
        }
        else
        {
            m_NavmeshAgent.SetDestination(transform.position);
            transform.LookAt(player.transform.position);
        }
    }


    private void Move()
    {
        if (m_OutOfRange)
        {
            m_OutOfRange = false;
        }

        m_NavmeshAgent.destination = player.transform.position;
    }

    private void Attack()
    {
        if (m_OutOfRange)
        {
            m_OutOfRange = false;
        }

        m_IsStabbing = true;
        m_NavmeshAgent.velocity = Vector3.zero;
        m_Animator.SetTrigger("Stab");
    }

    private void Animate()
    {
        m_Animator.SetBool("Running", m_NavmeshAgent.velocity != Vector3.zero);
    }
    
    private void LaunchBasicAttack()
    {
        Transform newBullet = Instantiate(m_Bullet);
        newBullet.position = m_BulletSpawner.position;
        newBullet.rotation = transform.rotation;
    }

    private void EndAttack()
    {
        m_IsStabbing = false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(m_DamageTag))
        {
            HP -= 25;
            m_HealthBar.value = HP / 100;
            if (HP > 0)
            {
                Destroy(other.gameObject);
            }
        }
    }
}
