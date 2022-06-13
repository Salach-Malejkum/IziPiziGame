using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject resume;
    public GameObject settings;
    public GameObject mainMenu;
    public GameObject musicVolume;
    public GameObject backSettings;
    public GameObject playerUI;
    public GameObject pauseUI;

    void Start()
    {
        musicVolume.SetActive(false);
        backSettings.SetActive(false);
        pauseUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && playerUI.activeSelf)
        {
            playerUI.SetActive(false);
            pauseUI.SetActive(true);
            Time.timeScale = 0.0f;

            //Makes it invisable
            Cursor.visible = true;
            //Locks the mouse in place
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 1.0f;
            //Makes it invisable
            Cursor.visible = false;
            //Locks the mouse in place
            Cursor.lockState = CursorLockMode.Locked;

            playerUI.SetActive(true);
            pauseUI.SetActive(false);
        }
    }

    public void Resume()
    {
        //Makes it invisable
        Cursor.visible = false;
        //Locks the mouse in place
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1.0f;

        playerUI.SetActive(true);
        pauseUI.SetActive(false);
    }

    public void Settings()
    {
        resume.SetActive(false);
        settings.SetActive(false);
        mainMenu.SetActive(false);

        musicVolume.SetActive(true);
        backSettings.SetActive(true);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void BackSettings()
    {
        musicVolume.SetActive(false);
        backSettings.SetActive(false);

        resume.SetActive(true);
        settings.SetActive(true);
        mainMenu.SetActive(true);
    }
}
