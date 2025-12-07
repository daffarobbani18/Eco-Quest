# 📋 RINGKASAN LAPORAN PROJECT ECO-QUEST
## Game Edukasi Pemilahan Sampah Berbasis Unity 2D

---

## 📌 INFORMASI PROJECT

**Nama Project:** Eco-Quest - Game Edukasi Pemilahan Sampah  
**Platform:** Windows PC (Unity 2D)  
**Target User:** Siswa Sekolah Dasar/Menengah  
**Tujuan:** Mengajarkan konsep pemilahan sampah (Organik, Anorganik, B3) melalui gameplay interaktif  
**Engine:** Unity 2021.3 LTS  
**Bahasa Pemrograman:** C# (.NET Framework)  
**Repository:** GitHub - Eco-Quest (Owner: daffarobbani18)

---

## 🎯 TUJUAN PEMBELAJARAN

1. **Kognitif:**
   - Memahami 3 kategori sampah (Organik, Anorganik, B3)
   - Mengetahui contoh sampah di setiap kategori
   - Memahami dampak pemilahan sampah terhadap lingkungan

2. **Afektif:**
   - Menumbuhkan kesadaran peduli lingkungan
   - Membangun kebiasaan pilah sampah sejak dini

3. **Psikomotorik:**
   - Melatih kecepatan dan akurasi dalam pengambilan keputusan
   - Mengembangkan koordinasi mata-tangan

---

## 🎮 FITUR UTAMA GAME

### **1. SISTEM PENGUMPULAN SAMPAH (Collection Phase)**
**Scene:** `02_Game_Kantin`, `02_Lab_IPA`

**Mekanik:**
- **Point & Click:** Pemain mengklik sampah yang tersebar di area
- **Visual Feedback:** Animasi sampah terbang ke tas/wadah
- **Progress Tracking:** Counter jumlah sampah terkumpul
- **Timer (Optional):** Batas waktu pengumpulan (dapat dimatikan untuk mode santai)

**Fitur Pendukung:**
- Auto-save inventory ke GameManager (DontDestroyOnLoad)
- Transisi mulus ke scene Processing setelah target tercapai
- Panel Win dengan tombol "Lanjut ke Sortir"

---

### **2. SISTEM PEMILAHAN SAMPAH (Processing Phase)**
**Scene:** `03_Game_Processing`

**Mekanik:**
- **Drag & Drop:** Sampah muncul di conveyor belt, pemain drag ke tong yang benar
- **3 Kategori Tong:**
  - 🟢 **Organik** (Hijau) - Sisa makanan, daun, kulit buah
  - 🟡 **Anorganik** (Kuning) - Plastik, kertas, kaleng
  - 🔴 **B3** (Merah) - Baterai, lampu, limbah kimia
- **Scoring System:**
  - Benar: +10 poin
  - Salah: -5 poin
  - Game Over jika skor < 0
- **Spawner Otomatis:** Sampah spawn secara berkala dari inventory
- **Timer:** Batas waktu sesuai kesulitan level

**Fitur Pendukung:**
- **Sorting Guide Panel:** Panduan visual kategori sampah sebelum game dimulai
- **Win Condition:** Semua sampah selesai dipilah dengan skor > 0
- **Lose Condition:** Skor < 0 atau waktu habis
- **Retry System:** Tombol "Ulangi" kembali ke scene Collection (bukan Processing)

---

### **3. SISTEM BRIEFING & TUTORIAL**
**Component:** `BriefingSequence.cs`

**Fitur:**
- **Panel Intro:** Animasi judul level + info target
- **Dialog Guru:** Sequence dialog step-by-step dengan tombol "Next"
- **Pause System:** Game di-pause (Time.timeScale = 0) saat briefing
- **Auto-Continue:** Tombol "Mulai" muncul setelah dialog selesai
- **Data-Driven:** Dialog diambil dari ScriptableObject `LevelData`

---

### **4. SISTEM PROGRESSION (Unlock Level)**
**Component:** `LevelSelectionManager.cs`, `LevelButton.cs`

