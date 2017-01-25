using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class COM : MonoBehaviour {

    SerialPort stream = new SerialPort("COM7", 9600); // Whatever COM port is being used

    public void write(byte baseRot, byte midRot, byte endRot) // Pass 1 instead of zero so robotic arm knows packet is real
    {
        if (baseRot == 0)
        {
            baseRot = 1;
        }
        if (midRot == 0)
        {
            midRot = 1;
        }
        if (endRot == 0)
        {
            endRot = 1;
        }

        if (baseRot > 0 && baseRot < 181 && midRot > 0 && midRot < 181 && endRot > 0 && endRot < 181) { // Within arm range of motion
            byte[] buffer = new byte[] { 255, baseRot, midRot, endRot }; // Start packet followed by poisition data
            stream.Write(buffer, 0, 4);
        }
    }

    // Use this for initialization
    void Start () {
        stream.ReadTimeout = 50;
        stream.Open();
    }
	
	// Update is called once per frame
	void Update () {

        float baseRot = (-(transform.localRotation.y * (float)(90/0.7)) + 90);
        byte byteBase = (byte)baseRot;

        float tilt = (-(transform.localRotation.x * (float)(90 / 0.7)));
        float contrib = tilt / 2;
        float midContrib = -contrib + 90;
        float endContrib = contrib + 90;

        byte byteMid = (byte)midContrib;
        byte byteEnd = (byte)endContrib;

        write(byteBase, byteMid, byteEnd);
    }
}
