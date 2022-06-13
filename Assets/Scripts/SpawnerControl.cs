using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerControl : MonoBehaviour
{
    public GameObject monsterPrefab;
    public int spawning_number = 1;
    private bool spawning = false;
    public int max_monsters = 50;

    private void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        // Debug.Log(enemies.Length);
        if (enemies.Length <= 0 && !spawning)
        {
            spawning = true;
            for (int i = 0; i < spawning_number; i++)
            {
                Invoke(nameof(AddMonster), .5f);
                spawning_number++;
            }
            spawning = false;
        }

    }

    private void AddMonster()
    {
        var monster = Instantiate(monsterPrefab, transform);
    }
}
