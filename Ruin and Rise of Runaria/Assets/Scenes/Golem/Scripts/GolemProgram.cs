using System;
using System.Collections.Generic;
using UnityEngine;

public class GolemProgram : MonoBehaviour
{
    public RuneSelectionUI runeSelectionUI;
    public List<String> program;
    private Dictionary<String, Sprite> runeSprites = new Dictionary<string, Sprite>();
    public bool actionResult = false;

    private int pc = 0;
    private Sprite sprite;
    private bool waitingForCondition = false;
    private bool conditionalMode = true;  // true - IF; false - ELSE

    private void IncrementPC()
    {
        pc = (pc + 1) % program.Count;
    }

    public void UpdatePC()
    {
        if (program.Count == 0) return;

        if (waitingForCondition) {
            waitingForCondition = false;
            // Jumps over action (do an additional increment) when
            // actionResult returns false in a IF (true)
            // actionResult return true in a ELSE (false)
            if (actionResult != conditionalMode) {
                IncrementPC();
            }
        }

        IncrementPC();

        if (program[pc].Equals("IF")) {
            waitingForCondition = true;
            conditionalMode = true;
            IncrementPC();
        } else if (program[pc].Equals("ELSE")) {
            waitingForCondition = true;
            conditionalMode = false;
            IncrementPC();
        }

        // Remove this when enemys are not golems anymore
        try
        {
            sprite = runeSprites[program[pc]];
            
        }
        catch (System.Exception)
        {
        }
    }

    public String GetCommand()
    {
        if (program.Count == 0) return new string("NoCommand");

        return program[pc];
    }

    public Sprite GetCurrentRuneSprite()
    {
        return sprite;
    }

    public void LoadProgram()
    {
        program = runeSelectionUI.GetProgram();
        runeSprites = runeSelectionUI.GetRuneSprites();
        pc = -1;
        UpdatePC();
    }
}
