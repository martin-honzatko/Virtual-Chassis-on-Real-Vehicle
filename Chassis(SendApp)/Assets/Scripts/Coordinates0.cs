using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Coordinates0 : MonoBehaviour
{
    float sour_x, sour_y, sour_z;
    private GameObject myCam;

    void Start()
    {
        myCam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void Update()
    {
        sour_x = myCam.transform.position.x - 12;
        sour_y = (myCam.transform.position.z + 11) * -1;
        sour_z = myCam.transform.position.y;
        GetComponent<TextMeshPro>().text = "X: " + sour_x.ToString("0.00") + " Y: " + sour_y.ToString("0.00") + " Z: " + sour_z.ToString("0.00");
    }
}