**Mekanik:**
- **Linear Progression:** Level dibuka secara berurutan
- **Persistent Data:** Progress disimpan di PlayerPrefs ("LevelTerbuka")
- **Visual State:**
  - Level terbuka: Tombol aktif + highlight
  - Level terkunci: Padlock icon + grayscale + audio "locked"
- **Unlock Trigger:** Selesaikan level N → Unlock level N+1

**Fitur:**
- Audio feedback (klik kertas untuk terbuka, klik terkunci untuk locked)
- Delay animasi sebelum transisi scene
- Index level untuk tracking player progress

---

### **5. SISTEM AUDIO**
**Component:** `MainMenuManager.cs`, `SettingsManager.cs`, `LevelButton.cs`

**Audio Types:**
- **BGM (Background Music):** Musik loop di menu utama
- **SFX (Sound Effects):**
  - Button click (kertas)
  - Level locked (denied sound)
  - Fail sound (sampah masuk fail zone)
  - Success sound (level selesai)

**Kontrol:**
- Volume slider di Settings Panel (0-100%)
- Persistent settings (PlayerPrefs)
- Toggle music on/off

---

### **6. SISTEM ANIMASI & UI POLISH**

**A. UI Floating Animation**
**Component:** `UIFloatingAnimation.cs`
- Logo/icon bergerak naik-turun (sine wave)
- Smooth animation menggunakan Mathf.Sin()

**B. Intro Panel Animation**
**Component:** `IntroPanelController.cs`
- Fade in/out dengan UnscaledTime (tidak terpengaruh pause)
- Auto-hide setelah durasi tertentu
- Animasi scale (punch effect)

**C. Crossfade Scene Transition**
**Component:** `LevelLoader.cs`
- Fade to black saat pindah scene
- Coroutine dengan yield WaitForSeconds
- Smooth visual transition

**D. Chemical Bubbles (Lab IPA)**
**Component:** `ChemicalBubbles.cs`
- Animasi gelembung naik-turun (PingPong)
- Dekorasi visual untuk scene Lab IPA

---

### **7. SISTEM GAME OVER & RETRY**
**Component:** `GameOverUI.cs`, `FailZone.cs`

**Trigger Game Over:**
- Skor < 0 (terlalu banyak salah)
- Sampah jatuh ke fail zone (ujung conveyor belt)

**Mekanik:**
- Panel Game Over muncul (Time.timeScale = 0)
- Opsi:
  - **Ulangi Level:** Kembali ke scene Collection (bukan Processing)
  - **Kembali ke Hub:** Scene level selection
  - **Main Menu:** Kembali ke menu utama
  - **Quit:** Keluar game

**Mapping Scene:**
- Level 1 (Kantin) → `02_Game_Kantin`
- Level 2 (Lab IPA) → `02_Lab_IPA`
- Level 3 (Gudang) → `05_Game_Gudang`

---

### **8. SISTEM NEXT LEVEL**
**Component:** `ProcessingLevelManager.cs`

**Mekanik:**
- Field `namaSceneSelanjutnya` di Inspector (data-driven)
- Validasi scene di Build Settings dengan `SceneExists()`
- Fallback ke Hub jika scene tidak ditemukan
- Clear inventory sebelum load scene baru
- Reset Time.timeScale untuk prevent freeze

---

## 🗺️ FLOW GAME (User Journey)

