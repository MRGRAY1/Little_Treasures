using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameHudUI : MonoBehaviour
{
    [Header("UI Elements")] [SerializeField]
    private UIDocument gameHud_UIDocument;

    private VisualElement RedGems_Container;
    private VisualElement BlueGems_Container;
    private VisualElement GreenGems_Container;

    [SerializeField] private VisualElement Coins_Container;

    private int TestInt = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var root = gameHud_UIDocument.rootVisualElement;
        RedGems_Container = root.Q<VisualElement>("Red_Gem_Container");
        BlueGems_Container = root.Q<VisualElement>("Blue_Gem_Container");
        GreenGems_Container = root.Q<VisualElement>("Green_Gem_Container");
        Coins_Container = root.Q<VisualElement>("Coins_Container");
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void LateUpdate()
    {
        UpdateLabels();
    }

    private void UpdateLabels()
    {
        TestInt++;
        RedGems_Container.Q<Label>("Gem_Lbl").text = "TestRed: " + TestInt;
        BlueGems_Container.Q<Label>("Gem_Lbl").text = "TestBlue: " + TestInt;
        GreenGems_Container.Q<Label>("Gem_Lbl").text = "TestGreen: " + TestInt;
        Coins_Container.Q<Label>("Coins_Lbl").text = "TestCoin: " + TestInt;
    }
}