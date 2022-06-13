using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawnerScript : MonoBehaviour
{
    public GameObject spawner;
    public GameObject melee;
    public GameObject ranged;

    private float maxVal = 45.0f;
    private float minVal = 0.0f;
    private GameObject weapon = null;
    private int wave = 0;
    private SpawnerControl spawnerControl;
    // Start is called before the first frame update
    void Start()
    {
        spawnerControl = spawner.GetComponent<SpawnerControl>();
    }

    // Update is called once per frame
    void Update()
    {
        int tmp = spawnerControl.nextWave + 1;
        if (wave != tmp)
        {
            float x = Random.Range(minVal, maxVal);
            float z = Random.Range(minVal, maxVal);

            if (weapon != null)
            {
                Destroy(weapon);
            }

            Vector3 spawnPos = new Vector3(0.0f, 0.0f, 0.0f);
            spawnPos.x = x;
            spawnPos.z = z;
            if (Random.value <= 0.5)
            {
                spawnPos.y = melee.gameObject.transform.position.y;
                weapon = Instantiate(melee, spawnPos, Quaternion.identity);
            }
            else
            {
                spawnPos.y = ranged.gameObject.transform.position.y;
                weapon = Instantiate(ranged, spawnPos, Quaternion.identity);
            }
            wave = tmp;
        }
    }
}