```
┌─────────────────────────────────────────────────────┐
│ 1. MAIN MENU (00_MainMenu)                         │
│    - Logo floating animation                        │
│    - Tombol: Play, Settings, Credits, Quit         │
│    - BGM loop                                       │
└──────────────┬──────────────────────────────────────┘
               ↓ [Klik Play]
┌─────────────────────────────────────────────────────┐
│ 2. HUB - LEVEL SELECTION (01_Hub_Klub)             │
│    - Grid layout level buttons                      │
│    - Level 1: Terbuka                               │
│    - Level 2-N: Terkunci (unlock setelah clear)    │
│    - Audio feedback (click/locked)                  │
└──────────────┬──────────────────────────────────────┘
               ↓ [Pilih Level]
┌─────────────────────────────────────────────────────┐
│ 3. COLLECTION PHASE (02_Game_Kantin / 02_Lab_IPA)  │
│    ┌─────────────────────────────────────────────┐ │
│    │ A. BRIEFING                                 │ │
│    │    - Panel Intro (animasi 3s)              │ │
│    │    - Dialog Guru (sequence)                │ │
│    │    - Tombol "Mulai" muncul                 │ │
│    │    - Time.timeScale = 0                    │ │
│    └─────────────────────────────────────────────┘ │
│                     ↓ [Klik Mulai]                  │
│    ┌─────────────────────────────────────────────┐ │
│    │ B. GAMEPLAY                                 │ │
│    │    - Klik sampah untuk kumpulkan           │ │
│    │    - Progress: 0/12 sampah                 │ │
│    │    - (Optional) Timer countdown            │ │
│    │    - Animasi sampah terbang ke wadah      │ │
│    └─────────────────────────────────────────────┘ │
│                     ↓ [Target Tercapai]             │
│    ┌─────────────────────────────────────────────┐ │
│    │ C. PANEL WIN                                │ │
│    │    - "Misi Selesai!"                       │ │
│    │    - Tombol: Lanjut ke Sortir              │ │
│    └─────────────────────────────────────────────┘ │
└──────────────┬──────────────────────────────────────┘
               ↓ [Lanjut ke Sortir]
               ↓ [Inventory tersimpan di GameManager]
┌─────────────────────────────────────────────────────┐
│ 4. PROCESSING PHASE (03_Game_Processing)           │
│    ┌─────────────────────────────────────────────┐ │
│    │ A. BRIEFING                                 │ │
│    │    - Dialog Guru (instruksi sortir)        │ │
│    │    - Tombol "Mulai"                        │ │
│    │    - Time.timeScale = 0                    │ │
│    └─────────────────────────────────────────────┘ │
│                     ↓ [Klik Mulai]                  │
│    ┌─────────────────────────────────────────────┐ │
│    │ B. SORTING GUIDE PANEL                      │ │
│    │    - Tampilkan kategori sampah (preview)   │ │
│    │    - Icon unique dari inventory            │ │
│    │    - 3 kolom: Organik, Anorganik, B3      │ │
│    │    - Tombol "Lanjut"                       │ │
│    │    - Time.timeScale tetap 0                │ │
│    └─────────────────────────────────────────────┘ │
│                     ↓ [Klik Lanjut]                 │
│    ┌─────────────────────────────────────────────┐ │
│    │ C. GAMEPLAY                                 │ │
│    │    - Spawner aktif (sampah dari inventory) │ │
│    │    - Drag sampah ke tong yang benar        │ │
│    │    - Skor: +10 (benar) / -5 (salah)       │ │
│    │    - Timer: Countdown                      │ │
│    │    - Target: Sortir semua sampah           │ │
│    └─────────────────────────────────────────────┘ │
│                     ↓                               │
│         ┌───────────┴───────────┐                  │
│         ↓                       ↓                   │
│    [Skor > 0]              [Skor < 0]              │
│    [Waktu > 0]             [Waktu Habis]           │
│         ↓                       ↓                   │
│    ┌─────────┐            ┌──────────┐            │
│    │ D. WIN  │            │ E. LOSE  │            │
│    │  PANEL  │            │  PANEL   │            │
│    └────┬────┘            └────┬─────┘            │
│         │                      │                   │
│    [Lanjut]               [Ulangi/Hub]            │
└─────────┴──────────────────────┴───────────────────┘
          ↓                      ↓
     [Next Level]          [Retry Collection]
     [atau Hub]            [atau Main Menu]
```

---

## 🏗️ ARSITEKTUR TEKNIS

### **DESIGN PATTERN YANG DIGUNAKAN:**

#### **1. SINGLETON PATTERN**
**Implementasi:**
- `GameManager.Instance` (global state management)
- `CollectionLevelManager.Instance` (scene-specific)
- `ProcessingLevelManager.Instance` (scene-specific)

**Manfaat:**
- Satu sumber kebenaran untuk data penting (skor, inventory, timer)
- Mudah diakses dari script manapun tanpa referensi kompleks
- Prevent duplikasi GameObject

