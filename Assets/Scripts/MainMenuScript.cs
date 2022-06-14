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
    public GameObject tutorial;
    public GameObject arena;
    public GameObject zombieImage;

    void Start()
    {
        options.SetActive(false);
        music.SetActive(false);
        backSettings.SetActive(false);
        tutorial.SetActive(false);
        arena.SetActive(false);
    }

    public void Play()
    {
        play.SetActive(false);
        settings.SetActive(false);
        exit.SetActive(false);
        title.SetActive(false);
        zombieImage.SetActive(false);

        tutorial.SetActive(true);
        arena.SetActive(true);
        backSettings.SetActive(true);
    }

    public void Settings()
    {
        play.SetActive(false);
        settings.SetActive(false);
        exit.SetActive(false);
        title.SetActive(false);
        zombieImage.SetActive(false);

        options.SetActive(true);
        music.SetActive(true);
        backSettings.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void BackSettings()
    {
        options.SetActive(false);
        music.SetActive(false);
        backSettings.SetActive(false);
        tutorial.SetActive(false);
        arena.SetActive(false);

        play.SetActive(true);
        settings.SetActive(true);
        exit.SetActive(true);
        title.SetActive(true);
        zombieImage.SetActive(true);
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void Arena()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
