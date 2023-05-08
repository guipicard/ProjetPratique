using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiComponents : MonoBehaviour
{
    
    [SerializeField] private TextMeshPro m_GreenCrystalsText;
    [SerializeField] private TextMeshPro m_RedCrystalsText;
    [SerializeField] private TextMeshPro m_YellowCrystalsText;
    [SerializeField] private TextMeshPro m_BlueCrystalsText;

    [SerializeField] private Image m_GreenCrystalImage;
    [SerializeField] private Image m_RedCrystalImage;
    [SerializeField] private Image m_YellowCrystalImage;
    [SerializeField] private Image m_BlueCrystalImage;

    void Start()
    {
        LevelManager.instance.CollectAction += UpdateUi;
        ChangeAlpha(m_GreenCrystalImage, 0.2f);
        ChangeAlpha(m_RedCrystalImage, 0.2f);
        ChangeAlpha(m_YellowCrystalImage, 0.2f);
        ChangeAlpha(m_BlueCrystalImage, 0.2f);
    }

    void Update()
    {
      
    }

    public void UpdateUi(string color)
    {
        int crystalAmount = LevelManager.instance.GetCollected(color);
        switch (color)
        {
            case "Green":
                m_GreenCrystalsText.text = crystalAmount.ToString();
                ChangeAlpha(m_GreenCrystalImage, crystalAmount >= 100 ? 1.0f: 0.2f);
                break;
            case "Red":
                m_RedCrystalsText.text = crystalAmount.ToString();
                ChangeAlpha(m_RedCrystalImage, crystalAmount >= 100 ? 1.0f: 0.2f);
                break;
            case "Yellow":
                m_YellowCrystalsText.text = crystalAmount.ToString();
                ChangeAlpha(m_YellowCrystalImage, crystalAmount >= 100 ? 1.0f: 0.2f);
                break;
            case "Blue":
                m_BlueCrystalsText.text = crystalAmount.ToString();
                ChangeAlpha(m_BlueCrystalImage, crystalAmount >= 100 ? 1.0f: 0.2f);
                break;
        }
    }

    private void ChangeAlpha(Image img, float alpha)
    {
        Color currentColor = img.color;
        currentColor.a = alpha;
        img.color = currentColor;
    }
}