**Code:**
```csharp
public static GameManager Instance;
void Awake() {
    if (Instance == null) {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    } else {
        Destroy(gameObject);
    }
}
```

---

#### **2. SCRIPTABLE OBJECT PATTERN (Data-Driven Design)**
**Implementasi:**
- `WasteData.asset` - Data sampah (nama, kategori, sprite, poin)
- `LevelData.asset` - Data level (dialog, target, timer)

**Manfaat:**
- Konten bisa diedit tanpa coding (guru bisa ubah dialog)
- Reusable data (1 WasteData dipakai di banyak level)
- Easy to create variants (copy-paste asset, edit properties)

**Code:**
```csharp
[CreateAssetMenu(menuName = "PjBL/Waste Data")]
public class WasteData : ScriptableObject {
    public string namaSampah;
    public WasteType tipeSampah;
    public Sprite iconSampah;
    public int nilaiPoin;
}
```

---

#### **3. STATE MACHINE**
**Implementasi:**
- Game states: `IDLE → BRIEFING → GUIDE → PLAYING → WIN/LOSE`
- Bool flags: `isGamePlaying`, `isGameActive`

**Manfaat:**
- Clear control flow (kapan pemain bisa interaksi)
- Easy debugging (tahu state saat ini)
- Prevent race conditions

---

#### **4. OBSERVER PATTERN (Event-Driven)**
**Implementasi:**
- `Button.onClick.AddListener()`
- UI update triggered by game state changes

**Manfaat:**
- Decoupling (UI tidak tahu detail game logic)
- Flexible (mudah tambah listener baru)

---

#### **5. OBJECT POOLING** *(Recommended untuk optimasi)*
**Implementasi:** `WasteSpawner` (spawn/recycle sampah)

**Manfaat:**
- Hemat memory (recycle GameObject, tidak Instantiate terus)
- Smooth performance di low-end PC

---

#### **6. COROUTINE (Async Operations)**
**Implementasi:**
- `IEnumerator Start()` - Delay untuk sync antar scene
- `WaitForSecondsRealtime()` - Animasi saat pause
- Scene transitions dengan fade

**Manfaat:**
- Non-blocking operations (animasi smooth)
- Time-independent (tidak terpengaruh Time.timeScale)

---

### **FILE STRUCTURE:**

```
Eco-Quest/
├── Assets/
│   ├── _Scenes/
│   │   ├── 00_MainMenu.unity
│   │   ├── 01_Hub_Klub.unity
│   │   ├── 02_Game_Kantin.unity
│   │   ├── 02_Lab_IPA.unity
│   │   └── 03_Game_Processing.unity
│   │
│   ├── _Scripts/
│   │   ├── Manager/
│   │   │   ├── GameManager.cs (Singleton, DontDestroyOnLoad)
│   │   │   ├── MainMenuManager.cs
│   │   │   ├── LevelSelectionManager.cs
│   │   │   └── SettingsManager.cs
│   │   │
│   │   ├── Gameplay/
│   │   │   ├── CollectionLevelManager.cs
│   │   │   ├── ProcessingLevelManager.cs
│   │   │   ├── CollectionItem.cs (Click sampah)
│   │   │   ├── DragController.cs (Drag sampah ke tong)
│   │   │   ├── BriefingSequence.cs
│   │   │   ├── WasteSpawner.cs
│   │   │   └── FailZone.cs
│   │   │
│   │   ├── UI/
│   │   │   ├── LevelButton.cs
│   │   │   ├── LevelLoader.cs (Scene transition)
│   │   │   ├── GameOverUI.cs
│   │   │   ├── SortingGuideUI.cs
│   │   │   ├── UIFloatingAnimation.cs
│   │   │   ├── IntroPanelController.cs
│   │   │   └── ChemicalBubbles.cs
│   │   │
│   │   └── Data/
│   │       ├── WasteData.cs (ScriptableObject)
│   │       └── LevelData.cs (ScriptableObject)
│   │
│   ├── Art/
│   │   ├── Sprites/ (Sampah, Tong, Background)
│   │   └── UI/ (Button, Panel, Icon)
│   │
│   ├── Audio/
│   │   ├── BGM/
│   │   └── SFX/
│   │
│   ├── Prefabs/
│   │   ├── Panel_DialogGuru.prefab
│   │   └── Sampah_Prefabs/
│   │
│   └── Data/
│       ├── WasteData_BotolPlastik.asset
│       ├── WasteData_Apel.asset
│       ├── LevelData_Kantin.asset
│       └── LevelData_Processing.asset
│
├── ProjectSettings/
├── Packages/
└── .gitignore
```

