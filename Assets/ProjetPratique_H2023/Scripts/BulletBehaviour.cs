using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] private float m_Speed;
    [SerializeField] private float m_MaxDistance;
    [SerializeField] private string m_DamageTag;
    
    private float m_SpeedMultiplier;
    private Vector3 m_InitialPosition;
    private Vector3 m_DistanceDone;
    
    // Start is called before the first frame update
    void Start()
    {
        m_InitialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        m_SpeedMultiplier = m_Speed * Time.deltaTime;
        transform.Translate(Vector3.forward * m_SpeedMultiplier);
        if (Vector3.Distance(transform.position, m_InitialPosition) > m_MaxDistance)
        {
            Destroy(gameObject);
        }
    }
}
