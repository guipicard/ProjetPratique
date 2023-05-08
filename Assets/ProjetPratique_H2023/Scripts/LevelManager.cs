using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    [Range(5.0f, 30.0f)]
    public float m_CrystalSpawnTimer;
    public float m_CrystalSpaceBetween;
    public int m_AiByCrystals;
    
    [Header ("Collectables")]
    public int m_GreenCollected;
    public int m_RedCollected;
    public int m_YellowCollected;
    public int m_BlueCollected;
    
    [Header("Active Crystals")]
    public int m_GreenCrystals;
    public int m_RedCrystals;
    public int m_YellowCrystals;
    public int m_BlueCrystals;

    public float playerHp;
    public float playerDamage;
    
    public Action<string> CollectAction;

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
        playerHp = 100.0f;
        playerDamage = 20.0f;
        CollectAction += CollectCrystal;
    }

    private void LoadLevel()
    {
        m_GreenCollected = 0;
        m_RedCollected = 0;
        m_YellowCollected = 0;
        m_BlueCollected = 0;
    }

    public void SpawnObj(string _tag, Vector3 _position, Quaternion _rotation)
    {
        GameObject obj = m_Pools.GetObj(_tag);
        obj.transform.position = _position;
        obj.transform.rotation = _rotation;
        ToggleActive(obj);
    }

    public List<GameObject> GetActiveInScene(string _tag)
    {
        return m_Pools.GetActive(_tag);
    }

    public void CollectCrystal(string _color)
    {
        switch (_color)
        {
            case "Green":
                m_GreenCollected++; 
                break;
            case "Red":
                m_RedCollected++;
                break;
            case "Yellow":
                m_YellowCollected++;
                break;
            case "Blue":
                m_BlueCollected++;
                break;
        }
    }

    public int GetCollected(string _color)
    {
        switch (_color)
        {
            case "Green":
                return m_GreenCollected;
            case "Red":
                return m_RedCollected;
            case "Yellow":
                return m_YellowCollected;
            case "Blue":
                return m_BlueCollected;
        }
        return 0;
    }
    
    public void ToggleActive(GameObject _obj)
    {
        _obj.SetActive(true);
        var parent = _obj.transform.parent;
        _obj.transform.parent = parent.parent;
    }
    
    public void ToggleInactive(GameObject _obj)
    {
        _obj.SetActive(false);
        _obj.transform.parent = _obj.transform.parent.Find("Inactive");
    }

    public int UpdateCrystalNums(string _color)
    {
        switch (_color)
        {
            case "Green_Crystal_Obj":
                m_GreenCrystals = GetActiveInScene(_color).Count;
                return m_GreenCrystals;
            case "Red_Crystal_Obj":
                m_RedCrystals = GetActiveInScene(_color).Count;
                return m_RedCrystals;
            case "Yellow_Crystal_Obj":
                m_YellowCrystals = GetActiveInScene(_color).Count;
                return m_YellowCrystals;
            case "Blue_Crystal_Obj":
                m_BlueCrystals = GetActiveInScene(_color).Count;
                return m_BlueCrystals;
        }

        return 0;
    }

    public void HealPlayer(float _hp)
    {
        playerHp += _hp;
        if (playerHp >= 100.0f) playerHp = 100.0f;
    }

    public void SetPlayerDamage(float _dmg)
    {
        playerDamage = _dmg;
    }
}
