using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusDistanceManager : MonoBehaviour
{
    [SerializeField] GameObject Near;
    [SerializeField] GameObject Far;
    // Start is called before the first frame update
    void Start()
    {
        Near.SetActive(true);
        Far.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("C!!!!!!!!!!!!!!!!!!!!!!!!!!");
            if(Near.activeSelf==true)
            {
                Debug.Log("Near!!!!!!!!!!!!!!!!!!!!!!!!!!");
                Near.SetActive(false);
                Far.SetActive(true);
            }
            else if (Far.activeSelf == true)
            {
                Debug.Log("Far!!!!!!!!!!!!!!!!!!!!!!!!!!");
                Far.SetActive(false);
                Near.SetActive(true);
            }
        }
    }
}
