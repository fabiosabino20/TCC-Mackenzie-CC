using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject reportMenu;
    [SerializeField] private TextMeshProUGUI FVCText;
    [SerializeField] private TextMeshProUGUI FEV1Text;
    [SerializeField] private TextMeshProUGUI relacaoFEV1FVCText;
    [SerializeField] private TextMeshProUGUI PEFText;
    [SerializeField] private TextMeshProUGUI FEF2575Text;

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void ToggleSoundMenu()
    {
        GameObject soundMenu = GameObject.Find("SoundMenuCanvas");
        bool isSoundMenuActive = soundMenu.transform.GetChild(0).gameObject.activeSelf;
        soundMenu.transform.GetChild(0).gameObject.SetActive(!isSoundMenuActive);
    }

    public void ToggleBluetoothMenu()
    {
        GameObject bluetoothMenu = GameObject.Find("BluetoothMenuCanvas");
        bool isBTMenuActive = bluetoothMenu.transform.GetChild(0).gameObject.activeSelf;
        bluetoothMenu.transform.GetChild(0).gameObject.SetActive(!isBTMenuActive);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void ShowReportMenu()
    {
        Time.timeScale = 0;
        reportMenu.SetActive(true);

        BluetoothManager bluetoothManager = GameObject.Find("BluetoothManager").GetComponent<BluetoothManager>();

        FVCText.text = "FVC: " + bluetoothManager.FVC.ToString("F2") + " L";
        FEV1Text.text = "FEV1: " + bluetoothManager.FEV1.ToString("F2") + " L";
        relacaoFEV1FVCText.text = "FEV1/FVC: " + bluetoothManager.relacaoFEV1FVC.ToString("F2") + "%";
        PEFText.text = "PEF: " + bluetoothManager.PEF.ToString("F2") + " L/s";
        FEF2575Text.text = "FEF 25-75: " + bluetoothManager.FVC.ToString("F2") + " L/s";
    }
}
