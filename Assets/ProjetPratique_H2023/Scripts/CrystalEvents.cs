using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalEvents : MonoBehaviour
{
    [SerializeField] private string m_CrystalTag;
    [SerializeField] private string m_PartsTag;

    private CrystalsBehaviour m_CrystalsBehaviour;
    
    void Start()
    {
        m_CrystalsBehaviour = transform.parent.GetComponent<CrystalsBehaviour>();
    }

    public void GetMined()
    {
        LevelManager.instance.SpawnObj(m_PartsTag, transform.position, Quaternion.identity);
        LevelManager.instance.ToggleInactive(gameObject);
        LevelManager.instance.UpdateCrystalNums(m_CrystalTag);
    }
}
