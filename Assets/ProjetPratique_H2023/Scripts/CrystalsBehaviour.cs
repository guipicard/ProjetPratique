using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CrystalsBehaviour : MonoBehaviour
{
    [SerializeField] private CrystalData m_CrystalsData;
    [SerializeField] private string m_CrystalName;
    
    [SerializeField] private string m_CrystalTag;
    [SerializeField] private string m_AiTag;
    
    private CrystalData.CrystalType m_CrystalType;

    [SerializeField] private Vector3 m_InitialPosition;

    public List<Vector2> m_CrystalsPosition;
    public List<Vector2> m_PotentialPosition;
    public List<Vector2> m_LastCrystalWave;

    private float m_Elapsed;
    private Ray m_Ray;
    private RaycastHit m_HitInfo;
    public int m_AiAlive;


    void Start()
    {
        LevelManager.instance.SpawnObj(m_CrystalTag, m_InitialPosition, Quaternion.identity);
        foreach (var type in m_CrystalsData.crystalTypes)
        {
            if (type.name == m_CrystalName)
            {
                m_CrystalType = type;
            }
        }

        m_AiAlive = 0;
        m_Elapsed = 0;

        m_PotentialPosition = new List<Vector2>();
        m_CrystalsPosition = new List<Vector2>();
        // Add All Present Crystals Positions in List "CrystalsPosition"
        FillCrystalList();
        m_LastCrystalWave = m_CrystalsPosition;
        SpawnAi();
    }

    void Update()
    {
        CrystalDuplicationLoop();
    }

    private void CrystalDuplicationLoop()
    {
        m_Elapsed += Time.deltaTime;
        if (m_Elapsed > m_CrystalType.spawnTimer)
        {
            m_AiAlive = LevelManager.instance.GetActiveInScene(m_AiTag).Count;
            FillCrystalList();
            GetNewPositions();
            Multiply();
            if (m_AiAlive < (m_CrystalsPosition.Count / m_CrystalType.aiByCrystals) + 1 &&
                m_CrystalsPosition.Count > m_CrystalType.aiByCrystals)
            {
                SpawnAi();
            }
            
            // Reset Lists
            m_PotentialPosition = new List<Vector2>();
            m_CrystalsPosition = new List<Vector2>();
            m_LastCrystalWave = new List<Vector2>();
            
            m_Elapsed = 0;
        }
        
    }

    private void FillCrystalList()
    {
        foreach (GameObject crystal in LevelManager.instance.GetActiveInScene(m_CrystalTag))
        {
            m_CrystalsPosition.Add(new Vector2(crystal.transform.position.x, crystal.transform.position.z));
        }
    }

    private void GetNewPositions()
    {
        // Add All Potential Places a new Crystal could be
        for (int i = m_CrystalsPosition.Count - 1; i >= 0; i--)
        {
            Vector2 currentCrystal = m_CrystalsPosition[i];
            float CrystalSpacing = m_CrystalsData.spaceBetween;
            Vector2[] surroundingPlacings =
            {
                currentCrystal + new Vector2(m_CrystalsData.spaceBetween, m_CrystalsData.spaceBetween),
                currentCrystal + new Vector2(-m_CrystalsData.spaceBetween, m_CrystalsData.spaceBetween),
                currentCrystal + new Vector2(m_CrystalsData.spaceBetween, -m_CrystalsData.spaceBetween),
                currentCrystal + new Vector2(-m_CrystalsData.spaceBetween, -m_CrystalsData.spaceBetween)
            };

            for (int j = surroundingPlacings.Length - 1; j >= 0; j--)
            {
                if (!m_PotentialPosition.Contains(surroundingPlacings[j]))
                {
                    m_Ray = new Ray(
                        new Vector3(surroundingPlacings[j].x, m_CrystalsData.crystalHeight + 2.0f,
                            surroundingPlacings[j].y), Vector3.down);
                    if (Physics.Raycast(m_Ray, out m_HitInfo, Mathf.Infinity))
                    {
                        if (m_HitInfo.collider.CompareTag("Ground") || m_HitInfo.collider.gameObject.layer == 6)
                        {
                            m_PotentialPosition.Add(surroundingPlacings[j]);
                        }
                    }
                }
            }
        }
    }

    private void Multiply()
    {
        // Create New Crystals
        foreach (Vector2 pos in m_PotentialPosition)
        {
            int chances = Random.Range(0, 5);

            m_Ray = new Ray(new Vector3(pos.x, m_CrystalsData.crystalHeight + 2.0f, pos.y), Vector3.down);
            bool myRaycast = Physics.Raycast(m_Ray, out m_HitInfo, Mathf.Infinity);
            if (chances == 1 && myRaycast)
            {
                if (m_HitInfo.collider.gameObject.layer == 6)
                {
                    m_HitInfo.collider.gameObject.SetActive(false);
                }
                m_LastCrystalWave.Add(pos);
                Vector3 newCrystalPosition = new Vector3(pos.x, m_CrystalsData.crystalHeight, pos.y);
                LevelManager.instance.SpawnObj(m_CrystalTag, newCrystalPosition, Quaternion.identity);
                // Instantiate(m_CrystalType.crystalMineral, newCrystalPosition, Quaternion.identity, transform);
            }
        }
    }
    
    private void SpawnAi()
    {
        int spawnPointCrystalIndex = m_LastCrystalWave.Count() == 1 ? 0 : Random.Range(0, m_LastCrystalWave.Count);
        Vector2 spawnPointCrystal = m_LastCrystalWave[spawnPointCrystalIndex];
        Vector3 newAiPosition = new Vector3(spawnPointCrystal.x, m_CrystalsData.crystalHeight, spawnPointCrystal.y);
        // Instantiate(m_CrystalType.aiPrefab, newAiPosition, Quaternion.identity, transform);
        LevelManager.instance.SpawnObj(m_AiTag, newAiPosition, Quaternion.identity);
    }
}
