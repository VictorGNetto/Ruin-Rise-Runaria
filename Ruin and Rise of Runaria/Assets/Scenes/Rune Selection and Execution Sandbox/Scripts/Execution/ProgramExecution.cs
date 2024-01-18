using System;
using UnityEngine;
using UnityEngine.UI;

public class ProgramExecution : MonoBehaviour
{
    public RuneSelectionUI runeSelection;
    public GolemProgram golemProgram;
    public Golem golem;

    private GameObject cooldownBar;
    private GameObject cooldownValue;
    private GameObject currentRune;

    private void Awake()
    {
        cooldownBar = transform.GetChild(2).gameObject;
        cooldownValue = transform.GetChild(3).transform.GetChild(0).gameObject;
        currentRune = transform.GetChild(4).transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        float cooldownBarValue = 0.0f;
        String cooldownValueText = "";
        if (golem.cooldown > 0.001f) {
            cooldownBarValue = golem.timeSinceLastAction / golem.cooldown;
            cooldownValueText = String.Format("{0:0.0}/{1:0.0} (s)", golem.timeSinceLastAction, golem.cooldown);
        }
        cooldownBar.GetComponent<Slider>().value = cooldownBarValue;
        cooldownValue.GetComponent<Text>().text = cooldownValueText;
        currentRune.GetComponent<Image>().sprite = golemProgram.GetCurrentRuneSprite();
    }

    public void OpenRuneSelectionUI()
    {
        runeSelection.OpenRuneSelectionUI();
    }
}
