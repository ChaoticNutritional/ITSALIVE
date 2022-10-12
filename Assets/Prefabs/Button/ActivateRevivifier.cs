using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateRevivifier : MonoBehaviour
{

    [SerializeField]
    private GameObject revivifier = null;

    public GameObject spawnLocation = null;

    public bool deviceEnabled = false;

    public void Activate()
    {
        //spawn the revivifier (bolt) at location
        GameObject lightningFX = Instantiate(revivifier, spawnLocation.transform.position, spawnLocation.transform.rotation);
        Destroy(lightningFX, 2);
    }
}
