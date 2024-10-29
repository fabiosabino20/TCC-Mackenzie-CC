using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
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
}
