using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelDirector : MonoBehaviour
{
    public List<Golem> golems;
    public List<Enemy> enemys;
    public bool levelStartedRunning = false;

    private void Awake()
    {
        int i = 0;
        foreach (Golem g in golems) {
            if (g != null) {
                g.guid = i;
            }
            i++;
        }

        i = 100;
        foreach (Enemy e in enemys) {
            if (e != null) {
                e.guid = i;
            }
            i++;
        }
    }

    // Returns a random Golem, but the one with the passed UGID
    public Golem GetRandomFriend()
    {
        List<Golem> validGolems = new List<Golem>();
        foreach (Golem g in golems) {
            if (g != null) {
                validGolems.Add(g);
            }
        }

        if (validGolems.Count == 0) return null;

        int index = UnityEngine.Random.Range(0, validGolems.Count);
        return validGolems[index];
    }

    // Returns a random Golem, but the one with the passed UGID
    public Golem GetRandomAliveFriend()
    {
        List<Golem> validGolems = new List<Golem>();
        foreach (Golem g in golems) {
            if (g != null && g.Alive()) {
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
            if (g != null && g.Alive() && g.guid != guid) {
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

    public Golem GetGolemWithHighestHealth()
    {
        List<Golem> validGolems = new List<Golem>();
        foreach (Golem g in golems) {
            if (g != null && g.Alive()) {
                validGolems.Add(g);
            }
        }

        Golem golemWithHighestHealth = validGolems[0];
        foreach (Golem g in validGolems) {
            if (g.health > golemWithHighestHealth.health) {
                golemWithHighestHealth = g;
            }
        }

        return golemWithHighestHealth;
    }

    public Golem GetGolemWithLowestHealth()
    {
        List<Golem> validGolems = new List<Golem>();
        foreach (Golem g in golems) {
            if (g != null && g.Alive()) {
                validGolems.Add(g);
            }
        }

        Golem golemWithLowestHealth = validGolems[0];
        foreach (Golem g in validGolems) {
            if (g.health < golemWithLowestHealth.health) {
                golemWithLowestHealth = g;
            }
        }

        return golemWithLowestHealth;
    }

    public Golem GetNearestGolem(int guid)
    {
        List<Golem> validGolems = new List<Golem>();
        foreach (Golem g in golems) {
            if (g != null && g.Alive() && g.guid != guid) {
                validGolems.Add(g);
            }
        }

        if (validGolems.Count == 0) return golems[guid];

        Golem nearestGolem = validGolems[0];
        float distance = (golems[guid].transform.position - nearestGolem.transform.position).magnitude;
        foreach (Golem g in validGolems) {
            float newDistance = (golems[guid].transform.position - g.transform.position).magnitude;
            if (newDistance < distance) {
                distance = newDistance;
                nearestGolem = g;
            }
        }

        return nearestGolem;
    }

    public Golem GetFartestGolem(int guid)
    {
        List<Golem> validGolems = new List<Golem>();
        foreach (Golem g in golems) {
            if (g != null && g.Alive() && g.guid != guid) {
                validGolems.Add(g);
            }
        }

        if (validGolems.Count == 0) return golems[guid];

        Golem fartestGolem = validGolems[0];
        float distance = (golems[guid].transform.position - fartestGolem.transform.position).magnitude;
        foreach (Golem g in validGolems) {
            float newDistance = (golems[guid].transform.position - g.transform.position).magnitude;
            if (newDistance > distance) {
                distance = newDistance;
                fartestGolem = g;
            }
        }

        return fartestGolem;
    }

    // Enemys
    public Enemy GetRandomEnemy()
    {
        List<Enemy> validEnemys = new List<Enemy>();
        foreach (Enemy e in enemys) {
            if (e != null) {
                validEnemys.Add(e);
            }
        }

        if (validEnemys.Count == 0) return null;

        int index = UnityEngine.Random.Range(0, validEnemys.Count);
        return validEnemys[index];
    }

    public Enemy GetRandomAliveEnemy()
    {
        List<Enemy> validEnemys = new List<Enemy>();
        foreach (Enemy e in enemys) {
            if (e != null && e.Alive()) {
                validEnemys.Add(e);
            }
        }

        if (validEnemys.Count == 0) return null;

        int index = UnityEngine.Random.Range(0, validEnemys.Count);
        return validEnemys[index];
    }

    public Vector3 GetEnemysCentroid(int guid)
    {
        List<Enemy> validEnemys = new List<Enemy>();
        foreach (Enemy e in enemys) {
            if (e != null && e.Alive()) {
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

    public Enemy GetEnemyWithHighestHealth()
    {
        List<Enemy> validEnemys = new List<Enemy>();
        foreach (Enemy e in enemys) {
            if (e != null && e.Alive()) {
                validEnemys.Add(e);
            }
        }

        if (validEnemys.Count == 0) return null;

        Enemy enemyWithHighestHealth = validEnemys[0];
        foreach (Enemy e in validEnemys) {
            if (e.health > enemyWithHighestHealth.health) {
                enemyWithHighestHealth = e;
            }
        }

        return enemyWithHighestHealth;
    }

    public Enemy GetEnemyWithLowestHealth()
    {
        List<Enemy> validEnemys = new List<Enemy>();
        foreach (Enemy e in enemys) {
            if (e != null && e.Alive()) {
                validEnemys.Add(e);
            }
        }

        if (validEnemys.Count == 0) return null;

        Enemy enemyWithLowestHealth = validEnemys[0];
        foreach (Enemy e in validEnemys) {
            if (e.health < enemyWithLowestHealth.health) {
                enemyWithLowestHealth = e;
            }
        }

        return enemyWithLowestHealth;
    }

    public Enemy GetNearestEnemy(int guid)
    {
        List<Enemy> validEnemys = new List<Enemy>();
        foreach (Enemy e in enemys) {
            if (e != null && e.Alive()) {
                validEnemys.Add(e);
            }
        }

        if (validEnemys.Count == 0) return null;

        Enemy nearestEnemy = validEnemys[0];
        float distance = (golems[guid].Position() - nearestEnemy.Position()).magnitude;
        foreach (Enemy e in validEnemys) {
            float newDistance = (golems[guid].Position() - e.Position()).magnitude;
            if (newDistance < distance) {
                distance = newDistance;
                nearestEnemy = e;
            }
        }

        return nearestEnemy;
    }

    public Enemy GetFartestEnemy(int guid)
    {
        List<Enemy> validEnemys = new List<Enemy>();
        foreach (Enemy e in enemys) {
            if (e != null && e.Alive()) {
                validEnemys.Add(e);
            }
        }

        if (validEnemys.Count == 0) return null;

        Enemy fartestEnemy = validEnemys[0];
        float distance = (golems[guid].transform.position - fartestEnemy.transform.position).magnitude;
        foreach (Enemy e in validEnemys) {
            float newDistance = (golems[guid].Position() - e.Position()).magnitude;
            if (newDistance > distance) {
                distance = newDistance;
                fartestEnemy = e;
            }
        }

        return fartestEnemy;
    }
}
