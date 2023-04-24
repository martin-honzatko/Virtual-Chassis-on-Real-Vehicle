using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UDPResponse : MonoBehaviour
{
	public TextMeshPro textMesh = null;
	string dataString;

	void Start()
	{

	}

	void Update()
	{

	}

	public void ResponseToUDPPacket(string fromIP, string fromPort, byte[] data)
	{
		dataString = transform.position.x.ToString() + " " + transform.position.y.ToString() + " " + transform.position.z.ToString() + "\n";
		dataString = "ahoj\n";

		if (textMesh != null)
		{
			textMesh.text = dataString;
		}
	}
}