---

## 📊 DATA PERSISTENCE

### **PlayerPrefs (Local Storage):**

| Key | Value Type | Purpose |
|-----|------------|---------|
| `LevelTerbuka` | int | Index level tertinggi yang terbuka (1-N) |
| `MusicVolume` | float | Volume musik (0.0 - 1.0) |
| `SFXVolume` | float | Volume sound effect (0.0 - 1.0) |
| `TotalAttempts_Level{N}` | int | Total percobaan di level N (untuk adaptive difficulty) |
| `CorrectAttempts_Level{N}` | int | Percobaan benar di level N (untuk mastery learning) |

### **DontDestroyOnLoad (Runtime Data):**
- **GameManager:** Skor, timer, inventory, win/lose panels, level index
- Bertahan saat scene transition (Collection → Processing)
- Cleared saat kembali ke Main Menu atau Hub

---

## 🎨 UI/UX DESIGN

### **Visual Hierarchy:**
1. **Tier 1 (Critical Info):** Skor, Timer (large, top HUD)
2. **Tier 2 (Gameplay):** Sampah, Tong (center screen)
3. **Tier 3 (Secondary):** Progress bar, hints (smaller, bottom)

### **Color Coding:**
- 🟢 **Hijau:** Organik, Success, Win
- 🟡 **Kuning:** Anorganik, Warning
- 🔴 **Merah:** B3, Error, Fail, Game Over
- ⚪ **Abu-abu:** Disabled, Locked

### **Feedback Layers:**
1. **Visual:** Particle effects, color flash, animations
2. **Audio:** SFX click, success, fail
3. **Text:** Floating "+10", "Salah!", tooltips
4. **Haptic:** *(Future: Controller vibration)*

---

## 🧪 TESTING SCENARIOS

### **Functional Testing:**
- ✅ Klik sampah → Masuk inventory
- ✅ Drag sampah benar → +10 poin
- ✅ Drag sampah salah → -5 poin
- ✅ Skor < 0 → Game Over panel muncul
- ✅ Target tercapai → Win panel muncul
- ✅ Timer habis → Lose panel muncul
- ✅ Level selesai → Unlock level berikutnya
- ✅ Scene transition → Inventory tersimpan

### **Edge Cases:**
- ✅ Spam klik sampah (double click prevention)
- ✅ Drag sampah keluar area → Return ke posisi awal
- ✅ Scene tidak di Build Settings → Fallback ke Hub
- ✅ GameManager NULL → Error handling log
- ✅ Time.timeScale conflicts → Reset di scene transition

### **Performance Testing:**
- ✅ 30+ sampah spawn → Smooth FPS (target: 60 FPS)
- ✅ 10+ animations concurrent → No lag
- ✅ Scene load time → < 2 detik

---

## 🚀 FITUR FUTURE (Roadmap)

### **Phase 2 - Enhanced Gameplay:**
- [ ] **Adaptive Difficulty:** Auto-adjust spawn speed berdasarkan performa
- [ ] **Spaced Repetition:** Daily quest system (review materi lama)
- [ ] **Power-ups:** Slow time, hint, double points
- [ ] **Combo System:** Bonus poin untuk streak benar
- [ ] **Leaderboard:** Local/online ranking

### **Phase 3 - Social Features:**
- [ ] **Multiplayer Co-op:** 2 pemain, 1 layar
- [ ] **Class Competition:** Ranking per kelas (bukan individu)
- [ ] **Share Progress:** Screenshot achievement

