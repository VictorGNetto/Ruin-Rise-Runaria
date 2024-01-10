using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemProgram : MonoBehaviour
{
    public List<String> program;
    private int pc = 0;

    void Update()
    {
        // UI stuff
    }

    private void IncrementPC()
    {
        this.pc = (this.pc + 1) % this.program.Count;
    }

    public void UpdatePC()
    {
        this.IncrementPC();
    }

    public String GetCommand()
    {
        return this.program[this.pc];
    }
}
