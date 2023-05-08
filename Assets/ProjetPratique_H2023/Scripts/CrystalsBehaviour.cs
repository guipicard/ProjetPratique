using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CrystalsBehaviour : MonoBehaviour
{
    [SerializeField] private string m_CrystalName;
    [SerializeField] private string m_CrystalTag;
    [SerializeField] private string m_AiTag;
    [SerializeField] private Vector3 m_InitialPosition;

    public HashSet<Vector2> m_CrystalsPosition;
    public HashSet<Vector2> m_PotentialPosition;
    public List<Vector2> m_LastCrystalWave;

    private float m_Elapsed;
    private Ray m_Ray;
    private RaycastHit m_HitInfo;

    public int m_AiActive;
    public int m_CrystalActive;
    private float m_CrystalHeight;
    private float CrystalSpacing;
    private Vector2[] m_SurroundOffsets;


    void Start()
    {
        CrystalSpacing = LevelManager.instance.m_CrystalSpaceBetween;
        m_CrystalHeight = m_InitialPosition.y;
        m_SurroundOffsets = new Vector2[4]
        {
            new Vector2(CrystalSpacing, CrystalSpacing),
            new Vector2(-CrystalSpacing, CrystalSpacing),
            new Vector2(CrystalSpacing, -CrystalSpacing),
            new Vector2(-CrystalSpacing, -CrystalSpacing)
        };

        m_Ray = new Ray();
        m_Ray.direction = Vector3.down;

        LevelManager.instance.SpawnObj(m_CrystalTag, m_InitialPosition, Quaternion.identity);

        m_AiActive = 0;
        m_CrystalActive = 1;
        m_Elapsed = 0;

        m_PotentialPosition = new HashSet<Vector2>();
        m_CrystalsPosition = new HashSet<Vector2>();
        // Add All Present Crystals Positions in List "CrystalsPosition"
        FillCrystalList();
        foreach (var pos in m_CrystalsPosition)
        {
            m_LastCrystalWave.Add(pos);
        }

        SpawnAi();
    }

    void Update()
    {
        CrystalDuplicationLoop();
    }

    private void CrystalDuplicationLoop()
    {
        m_Elapsed += Time.deltaTime;
        if (m_Elapsed > LevelManager.instance.m_CrystalSpawnTimer)
        {
            m_AiActive = LevelManager.instance.GetActiveInScene(m_AiTag).Count;
            m_CrystalActive = LevelManager.instance.UpdateCrystalNums(m_CrystalTag);
            FillCrystalList();
            GetNewPositions();
            Multiply();
            m_CrystalActive = LevelManager.instance.UpdateCrystalNums(m_CrystalTag);
            if (m_AiActive < 1 || m_AiActive + 1 <= (m_CrystalActive / LevelManager.instance.m_AiByCrystals) + 1)
            {
                SpawnAi();
            }


            // Reset Lists
            m_PotentialPosition.Clear();
            m_CrystalsPosition.Clear();

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

        foreach (var pos in m_CrystalsPosition)
        {
            for (int j = m_SurroundOffsets.Length - 1; j >= 0; j--)
            {
                Vector2 m_currentPosition = m_SurroundOffsets[j] + pos;

                m_Ray.origin = new Vector3(m_currentPosition.x, m_CrystalHeight + 2.0f, m_currentPosition.y);
                if (Physics.Raycast(m_Ray, out m_HitInfo, Mathf.Infinity))
                {
                    m_PotentialPosition.Add(m_currentPosition);
                }
            }
        }
    }

    private void Multiply()
    {
        // Create New Crystals
        HashSet<Vector2> newWave = new HashSet<Vector2>();
        foreach (Vector2 pos in m_PotentialPosition)
        {
            var chances = Random.Range(0, 5);

            m_Ray = new Ray(new Vector3(pos.x, m_CrystalHeight + 2.0f, pos.y), Vector3.down);
            var myRaycast = Physics.Raycast(m_Ray, out m_HitInfo, Mathf.Infinity);
            if (chances == 1 && myRaycast)
            {
                if (m_HitInfo.collider.gameObject.layer == 6)
                {
                    var hitObj = m_HitInfo.collider.transform.parent.parent.gameObject;
                    int hitActiveInSceneCount = hitObj.GetComponent<CrystalsBehaviour>().m_CrystalActive;
                    var hasMoreCrystals = m_CrystalActive > hitActiveInSceneCount;
                    if (hasMoreCrystals)
                    {
                        LevelManager.instance.ToggleInactive(m_HitInfo.collider.gameObject);
                        newWave.Add(pos);
                        Vector3 newPos = new Vector3(pos.x, m_CrystalHeight, pos.y);
                        LevelManager.instance.SpawnObj(m_CrystalTag, newPos, Quaternion.identity);
                    }

                    continue;
                }

                newWave.Add(pos);
                var newCrystalPosition = new Vector3(pos.x, m_CrystalHeight, pos.y);
                LevelManager.instance.SpawnObj(m_CrystalTag, newCrystalPosition, Quaternion.identity);
            }
        }

        if (newWave.Count > 0)
        {
            m_LastCrystalWave = newWave.ToList();
        }
    }

    private void SpawnAi()
    {
        int spawnPointCrystalIndex = m_LastCrystalWave.Count == 1 ? 0 : Random.Range(0, m_LastCrystalWave.Count);
        Vector2 spawnPointCrystal = m_LastCrystalWave[spawnPointCrystalIndex];
        Vector2 spawnPointOffset = new Vector2(m_InitialPosition.x - spawnPointCrystal.y, m_InitialPosition.z - spawnPointCrystal.y).normalized;
        Vector2 spawnPointAi = spawnPointCrystal - spawnPointOffset;
        Vector3 newAiPosition = new Vector3(spawnPointAi.x, m_CrystalHeight, spawnPointAi.y);
        LevelManager.instance.SpawnObj(m_AiTag, newAiPosition, Quaternion.identity);
    }
}
