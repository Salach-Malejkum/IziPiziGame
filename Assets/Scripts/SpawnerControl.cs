using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerControl : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, COUNTING };
    [System.Serializable]
    public class Wave
    {
        public int number;
        public Transform enemy;
        public int spawning_number;
    }

    public Wave[] waves;
    public int nextWave = 0;
    public SpawnState state = SpawnState.COUNTING;

    public float spawnDelay = 5f;
    public float countdown;
    private float searchDelay = 0f;
    private float monsterDelay = .2f;

    private void Start()
    {
        countdown = spawnDelay;
    }

    private void Update()
    {
        if (state == SpawnState.WAITING)
        {
            if (!CheckIfEnemiesAlive())
            {
                WaveCompleted();
            }
            else return;
        }

        if (countdown <= 0 && state == SpawnState.COUNTING) StartCoroutine(SpawnWave(waves[nextWave]));
        else if (countdown > 0) countdown -= Time.deltaTime;
    }

    void WaveCompleted()
    {
        Debug.Log("Wave completed");

        state = SpawnState.COUNTING;
        countdown = spawnDelay;

        if (nextWave+1 > waves.Length-1)
        { // doda� tutaj co si� b�dzie dzia�o jak sko�cz� si� fale
            nextWave = 0;
            Debug.Log("All waves completed! Looping back in time, good luck"); 
        }
        nextWave++;
    }

    bool CheckIfEnemiesAlive()
    {
        searchDelay -= Time.deltaTime;
        if (searchDelay <= 0)
        {
            searchDelay = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null) return false;
        }
        
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("Spawn wave");
        state = SpawnState.SPAWNING;

        for (int i = 0; i < _wave.spawning_number; i++)
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(monsterDelay);
        }

        state = SpawnState.WAITING;

        yield break;
    }

    void SpawnEnemy(Transform _enemy)
    {
        Debug.Log("Spawn Enemy");
        Instantiate(_enemy, transform.position, transform.rotation);
    }

    public int GetWave()
    {
        return waves[nextWave].number;
    }
}
