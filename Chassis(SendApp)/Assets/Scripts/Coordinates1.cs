using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Coordinates1 : MonoBehaviour
{
    float sour_rx, sour_ry, sour_rz;
    private GameObject myCam2;

    void Start()
    {
        myCam2 = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void Update()
    {
        sour_rx = myCam2.transform.rotation.eulerAngles.x;
        sour_ry = myCam2.transform.rotation.eulerAngles.y;
        sour_rz = myCam2.transform.rotation.eulerAngles.z;
        GetComponent<TextMeshPro>().text = "A: " + sour_rx.ToString("0.00") + " B: " + sour_ry.ToString("0.00") + " C: " + sour_rz.ToString("0.00");
    }
}
