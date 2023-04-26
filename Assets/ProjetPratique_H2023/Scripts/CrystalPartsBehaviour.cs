using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;
using Random = UnityEngine.Random;

public class CrystalPartsBehaviour : MonoBehaviour
{
    [SerializeField] private float m_Speed;
    [SerializeField] private float m_ExplosionForce;
    [SerializeField] private float m_RedirectionTime;
    [SerializeField] private string m_Color;
    private GameObject m_Player;
    private Vector3 heightOffset;
    private Rigidbody m_Rigidbody;
    private Vector3 m_InitialDirection;
    private float m_DirectionLerpTime;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Player = GameObject.FindWithTag("Player");
        heightOffset = new Vector3(0, 1, 0);
        m_InitialDirection =
            new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(0, 1.0f), Random.Range(-1.0f, 1.0f)) *
            m_ExplosionForce;
        m_Rigidbody.AddForce(m_InitialDirection, ForceMode.Impulse);
        m_DirectionLerpTime = 0;
    }

    void Update()
    {
        Vector3 playerPosHeight = m_Player.transform.position + heightOffset;
        Vector3 playerPosDifference = (playerPosHeight - transform.position).normalized * m_Speed;
        if (m_DirectionLerpTime <= m_RedirectionTime)
        {
            m_DirectionLerpTime += Time.deltaTime;
        }

        m_Rigidbody.velocity =
            Vector3.Lerp(m_InitialDirection, playerPosDifference, m_DirectionLerpTime / m_RedirectionTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            // m_Player.GetComponent<PlayerController>().GetMinerals(m_Color);

            Destroy(gameObject);
        }
    }
}