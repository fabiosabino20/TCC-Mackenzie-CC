using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using TMPro;
using UnityEngine;


public class ArduinoReceiver : MonoBehaviour
{
    public PTP port;
    float flow_rate = 0;

    PlayerController playerController;

    void Awake()
    {
        port = GameObject.Find("ComPort").GetComponent<PTP>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void Start()
    {
        // On Receive data
        port.OnPackReceived -= PackReceiveProcessing;
        port.OnPackReceived += PackReceiveProcessing;
    }

    void PackReceiveProcessing(PTP.DATA data)
    {
        flow_rate = data.GetFloat();
        playerController.engineHeat = flow_rate;
        //Debug.Log("\n-= Pack Data = -\n");
        //Debug.Log(flow_rate);
    }

    void Update()
    {
        
    }
}
