using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiComponents : MonoBehaviour
{
    
    [SerializeField] private TextMeshPro m_BlueCrystalsText;
    [SerializeField] private TextMeshPro m_RedCrystalsText;
    [SerializeField] private TextMeshPro m_YellowCrystalsText;
    [SerializeField] private TextMeshPro m_GreenCrystalsText;

    [SerializeField] private Image m_BlueCrystalImage;
    [SerializeField] private Image m_RedCrystalImage;
    [SerializeField] private Image m_YellowCrystalImage;
    [SerializeField] private Image m_GreenCrystalImage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
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
        if (LevelManager.instance.m_GreenCollected > 100)
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
        
        if (LevelManager.instance.m_RedCollected > 100)
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
        
        if (LevelManager.instance.m_YellowCollected > 100)
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
        
        if (LevelManager.instance.m_BlueCollected > 100)
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
}