### **Phase 4 - Content Expansion:**
- [ ] **Level 3:** Gudang (15 sampah, mixed categories)
- [ ] **Level 4:** TPA (Tempat Pembuangan Akhir) - Boss level
- [ ] **Mini-games:** Quiz, sorting quiz, memory game
- [ ] **Encyclopedia:** Database info sampah + cara daur ulang

### **Phase 5 - Analytics:**
- [ ] **Teacher Dashboard:** Track progress siswa
- [ ] **Heatmap:** Area yang paling sering salah
- [ ] **Engagement Metrics:** Time spent, replay rate

---

## 📈 METRICS KEBERHASILAN

### **Learning Outcomes:**
- **Pre-test vs Post-test:** Peningkatan skor pengetahuan pemilahan sampah (target: +30%)
- **Retention Rate:** Siswa masih ingat kategori 1 bulan kemudian (target: >70%)
- **Behavioral Change:** Survey guru - siswa pilah sampah di sekolah (target: +50%)

### **Engagement Metrics:**
- **Completion Rate:** Siswa selesaikan semua level (target: >80%)
- **Average Session Time:** 15-20 menit (optimal untuk attention span)
- **Replay Rate:** Ulangi level untuk perfect score (target: >40%)
- **NPS (Net Promoter Score):** "Apakah kamu akan rekomendasikan game ini?" (target: >8/10)

---

## 🛠️ TEKNOLOGI & TOOLS

| Kategori | Tool/Library | Purpose |
|----------|--------------|---------|
| **Engine** | Unity 2021.3 LTS | Game development |
| **Bahasa** | C# | Scripting |
| **UI Framework** | Unity UI (Canvas) | UI rendering |
| **Text** | TextMesh Pro | High-quality text |
| **Version Control** | Git + GitHub | Code repository |
| **Audio** | Unity Audio Mixer | Sound management |
| **Sprites** | Unity 2D Sprites | 2D graphics |
| **Animations** | Unity Animator | State machine animations |
| **Data** | ScriptableObject | Asset-based data |
| **Build** | Unity Build Pipeline | Executable creation |

---

## 👥 TARGET AUDIENCE

### **Primary Users:**
- **Siswa SD Kelas 4-6** (10-12 tahun)
- **Siswa SMP Kelas 7-8** (13-14 tahun)

### **Characteristics:**
- Familiar dengan game mobile/PC
- Attention span: 15-30 menit
- Learning style: Visual, kinesthetic
- Motivasi: Achievement, competition, fun

### **Secondary Users:**
- **Guru:** Monitor progress, integrate ke kurikulum
- **Orang Tua:** Edukasi di rumah

---

## 📝 KESIMPULAN

**Eco-Quest** adalah game edukasi yang menggabungkan **pembelajaran efektif** dengan **gameplay engaging**. Dengan menerapkan prinsip **pedagogis modern** (immediate feedback, scaffolding, mastery learning) dan **game design** yang solid (clear goals, balanced difficulty, reward system), game ini tidak hanya mengajarkan konsep pemilahan sampah, tetapi juga **menumbuhkan kesadaran lingkungan** sejak dini.

**Key Strengths:**
- ✅ Data-driven design (mudah dikustomisasi guru)
- ✅ Clear progression system (motivasi siswa)
- ✅ Comprehensive feedback (belajar dari kesalahan)
- ✅ Scalable architecture (mudah ditambah level baru)
- ✅ Cross-platform ready (Windows, Android future)

**Impact:**
Melalui game ini, diharapkan siswa tidak hanya **tahu** cara pilah sampah, tetapi juga **paham alasannya** dan **mau mempraktikkan** di kehidupan sehari-hari. Game menjadi **jembatan** antara pengetahuan teoritis dan aksi nyata.

---

## 📞 KONTAK & CREDITS

**Developer:** daffarobbani18  
**Repository:** [GitHub - Eco-Quest](https://github.com/daffarobbani18/eco-quest)  
**Platform:** Unity 2021.3 LTS  
**Lisensi:** [Lihat LICENSE file]

---

**Tanggal Laporan:** 6 Desember 2025  
**Versi Document:** 1.0  
**Status Project:** ✅ **PRODUCTION READY**

---

*"Educate to Act, Play to Change the World"* 🌍♻️
