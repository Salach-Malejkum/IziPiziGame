using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnerControl : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, COUNTING };
    [System.Serializable]
    public class Wave
    {
        public int number;
        public int spawning_number;
    }

    [SerializeField]
    public Transform[] monstersWithDiff;

    public Wave[] waves;
    public int nextWave = 0;
    public SpawnState state = SpawnState.COUNTING;
    public GameObject endScreen;
    public GameObject playerUI;

    public float spawnDelay = 5f;
    public float countdown;
    private float searchDelay = 0f;
    private float monsterDelay = .2f;

    public Transform door;
    private Animator doorAnimator;
    public Transform enemy;

    private void Start()
    {
        countdown = spawnDelay;
        endScreen.SetActive(false);
        doorAnimator = door.GetComponent<Animator>();
        string difficultyLevel = PlayerPrefs.GetString("difficultyLevel");
        switch (difficultyLevel) {
            case "Easy":
                enemy = monstersWithDiff[0];
                break;
            case "Medium":
                enemy = monstersWithDiff[1];
                break;
            case "Hard":
                enemy = monstersWithDiff[2];
                break;
            default:
                enemy = monstersWithDiff[0];
                break;
        }
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
        // Debug.Log("Wave completed");

        state = SpawnState.COUNTING;
        countdown = spawnDelay;

        if (nextWave+1 > waves.Length-1)
        { // doda� tutaj co si� b�dzie dzia�o jak sko�cz� si� fale
            playerUI.SetActive(false);
            endScreen.SetActive(true);
            nextWave = 0;
            StartCoroutine(EndGame());
        }
        nextWave++;
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene("MainMenu"); 
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
        // Debug.Log("Spawn wave");
        state = SpawnState.SPAWNING;
        doorAnimator.SetBool("Open", true);

        for (int i = 0; i < _wave.spawning_number; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(monsterDelay);
        }
        doorAnimator.SetBool("Open", false);

        state = SpawnState.WAITING;

        yield break;
    }

    void SpawnEnemy()
    {
        // Debug.Log("Spawn Enemy");
        Instantiate(enemy, transform.position, transform.rotation);
    }

    public int GetWave()
    {
        return waves[nextWave].number;
    }
}
