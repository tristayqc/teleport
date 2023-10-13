using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCueToDisplay : MonoBehaviour
{
    [SerializeField] GameObject CueToDisplay;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            CueToDisplay.SetActive(true);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("MainCamera")&& !CueToDisplay.activeSelf)
        {
            CueToDisplay.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            CueToDisplay.SetActive(false);
        }
    }
}
