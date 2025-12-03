using UnityEngine;
using UnityEngine.SceneManagement; // Wajib

public class LevelMenuController : MonoBehaviour
{
    public void PindahKeRuangSortir()
    {
        // Pastikan nama scene tujuan SAMA PERSIS dengan di folder _Scenes
        SceneManager.LoadScene("03_Game_Processing");
    }
}