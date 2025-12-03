using UnityEngine;
using UnityEngine.SceneManagement; // Wajib

public class MainMenuController : MonoBehaviour
{
    [Header("Settings")]
    public string namaScenePertama = "01_Hub_Klub"; // Kita akan ke Hub dulu, bukan langsung main

    public void MulaiGame()
    {
        // Pastikan scene ini ada di Build Settings!
        SceneManager.LoadScene(namaScenePertama);
    }

    public void KeluarGame()
    {
        Debug.Log("Keluar dari Game!");
        Application.Quit();
    }
}