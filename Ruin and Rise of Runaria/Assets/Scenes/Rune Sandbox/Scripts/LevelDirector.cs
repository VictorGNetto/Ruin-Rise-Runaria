using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDirector : MonoBehaviour
{
    public List<Golem> golems;
    public List<Golem> enemys;

    public Golem GetEnemy()
    {
        return enemys[0];
    }
}
