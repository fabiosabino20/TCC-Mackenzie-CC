using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using ArduinoBluetoothAPI;
using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;

public class BluetoothManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI bluetoothInformation;

    private BluetoothHelper bluetoothHelper;
    private string deviceName = "HC-05"; // Nome do dispositivo Bluetooth do Arduino
    private List<string> dataBuffer = new List<string>(); // Buffer para armazenar as linhas recebidas
    private TextMeshProUGUI flowText;

    float flow_rate = 0;
    float FVC = 0;
    float FEV1 = 0;
    float PEF = 0;
    float relacaoFEV1FVC = 0;
    float FEF2575 = 0;

    PlayerController playerController;

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        try
        {
            // Inicializa o BluetoothHelper com o nome do dispositivo
            bluetoothHelper = BluetoothHelper.GetInstance(deviceName);
            bluetoothHelper.OnConnected += OnConnected;
            bluetoothHelper.OnConnectionFailed += OnConnectionFailed;
            bluetoothHelper.OnDataReceived += OnDataReceived;

            // Configura o modo de leitura baseado em terminador para delimitar as mensagens
            bluetoothHelper.setTerminatorBasedStream("\n");

            // Inicia a tentativa de conexão
            bluetoothHelper.Connect();
        }
        catch (BluetoothHelper.BlueToothNotEnabledException ex)
        {
            Debug.LogError($"Bluetooth não está ativado. {ex}");
        }
    }

    void OnConnected(BluetoothHelper helper)
    {
        bluetoothInformation.text = "Dispositivo bluetooth conectado";
        Debug.Log("Conectado ao dispositivo Bluetooth.");
        bluetoothHelper.StartListening(); // Começa a escutar por dados recebidos
    }

    void OnConnectionFailed(BluetoothHelper helper)
    {
        bluetoothInformation.text = "Erro ao conectar ao dispositivo Bluetooth";
        Debug.LogError("Falha na conexão com o dispositivo Bluetooth");
    }

    void OnDataReceived(BluetoothHelper helper)
    {
        Debug.Log("A");
        // Lê os dados recebidos linha por linha
        string line = helper.Read().Trim(); // Remove espaços ou quebras de linha extras
        if (!string.IsNullOrEmpty(line))
        {
            dataBuffer.Add(line);
            Debug.Log($"Linha recebida: {line}");

            // Verifica se temos as 6 linhas necessárias
            if (dataBuffer.Count >= 6)
            {
                // Junta as linhas acumuladas para processamento
                string combinedData = string.Join("\n", dataBuffer);
                ProcessReceivedData(combinedData);

                // Limpa o buffer após processar os dados
                dataBuffer.Clear();
            }
        }
    }
    void ProcessReceivedData(string data)
    {
        try
        {
            // Divide os dados em linhas
            string[] lines = data.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length >= 6)
            {
                // Parseia os valores para as variáveis correspondentes usando a cultura invariável
                flow_rate = float.Parse(lines[0], CultureInfo.InvariantCulture);
                FVC = float.Parse(lines[1], CultureInfo.InvariantCulture);
                FEV1 = float.Parse(lines[2], CultureInfo.InvariantCulture);
                PEF = float.Parse(lines[3], CultureInfo.InvariantCulture);
                relacaoFEV1FVC = float.Parse(lines[4], CultureInfo.InvariantCulture);
                FEF2575 = float.Parse(lines[5], CultureInfo.InvariantCulture);

                if (flowText != null)
                {
                    UpdateFlowText(flow_rate);
                }

                Debug.Log($"Fluxo: {flow_rate} L/s");
                Debug.Log($"FVC: {FVC} L");
                Debug.Log($"FEV1: {FEV1} L");
                Debug.Log($"PEF: {PEF} L/s");
                Debug.Log($"Relação FEV1/FVC: {relacaoFEV1FVC}%");
                Debug.Log($"FEF 25-75: {FEF2575} L/s");

                // Atualiza o PlayerController com o valor de flow_rate, caso exista
                if (playerController != null)
                {
                    playerController.engineHeat = flow_rate;
                }
            }
            else
            {
                Debug.LogWarning("Dados incompletos recebidos.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erro ao processar os dados recebidos: {ex.Message}");
        }
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
        {
            playerController = GameObject.Find("Player").GetComponent<PlayerController>();
            flowText = GameObject.Find("Fluxo").GetComponent<TextMeshProUGUI>();
            Debug.Log("PlayerController conectado.");
        }
        else
        {
            playerController = null;
        }
    }

    private void OnApplicationQuit()
    {
        // Finaliza a conexão Bluetooth ao sair do aplicativo
        if (bluetoothHelper != null)
        {
            bluetoothHelper.Disconnect();
        }
    }

    public void ConnectBT()
    {
        if (bluetoothHelper == null || bluetoothHelper.isConnected())
        {
            bluetoothInformation.text = "Já conectado ao dispositivo Bluetooth!";
            StartCoroutine(HideText());
            return;
        }

        try
        {
            bluetoothHelper.Connect();
            bluetoothInformation.text = "Conectando ao dispositivo Bluetooth...";
        }
        catch (BluetoothHelper.BlueToothNotReadyException ex)
        {
            bluetoothInformation.text = "Dispositivo Bluetooth não emparelhado!";
            Debug.LogError(ex);
        }

        StartCoroutine(HideText());
    }

    public void DisconnectBT()
    {
        if (bluetoothHelper != null && bluetoothHelper.isConnected())
        {
            bluetoothHelper.Disconnect();
            bluetoothInformation.text = "Dispositivo Bluetooth desconectado!";
        }
        else
        {
            bluetoothInformation.text = "Nenhum dispositivo conectado.";
        }

        StartCoroutine(HideText());
    }

    IEnumerator HideText()
    {
        yield return new WaitForSeconds(3);
        bluetoothInformation.text = "";
    }

    private void UpdateFlowText(float flowValue)
    {
        flowText.text = "Fluxo: " + flowValue.ToString("F2") + " L/min";
    }
}
