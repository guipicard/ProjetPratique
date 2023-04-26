using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    public float HP;
    [SerializeField] private Transform m_BulletSpawner;
    [SerializeField] private Transform m_Bullet;

    [SerializeField] private string m_DamageTag;

    private NavMeshAgent m_NavMeshAgent;
    private Animator m_Animator;

    private Camera mainCamera;
    [SerializeField] private Vector3 m_CameraOffset;

    private bool m_CanMove;

    private Ray m_MouseRay;
    private RaycastHit m_HitInfo;

    private GameObject m_TargetCrystal;

    public int m_BlueCrystals;
    public int m_RedCrystals;
    public int m_YellowCrystals;
    public int m_GreenCrystals;

    private int m_BlueCrystalsInventory;
    private int m_RedCrystalsInventory;
    private int m_YellowCrystalsInventory;
    private int m_GreenCrystalsInventory;

    [SerializeField] private TextMeshPro m_BlueCrystalsText;
    [SerializeField] private TextMeshPro m_RedCrystalsText;
    [SerializeField] private TextMeshPro m_YellowCrystalsText;
    [SerializeField] private TextMeshPro m_GreenCrystalsText;

    [SerializeField] private Image m_BlueCrystalImage;
    [SerializeField] private Image m_RedCrystalImage;
    [SerializeField] private Image m_YellowCrystalImage;
    [SerializeField] private Image m_GreenCrystalImage;

    [SerializeField] public Canvas m_PlayerCanvas;
    [SerializeField] private Slider m_HealthBar;

    void Start()
    {
        HP = 100.0f;
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
        mainCamera = Camera.main;
        m_TargetCrystal = null;

        m_BlueCrystals = 1;
        m_RedCrystals = 1;
        m_YellowCrystals = 1;
        m_GreenCrystals = 1;

        m_BlueCrystalsInventory = 0;
        m_RedCrystalsInventory = 0;
        m_YellowCrystalsInventory = 0;
        m_GreenCrystalsInventory = 0;

        m_BlueCrystalsText.text = m_BlueCrystalsInventory.ToString();
        m_RedCrystalsText.text = m_RedCrystalsInventory.ToString();
        m_YellowCrystalsText.text = m_YellowCrystalsInventory.ToString();
        m_GreenCrystalsText.text = m_GreenCrystalsInventory.ToString();

        ChangeSpellState();
        UpdateHealthBar();
        
    }

    void Update()
    {
        CameraFollow();
        Move();
        Animate();
        if (m_TargetCrystal != null)
        {
            if (Vector3.Distance(transform.position, m_TargetCrystal.transform.position) <= 2.5f)
            {
                Mine();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && m_GreenCrystalsInventory >= 100)
        {
            GreenSpell();
        }
        
        m_PlayerCanvas.transform.LookAt(mainCamera.transform.position);
    }

    private void CameraFollow()
    {
        mainCamera.transform.position = transform.position - m_CameraOffset;
        mainCamera.transform.LookAt(transform.position);
    }

    private void Move()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_MouseRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(m_MouseRay, out m_HitInfo))
            {
                transform.LookAt(m_HitInfo.point);
                if (m_HitInfo.collider.gameObject.layer == 7)
                {
                    Attack();
                }

                if (m_HitInfo.collider.CompareTag("Ground"))
                {
                    m_NavMeshAgent.destination = m_HitInfo.point;
                }

                if (m_HitInfo.collider.gameObject.layer == 6)
                {
                    m_NavMeshAgent.stoppingDistance = 2.0f;
                    m_NavMeshAgent.destination = m_HitInfo.point;
                    m_TargetCrystal = m_HitInfo.collider.gameObject;
                }
                else
                {
                    m_TargetCrystal = null;
                    m_NavMeshAgent.stoppingDistance = 0;
                }
            }
        }
    }

    private void Animate()
    {
        m_Animator.SetBool("Running", m_NavMeshAgent.velocity != Vector3.zero);
    }

    private void Attack()
    {
        m_NavMeshAgent.destination = transform.position;
        transform.LookAt(m_HitInfo.collider.transform.position);
        m_Animator.SetTrigger("Stab");
    }

    private void LaunchBasicAttack()
    {
        Transform newBullet = Instantiate(m_Bullet);
        newBullet.position = m_BulletSpawner.position;
        newBullet.rotation = transform.rotation;
    }

    // private void CanMoveToggle()
    // {
    //     m_CanMove = !m_CanMove;
    // }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(m_DamageTag))
        {
            TakeDmg(20.0f);
            UpdateHealthBar();
            Destroy(other.gameObject);
        }
    }

    private void Mine()
    {
        m_HitInfo.collider.GetComponent<CrystalEvents>().GetMined();
    }

    public void GetMinerals(string color)
    {
        if (color == "Red")
        {
            m_RedCrystals++;
            if (m_RedCrystals % 2 == 0) m_RedCrystalsInventory++;
            m_RedCrystalsText.text = m_RedCrystalsInventory.ToString();
        }

        if (color == "Green")
        {
            m_GreenCrystals++;
            if (m_GreenCrystals % 2 == 0) m_GreenCrystalsInventory++;
            m_GreenCrystalsText.text = m_GreenCrystalsInventory.ToString();
        }

        if (color == "Yellow")
        {
            m_YellowCrystals++;
            if (m_YellowCrystals % 2 == 0) m_YellowCrystalsInventory++;
            m_YellowCrystalsText.text = m_YellowCrystalsInventory.ToString();
        }

        if (color == "Blue")
        {
            m_BlueCrystals++;
            if (m_BlueCrystals % 2 == 0) m_BlueCrystalsInventory++;
            m_BlueCrystalsText.text = m_BlueCrystalsInventory.ToString();
        }

        ChangeSpellState();
    }

    private void ChangeSpellState()
    {
        if (m_GreenCrystalsInventory > 100)
        {
            Color currentColor = m_GreenCrystalImage.color;
            currentColor.a = 1.0f;
            m_GreenCrystalImage.color = currentColor;
        }
        else
        {
            Color currentColor = m_GreenCrystalImage.color;
            currentColor.a = 0.2f;
            m_GreenCrystalImage.color = currentColor;
        }
        
        if (m_RedCrystalsInventory > 100)
        {
            Color currentColor = m_RedCrystalImage.color;
            currentColor.a = 1.0f;
            m_RedCrystalImage.color = currentColor;
        }
        else
        {
            Color currentColor = m_RedCrystalImage.color;
            currentColor.a = 0.2f;
            m_RedCrystalImage.color = currentColor;
        }
        
        if (m_YellowCrystalsInventory > 100)
        {
            Color currentColor = m_YellowCrystalImage.color;
            currentColor.a = 1.0f;
            m_YellowCrystalImage.color = currentColor;
        }
        else
        {
            Color currentColor = m_YellowCrystalImage.color;
            currentColor.a = 0.2f;
            m_YellowCrystalImage.color = currentColor;
        }
        
        if (m_BlueCrystalsInventory > 100)
        {
            Color currentColor = m_BlueCrystalImage.color;
            currentColor.a = 1.0f;
            m_BlueCrystalImage.color = currentColor;
        }
        else
        {
            Color currentColor = m_BlueCrystalImage.color;
            currentColor.a = 0.2f;
            m_BlueCrystalImage.color = currentColor;
        }
    }

    private void UpdateHealthBar()
    {
        m_HealthBar.value = HP / 100;
    }

    private void GreenSpell()
    {
        HP += 30.0f;
        if (HP >= 100)
        {
            HP = 100;
        }
        m_GreenCrystalsInventory -= 100;
        m_GreenCrystalsText.text = m_GreenCrystalsInventory.ToString();
        ChangeSpellState();
        UpdateHealthBar();
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
}