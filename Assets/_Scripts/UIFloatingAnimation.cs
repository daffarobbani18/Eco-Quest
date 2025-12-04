using UnityEngine;

public class UIFloatingAnimation : MonoBehaviour
{
    [Header("Floating Settings")]
    [Tooltip("Kekuatan/jarak naik turun dalam pixel")]
    public float amplitude = 10f;
    
    [Tooltip("Kecepatan gerakan naik turun")]
    public float speed = 2f;

    private Vector3 startPos;
    private RectTransform rectTransform;

    void Start()
    {
        // Ambil komponen RectTransform
        rectTransform = GetComponent<RectTransform>();
        
        if (rectTransform == null)
        {
            Debug.LogError("❌ UIFloatingAnimation: RectTransform tidak ditemukan! Script ini hanya untuk UI objects.");
            enabled = false;
            return;
        }
        
        // Simpan posisi awal (anchoredPosition untuk UI)
        startPos = rectTransform.anchoredPosition;
        
        Debug.Log("✅ UIFloatingAnimation: Floating animation aktif!");
    }

    void Update()
    {
        if (rectTransform == null) return;
        
        // Hitung offset Y menggunakan fungsi sinus
        // Mathf.Sin menghasilkan nilai antara -1 sampai 1
        // Dikali amplitude untuk mendapatkan jarak naik-turun
        float offsetY = Mathf.Sin(Time.time * speed) * amplitude;
        
        // Terapkan offset ke posisi awal
        // Hanya sumbu Y yang berubah, X tetap sama dengan startPos
        rectTransform.anchoredPosition = new Vector3(startPos.x, startPos.y + offsetY, startPos.z);
    }
}
