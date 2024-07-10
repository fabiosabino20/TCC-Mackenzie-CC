using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArduinoReceiver : MonoBehaviour
{
    public PTP port;
    Rigidbody2D playerRb;
    float timer = 0;
    Vector2 BreathStr;
    float flow_rate = 0;
    int previous_value_received = 0;

    void Start()
    {
        // On Receive data
        port.OnPackReceived -= PackReceiveProcessing;
        port.OnPackReceived += PackReceiveProcessing;
        playerRb = GameObject.Find("Circle").GetComponent<Rigidbody2D>();
    }

    void PackReceiveProcessing(PTP.DATA data)
    {
        int value_received = data.GetInt();
        flow_rate += value_received;
        print("\n-= Pack Data = -\n");
        print(value_received);

        if (value_received > 0 && value_received < 10 && value_received >= previous_value_received)
        {
            BreathStr = new Vector2(0, value_received);
        }
        else if (value_received >= 15)
        {
            BreathStr = new Vector2(0, 10/value_received);
        }

        //Vector2 flow_rate_force = new Vector2(0,(float)flow_rate).normalized;
        //Vector2 flow_rate_force = new Vector2(0, flow_rate);
        playerRb.AddForce(BreathStr * 10, ForceMode2D.Force);
        previous_value_received = value_received;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1)
        {
            timer = 0;
            flow_rate /= (float)4.8;
            //print("\n-= Flow Rate = -\n");
            //print(flow_rate);
        }
    }
}
