using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelDirector : MonoBehaviour
{
    public List<Golem> golems;
    public List<Enemy> enemys;
    public bool levelStartedRunning = false;

    public Enemy GetRandomEnemy()
    {
        List<Enemy> validEnemys = new List<Enemy>();
        foreach (Enemy e in enemys) {
            if (e != null && !e.isDead) {
                validEnemys.Add(e);
            }
        }

        enemys = validEnemys;
        if (enemys.Count == 0) return null;

        int index = UnityEngine.Random.Range(0, enemys.Count);
        return enemys[index];
    }

    // Returns a random Golem, but the one with the passed UGID
    public Golem GetRandomFriend(int guid)
    {
        List<Golem> validGolems = new List<Golem>();
        foreach (Golem g in golems) {
            if (g != null && g.guid != guid) {
                validGolems.Add(g);
            }
        }

        if (validGolems.Count == 0) return null;

        int index = UnityEngine.Random.Range(0, validGolems.Count);
        return validGolems[index];
    }
}
