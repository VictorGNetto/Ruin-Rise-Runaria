using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemProgram : MonoBehaviour
{
    public RuneSelectionUI runeSelectionUI;
    public List<String> program;
    public bool actionResult = false;

    private int pc = 0;
    private bool waitingForCondition = false;
    private bool conditionalMode = true;  // true - IF; false - ELSE

    void Update()
    {
        // UI stuff
    }

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
    }

    public String GetCommand()
    {
        if (program.Count == 0) return new string("NoCommand");

        return this.program[this.pc];
    }

    public void LoadProgram()
    {
        program = runeSelectionUI.GetProgram();
        pc = -1;
        UpdatePC();
    }
}
