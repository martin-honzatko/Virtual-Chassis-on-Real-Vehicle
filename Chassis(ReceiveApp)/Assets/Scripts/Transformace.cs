using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/*================================================================================
 * This script is working with a positioning informations from a UDPReceive script
 * to change a position and a rotation of an object a script is attached to.
 * ===============================================================================*/

public class Transformace : MonoBehaviour
{
    /*=======================================
     * The start of a space for an variables. 
     * ======================================*/
    private GameObject myDisplay;
    public GameObject myCamera;

    public float myX1 = 0;
    public float myY1 = 0;
    public float myZ1 = 0;

    public float myA1 = 0;
    public float myB1 = 0;
    public float myC1 = 0;

    private Vector3 myPos;
    private Vector3 myPos1;
    /*=====================================
     * The end of a space for an variables. 
     * ====================================*/

    void Start() //In a Start method are declared the basic values of an variables.
    {
        myDisplay = GameObject.FindGameObjectWithTag("Display");
        myPos = new Vector3(0, 0, 0);
        myPos = transform.position;
        myPos1 = new Vector3(0, 0, 0);
    }

    void Update()
    {
        myX1 = myCamera.GetComponent<UDPReceive>().myX; //Position from a UDPPeceive script.
        myY1 = myCamera.GetComponent<UDPReceive>().myY;
        myZ1 = myCamera.GetComponent<UDPReceive>().myZ;

        myA1 = myCamera.GetComponent<UDPReceive>().myA; //Rotation from a UDPPeceive script.
        myB1 = myCamera.GetComponent<UDPReceive>().myB;
        myC1 = myCamera.GetComponent<UDPReceive>().myC;

        myPos1.x = (float)myY1; //Insert of a positioning informations into the vector.
        myPos1.y = (float)myZ1; //moving from HL position to real vehicle's position
        myPos1.z = (float)myX1;

        transform.position = myPos + myPos1; //Transformation of a position of an object to a script is attached to.

        transform.rotation = Quaternion.Euler(myA1, myB1, myC1); //Transformation of a rotation of an object to which is a script attached.
    }
}
