using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private int m_GreenCollected;
    private int m_RedCollected;
    private int m_YellowCollected;
    private int m_BlueCollected;

    [SerializeField] private Transform m_Player;
    [SerializeField] private ObjPool m_Pools;
    
    private static LevelManager levelManager;

    public static LevelManager instance
    {
        get
        {
            if (!levelManager)
            {
                levelManager = FindObjectOfType(typeof(LevelManager)) as LevelManager;

                if (!levelManager)
                {
                    Debug.LogError("There needs to be one active EventManager script on a GameObject in your scene.");
                }
                else
                {
                    levelManager.LoadLevel();
                    DontDestroyOnLoad(levelManager);
                }
            }

            return levelManager;
        }
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadLevel()
    {
        m_GreenCollected = 0;
        m_RedCollected = 0;
        m_YellowCollected = 0;
        m_BlueCollected = 0;
    }

    public void SpawnObj(string tag, Vector3 position, Quaternion rotation)
    {
        GameObject obj = m_Pools.GetObj(tag);
        obj.SetActive(true);
        obj.transform.position = position;
        obj.transform.rotation = rotation;
    }

    public List<GameObject> GetActiveInScene(string tag)
    {
        return m_Pools.GetActive(tag);
    }

    public Vector3 GetPlayerPosition()
    {
        return m_Player.transform.position;
    }
}
