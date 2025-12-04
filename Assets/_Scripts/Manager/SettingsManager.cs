using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Slider untuk mengatur volume Background Music")]
    public Slider sliderBGM;
    
    [Tooltip("Slider untuk mengatur volume Sound Effects")]
    public Slider sliderSFX;

    [Header("Audio Sources")]
    [Tooltip("AudioSource yang memutar Background Music")]
    public AudioSource bgmSource;
    
    [Tooltip("AudioSource yang memutar Sound Effects")]
    public AudioSource sfxSource;

    void Start()
    {
        // Sinkronkan slider dengan volume AudioSource saat ini
        if (sliderBGM != null && bgmSource != null)
        {
            sliderBGM.value = bgmSource.volume;
            sliderBGM.onValueChanged.AddListener(SetVolumeBGM);
            Debug.Log($"✅ Slider BGM initialized at volume: {bgmSource.volume}");
        }
        else
        {
            Debug.LogWarning("⚠️ Slider BGM atau AudioSource BGM belum di-assign!");
        }

        if (sliderSFX != null && sfxSource != null)
        {
            sliderSFX.value = sfxSource.volume;
            sliderSFX.onValueChanged.AddListener(SetVolumeSFX);
            Debug.Log($"✅ Slider SFX initialized at volume: {sfxSource.volume}");
        }
        else
        {
            Debug.LogWarning("⚠️ Slider SFX atau AudioSource SFX belum di-assign!");
        }
    }

    /// <summary>
    /// Mengatur volume Background Music secara realtime
    /// </summary>
    /// <param name="value">Nilai slider (0.0 - 1.0)</param>
    public void SetVolumeBGM(float value)
    {
        if (bgmSource != null)
        {
            bgmSource.volume = value;
            Debug.Log($"🎵 BGM Volume: {value:F2}");
        }
    }

    /// <summary>
    /// Mengatur volume Sound Effects secara realtime
    /// </summary>
    /// <param name="value">Nilai slider (0.0 - 1.0)</param>
    public void SetVolumeSFX(float value)
    {
        if (sfxSource != null)
        {
            sfxSource.volume = value;
            Debug.Log($"🔊 SFX Volume: {value:F2}");
        }
    }

    void OnDestroy()
    {
        // Cleanup listeners untuk mencegah memory leak
        if (sliderBGM != null)
            sliderBGM.onValueChanged.RemoveListener(SetVolumeBGM);
        
        if (sliderSFX != null)
            sliderSFX.onValueChanged.RemoveListener(SetVolumeSFX);
    }
}
