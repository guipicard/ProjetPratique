using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CrystalData : ScriptableObject
{
    public float spaceBetween;
    public float crystalHeight;

    [System.Serializable]
    public struct CrystalType
    {
        public string name;
        public GameObject crystalMineral;
        public GameObject crystalParts;
        public float spawnTimer;
        public GameObject aiPrefab;
        public int aiByCrystals;
        public int chancesToSpawn;
    }
    public List<CrystalType> crystalTypes = new List<CrystalType>();
}
