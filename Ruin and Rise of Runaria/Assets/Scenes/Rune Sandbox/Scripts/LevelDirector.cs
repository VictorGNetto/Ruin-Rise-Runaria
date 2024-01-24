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

    public Vector3 GetGolemsCentroid(int guid)
    {
        List<Golem> validGolems = new List<Golem>();
        foreach (Golem g in golems) {
            if (g != null && g.guid != guid) {
                validGolems.Add(g);
            }
        }

        if (validGolems.Count == 0) return golems[guid].transform.position;

        float x = 0;
        float y = 0;
        foreach (Golem g in validGolems) {
            x += g.transform.position.x;
            y += g.transform.position.y;
        }
        x = x / validGolems.Count;
        y = y / validGolems.Count;

        return new Vector3(x, y, 0);
    }

    public Vector3 GetEnemysCentroid(int guid)
    {
        List<Enemy> validEnemys = new List<Enemy>();
        foreach (Enemy e in enemys) {
            if (e != null && !e.isDead) {
                validEnemys.Add(e);
            }
        }

        if (validEnemys.Count == 0) return golems[guid].transform.position;

        float x = 0;
        float y = 0;
        foreach (Enemy e in validEnemys) {
            x += e.transform.position.x;
            y += e.transform.position.y;
        }
        x = x / validEnemys.Count;
        y = y / validEnemys.Count;

        return new Vector3(x, y, 0);
    }
}
