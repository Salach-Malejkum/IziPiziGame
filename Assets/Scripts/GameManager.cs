using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject movementTip;
    public GameObject attackTip;
    public GameObject pickUpTip;
    public GameObject zombie;
    public GameObject pickUpWeapon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (zombie == null)
        {
            attackTip.SetActive(false);
            pickUpTip.SetActive(true);
        }

        if (pickUpWeapon == null)
        {
            pickUpTip.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.transform.gameObject.CompareTag("Player"))
        {
            movementTip.SetActive(false);
            attackTip.SetActive(true);
        }
    }
}
