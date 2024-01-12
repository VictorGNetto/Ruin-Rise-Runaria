using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemProgram : MonoBehaviour
{
    public RuneSelectionUI runeSelectionUI;
    public List<String> program;

    private int pc = 0;

    void Update()
    {
        // UI stuff
    }

    private void IncrementPC()
    {
        if (this.program.Count == 0) return;
        
        this.pc = (this.pc + 1) % this.program.Count;
    }

    public void UpdatePC()
    {
        this.IncrementPC();
    }

    public String GetCommand()
    {
        if (program.Count == 0) return new string("NoCommand");

        return this.program[this.pc];
    }

    public void LoadProgram()
    {
        program = runeSelectionUI.GetProgram();
        pc = 0;
    }
}
