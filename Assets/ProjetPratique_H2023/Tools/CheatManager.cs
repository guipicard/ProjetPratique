using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatManager : MonoBehaviour
{
    private bool showWindow;

    private Rect m_WinRect = new Rect(20, 20, 500, 500);

    private const int m_CrystalTypesNum = 4;

    private bool showCrystalState;
    private bool showCheatsState;

    private string[] m_CrystalTags;
    private string[] m_AiTags;

    private bool godMode;
    private bool maxDamage;


    private static CheatManager m_UniqueInstance;

    public static CheatManager Instance
    {
        get
        {
            if (m_UniqueInstance == null)
            {
                GameObject go = new GameObject("CheatManager");
                go.AddComponent<CheatManager>();
            }

            return m_UniqueInstance;
        }
    }

    private void Awake()
    {
        m_UniqueInstance = this;
    }

    private void Start()
    {
        godMode = false;
        maxDamage = false;

        showWindow = false;
        showCrystalState = false;
        showCheatsState = false;
        m_AiTags = new string[m_CrystalTypesNum] { "Green_Ai", "Red_Ai", "Yellow_Ai", "Blue_Ai" };

        m_CrystalTags = new string[m_CrystalTypesNum]
            { "Green_Crystal_Obj", "Red_Crystal_Obj", "Yellow_Crystal_Obj", "Blue_Crystal_Obj" };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3)) ToggleShowWindow();
    }

    private void OnGUI()
    {
        if (showWindow) m_WinRect = GUI.Window(0, m_WinRect, WindowFunction, "GameInfo");
    }

    void WindowFunction(int windowID)
    {
        string crystalWindowTxt =
            showCrystalState ? "\u2191 Show Crystal Infos \u2191" : "\u2193 Show Crystal Infos \u2193";
        if (GUILayout.Button(crystalWindowTxt))
        {
            ToggleshowCrystalState();
        }

        GUILayout.BeginVertical();
        if (showCrystalState)
        {
            for (int i = 0; i < m_CrystalTypesNum; i++)
            {
                GUILayout.BeginHorizontal();

                GUILayout.TextArea(
                    $"{m_CrystalTags[i]}: {LevelManager.instance.GetActiveInScene(m_CrystalTags[i]).Count}");

                GUILayout.Space(10);

                GUILayout.TextArea($"{m_AiTags[i]} {LevelManager.instance.GetActiveInScene(m_AiTags[i]).Count}");

                GUILayout.Space(20);

                GUILayout.EndHorizontal();
            }

            GUILayout.Space(20);
        }

        GUILayout.EndVertical();

        // Cheats
        string cheatsWindowTxt =
            showCheatsState ? "\u2191 Show Cheat Options \u2191" : "\u2193 Show Cheat Options \u2193";

        if (GUILayout.Button(cheatsWindowTxt))
        {
            ToggleshowCheatsState();
        }

        if (showCheatsState)
        {
            godMode = GUILayout.Toggle(godMode, "Heal");
            if (godMode)
            {
                if (LevelManager.instance.playerHp < 100.0f)
                {
                    LevelManager.instance.HealPlayer(100.0f);
                }
            }

            if (GUILayout.Button("Heal"))
            {
                LevelManager.instance.HealPlayer(100.0f);
            }

            maxDamage = GUILayout.Toggle(maxDamage, "One Shot Ennemies");
            if (maxDamage)
            {
                LevelManager.instance.SetPlayerDamage(100.0f);
            }
            else
            {
                LevelManager.instance.SetPlayerDamage(20.0f);
            }

            if (GUILayout.Button("Give Crystals"))
            {
                for (int i = 0; i < 100; i++)
                {
                    LevelManager.instance.CollectAction?.Invoke("Green");
                    LevelManager.instance.CollectAction?.Invoke("Red");
                    LevelManager.instance.CollectAction?.Invoke("Yellow");
                    LevelManager.instance.CollectAction?.Invoke("Blue");
                }
            }
        }
    }


    void ToggleShowWindow()
    {
        showWindow = !showWindow;
    }

    void ToggleshowCrystalState()
    {
        showCrystalState = !showCrystalState;
    }

    void ToggleshowCheatsState()
    {
        showCheatsState = !showCheatsState;
    }
}