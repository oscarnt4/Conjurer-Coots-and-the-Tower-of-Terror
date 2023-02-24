using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI promptText;
    [SerializeField] Slider towerHeightSlider;
    [SerializeField] Slider manaSlider;
    [SerializeField] TextMeshProUGUI manaText;

    void Start()
    {

    }

    public void UpdateText(string text)
    {
        promptText.text = text;
    }

    public void UpdateHeight(float heightRatio)
    {
        towerHeightSlider.value = heightRatio;
    }

    public void UpdateMana(int currentMana)
    {
        manaSlider.value = currentMana;
        manaText.text = currentMana.ToString() + "/100";
    }
}
