using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUIScript : MonoBehaviour
{
    public GameObject spawner;
    public GameObject player;
    public TMPro.TextMeshProUGUI timer;
    public TMPro.TextMeshProUGUI wave;
    private float timePassed = 0f;
    private SpawnerControl spawnerControl;
    // Start is called before the first frame update
    void Start()
    {
        spawnerControl = spawner.GetComponent<SpawnerControl>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTime();
        UpdateWave();
    }

    void UpdateTime()
    {
        timePassed += Time.deltaTime;
        float minutes = Mathf.FloorToInt(timePassed / 60);
        float seconds = Mathf.FloorToInt(timePassed % 60);
        timer.SetText(string.Format("{0:00}:{1:00}", minutes, seconds));
    }

    void UpdateWave()
    {
        wave.SetText("x " + spawnerControl.GetWave().ToString());
    }
}
