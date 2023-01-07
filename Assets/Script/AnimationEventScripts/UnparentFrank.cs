using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnparentFrank : MonoBehaviour
{
    public GameObject frank;
    public GameObject standingPosition;
    public TaskManager taskManager; 

    public void orphanFrank()
    {
        //Debug.Log("WE ARE ORPHANING FRANK!");
        frank.transform.parent = null;

        //frank.transform.position = new Vector3(3.633754f, -0.130233f, 0.7654648f);
        //frank.transform.SetPositionAndRotation(new Vector3(3.633754f, -0.130233f, 0.7654648f), frank.transform.rotation);
        //frank.transform.parent = standingPosition.transform;

    }


}
