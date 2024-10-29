using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BluetoothManager : MonoBehaviour
{
    [SerializeField] PTP port;
    [SerializeField] TextMeshProUGUI bluetoothInformation;

    // Start is called before the first frame update
    void Start()
    {
        if (port.Connect2())
        {
            bluetoothInformation.text = "Dispositivo bluetooth conectado";
            StartCoroutine(HideText());
        }
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

    IEnumerator HideText()
    {
        yield return new WaitForSeconds(3);
        bluetoothInformation.text = "";
    }
}
