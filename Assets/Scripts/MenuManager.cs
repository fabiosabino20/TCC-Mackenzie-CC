using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public PTP port;

    TextMeshProUGUI bluetoothInformation;

    // Start is called before the first frame update
    void Start()
    {
        bluetoothInformation = GameObject.Find("BT Information").GetComponent<TextMeshProUGUI>();
        if (port.Connect2())
        {
            bluetoothInformation.text = "Dispositivo bluetooth conectado";
            StartCoroutine(HideText());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ConnectBT()
    {
        if (port.Connect2())
        {
            bluetoothInformation.text = "Dispositivo bluetooth conectado";
        }
        else
        {
            bluetoothInformation.text = "A conexão falhou";
        }
        StartCoroutine(HideText());
    }

    public void DisconnectBT()
    {
        bluetoothInformation.text = "Dispositivo bluetooth desconectado";
        port.Disconnect();
        StartCoroutine(HideText());
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    IEnumerator HideText()
    {
        yield return new WaitForSeconds(3);
        bluetoothInformation.text = "";
    }
}
