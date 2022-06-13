using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject play;
    public GameObject settings;
    public GameObject exit;
    public GameObject options;
    public GameObject music;
    public GameObject title;
    public GameObject backSettings;

    void Start()
    {
        options.SetActive(false);
        music.SetActive(false);
        backSettings.SetActive(false);
    }

    public void Play()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Settings()
    {
        play.SetActive(false);
        settings.SetActive(false);
        exit.SetActive(false);
        title.SetActive(false);

        options.SetActive(true);
        music.SetActive(true);
        backSettings.SetActive(true);
    }

    public void Exit()
    {
        EditorApplication.isPlaying = false;
    }

    public void BackSettings()
    {
        options.SetActive(false);
        music.SetActive(false);
        backSettings.SetActive(false);

        play.SetActive(true);
        settings.SetActive(true);
        exit.SetActive(true);
        title.SetActive(true);
    }
}
