using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishTutorial : MonoBehaviour
{
    public GameObject tutorialEndScreen;
    public GameObject playerUI;

    void OnTriggerEnter(Collider coll)
    {
        if (coll.transform.gameObject.CompareTag("Player"))
        {
            playerUI.SetActive(false);
            tutorialEndScreen.SetActive(true);
            StartCoroutine(EndTutorial());
        }
    }

    IEnumerator EndTutorial()
    {
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene("SampleScene"); 
    }
}
