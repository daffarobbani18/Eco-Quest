using UnityEngine;

public class WasteItem : MonoBehaviour
{
    [Header("Data Sampah")]
    public WasteData dataSampah; // Slot untuk memasukkan file data (Apel, Botol, dll)

    // Saat game mulai, kita ubah gambar sprite otomatis sesuai data
    void Start()
    {
        if (dataSampah != null)
        {
            GetComponent<SpriteRenderer>().sprite = dataSampah.iconSampah;
        }
    }
}