using UnityEngine;

public class ChemicalBubbles : MonoBehaviour
{
    [Header("Bubble Animation Settings")]
    [Tooltip("Kecepatan animasi mendidih (lebih tinggi = lebih cepat)")]
    public float speed = 1.5f;
    
    [Tooltip("Kekuatan scale membesar-mengecil (0.1 = 10% perubahan)")]
    public float scaleAmount = 0.1f;
    
    [Header("Randomization")]
    [Tooltip("Offset waktu acak agar tidak sinkron dengan objek lain")]
    public bool randomizeStartTime = true;

    private Vector3 originalScale;
    private float timeOffset;

    void Start()
    {
        // Simpan scale awal
        originalScale = transform.localScale;
        
        // Randomize start time agar setiap beaker berbeda fase
        if (randomizeStartTime)
        {
            timeOffset = Random.Range(0f, 10f);
        }
        
        Debug.Log($"🧪 ChemicalBubbles aktif pada {gameObject.name}");
    }

    void Update()
    {
        // Hitung nilai PingPong (0 → 1 → 0 → 1, dst...)
        // Menggunakan Time.time untuk gerakan smooth
        float scaleFactor = Mathf.PingPong((Time.time + timeOffset) * speed, 1f);
        
        // Konversi ke range: -scaleAmount sampai +scaleAmount
        // PingPong(0→1) dikali 2 jadi (0→2), lalu dikurangi 1 jadi (-1→1)
        float scaleOffset = (scaleFactor * 2f - 1f) * scaleAmount;
        
        // Terapkan scale ke semua axis (uniform scaling)
        Vector3 newScale = originalScale * (1f + scaleOffset);
        transform.localScale = newScale;
    }

    /// <summary>
    /// Reset scale ke ukuran awal (untuk debugging)
    /// </summary>
    public void ResetScale()
    {
        transform.localScale = originalScale;
    }

    /// <summary>
    /// Set kecepatan animasi secara dinamis
    /// </summary>
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}
