# 🐛 ANALISIS BUG DAN DOKUMENTASI SETUP
## Game Edukasi Pemilahan Sampah - Eco Quest

---

## 📋 DAFTAR ISI
1. [Analisis Bug Scene 03_Game_Processing](#analisis-bug)
2. [Root Cause (Akar Masalah)](#root-cause)
3. [Solusi yang Diperlukan](#solusi)
4. [Setup Hierarchy untuk Scene 02_Game_Kantin](#setup-scene-02)
5. [Setup Hierarchy untuk Scene 03_Game_Processing](#setup-scene-03)
6. [Checklist Verifikasi](#checklist)
7. [Flow Diagram Transisi Scene](#flow-diagram)

---

## 🐛 ANALISIS BUG SCENE 03_GAME_PROCESSING {#analisis-bug}

### **Gejala Bug:**
- Scene 03_Game_Processing **dimuat** tetapi **tidak berjalan**
- Panel_DialogGuru (briefing) **tidak muncul**
- Tidak ada sampah yang spawn
- Game seperti "beku" atau tidak responsif

### **Skenario yang Terjadi:**
1. ✅ Pemain menyelesaikan Scene 02_Game_Kantin (mengumpulkan sampah)
2. ✅ Panel Berhasil muncul dengan tombol "Lanjut"
3. ✅ Pemain klik tombol "Lanjut" → `SceneManager.LoadScene("03_Game_Processing")` dipanggil
4. ✅ Scene 03_Game_Processing terbuka
5. ❌ **MASALAH DIMULAI DI SINI:**
   - Panel_DialogGuru tidak muncul
   - Briefing tidak jalan
   - Spawner tidak aktif
   - Game tidak bisa dimainkan

---

## 🔍 ROOT CAUSE (AKAR MASALAH) {#root-cause}

### **Masalah #1: Missing GameObject di Scene 03**
**Status:** ⚠️ **CRITICAL - PENYEBAB UTAMA**

Scene 03_Game_Processing **TIDAK memiliki** GameObject dengan script berikut:
- ❌ **GameManager** (yang harusnya persistent dari Scene 02)
- ❌ **ProcessingLevelManager**
- ❌ **WasteSpawner**
- ❌ **BriefingSequence**

**Hasil grep pada file `.unity`:**
```
03_Game_Processing.unity: Hanya 1 match untuk "ProcessingLevelManager"
02_Game_Kantin.unity: 2 match (GameManager + CollectionLevelManager)
```

**Kesimpulan:** Scene 03 hampir KOSONG dari sisi Manager Scripts!

---

### **Masalah #2: Referensi UI Tidak Tersambung**
Meskipun `ProcessingLevelManager.Start()` sudah benar secara kode, script ini **TIDAK AKAN JALAN** jika:

1. **GameObject "LevelManager" tidak ada di Hierarchy Scene 03**
   - Tanpa GameObject ini, script `ProcessingLevelManager` tidak akan dieksekusi sama sekali

2. **Referensi Inspector tidak diisi:**
   ```csharp
   [Header("Referensi Sistem")]
   public WasteSpawner mesinSpawner;        // ❌ NULL
   public BriefingSequence briefingScript;  // ❌ NULL
   
   [Header("UI Scene Ini")]
   public GameObject panelWinScene2;        // ❌ NULL
   public TMP_Text textSkorAkhirScene2;     // ❌ NULL
   public TMP_Text textWaktuAkhirScene2;    // ❌ NULL
   ```

3. **Panel_DialogGuru tidak tersambung ke BriefingSequence**

---

### **Masalah #3: GameManager Mungkin Tidak Persisten**
Cek file `GameManager.cs` baris 32-43:

```csharp
void Awake()
{
    if (Instance == null)
    {
        Instance = this;
        DontDestroyOnLoad(gameObject); // ✅ Sudah benar
        trashInventory = new List<WasteData>();
    }
    else
    {
        Destroy(gameObject); // ⚠️ Ini menghancurkan duplicate
    }
}
```

**Potensi Masalah:**
- Jika di Scene 02 ada **2 GameManager** (1 asli, 1 prefab), yang salah bisa ter-destroy
- Jika GameManager tidak di-DontDestroyOnLoad dengan benar, inventory hilang saat pindah scene

---

### **Masalah #4: Time.timeScale = 0 Tidak Ter-reset**
Di `CollectionLevelManager.cs` (Scene 02), saat pemain menang:
```csharp
void MisiSelesai()
{
    if (panelSelesai != null) panelSelesai.SetActive(true);
    // ⚠️ TIDAK ADA Time.timeScale = 0 di sini
}
```

**Namun** di `GameManager.cs` baris 157:
```csharp
void LevelSelesai()
{
    // ...
    Time.timeScale = 0; // ⚠️ WAKTU DIBEKUKAN
}
```

Jika ada logic yang memanggil `GameManager.LevelSelesai()` di Scene 02, maka:
- `Time.timeScale = 0` akan aktif
- Saat pindah ke Scene 03, waktu masih beku
- Meskipun `ProcessingLevelManager.Start()` memanggil `Time.timeScale = 1`, bisa jadi ada race condition

---

## ✅ SOLUSI YANG DIPERLUKAN {#solusi}

### **Solusi Utama:**

#### 1. **Setup Hierarchy Scene 03_Game_Processing dengan Lengkap**
   - Tambahkan semua GameObject Manager yang diperlukan
   - Sambungkan semua referensi UI di Inspector
   - Pastikan struktur sama seperti Scene 02 (tapi dengan script yang berbeda)

#### 2. **Pastikan GameManager Persistent**
   - Cek Scene 02: Hanya ada **1 GameManager** di Hierarchy
   - GameManager harus ada di Scene 02 (bukan di Scene 03)
   - Verifikasi `DontDestroyOnLoad` aktif

#### 3. **Tambahkan Null Check & Debug Log**
   - Di `ProcessingLevelManager.Start()` sudah ada, tapi perlu diperkuat
   - Tambahkan log untuk tracking UI references

#### 4. **Pastikan LevelData Terisi**
   - Buat ScriptableObject `LevelData` untuk Scene 03
   - Isi field `barisDialogSortir` dengan dialog fase pengolahan
   - Assign ke `ProcessingLevelManager.dataLevelIni` di Inspector

---

## 🎮 SETUP HIERARCHY SCENE 02_GAME_KANTIN {#setup-scene-02}

### **Struktur Hierarchy:**

```
Scene: 02_Game_Kantin
├── 📁 === MANAGERS ===
│   ├── 🎯 GameManager (Script: GameManager.cs) ⭐ SINGLETON PERSISTENT
│   │   └── [Inspector Settings]
│   │       ├── Level Pakai Timer: ☐ (false untuk scene kantin)
│   │       ├── Total Sampah Level Ini: 0 (akan diisi otomatis)
│   │       ├── Win Panel: (kosongkan dulu, nanti diisi oleh CollectionLevelManager)
│   │       └── Trash Inventory: (List otomatis, jangan diisi manual)
│   │
│   └── 🎯 LevelManager (Script: CollectionLevelManager.cs)
│       └── [Inspector Settings]
│           ├── Panel Selesai: → Link ke Canvas/Panel_Selesai
│           └── Data Level Ini: → Drag ScriptableObject "LevelData_Kantin"
│
├── 📁 === UI ===
│   └── Canvas
│       ├── Panel_DialogGuru (Prefab) ⭐ BRIEFING SYSTEM
│       │   └── (Script: BriefingSequence.cs) - sudah ada di Prefab
│       │
│       ├── Panel_Selesai
│       │   └── Button_Lanjut (OnClick → CollectionLevelManager.PindahKeSortir)
│       │
│       └── HUD
│           └── (UI skor, timer jika ada)
│
├── 📁 === GAMEPLAY ===
│   ├── Sampah_01 (Script: CollectionItem.cs)
│   ├── Sampah_02 (Script: CollectionItem.cs)
│   ├── Sampah_03 (Script: CollectionItem.cs)
│   └── ... (sampah lainnya)
│
└── 📁 === ENVIRONMENT ===
    └── (Background, dekorasi, dll)
```

---

### **Penjelasan GameObject Scene 02:**

#### **1. GameManager** ⭐ PALING PENTING
- **Fungsi:** Singleton yang dibawa ke semua scene
- **Script:** `GameManager.cs`
- **Inspector Setup:**
  ```
  Level Pakai Timer: [☐] FALSE (karena scene kantin tidak pakai timer)
  Total Sampah Level Ini: 0 (akan diisi otomatis)
  Win Panel: (KOSONGKAN - tidak dipakai di scene kantin)
  Score Text UI: (KOSONGKAN - tidak ada HUD skor di kantin)
  Timer Text UI: (KOSONGKAN)
  Trash Inventory: (BIARKAN KOSONG - akan terisi saat pemain klik sampah)
  ```

- **⚠️ PERINGATAN PENTING:**
  - Hanya boleh ada **1 GameManager** di Scene 02
  - Jangan letakkan GameManager di Scene 03 (akan otomatis terbawa dari Scene 02)
  - Tag GameObject ini sebagai "GameController" (opsional, untuk memudahkan debug)

---

#### **2. LevelManager (CollectionLevelManager)**
- **Fungsi:** Manager lokal Scene 02 (hanya hidup di scene kantin)
- **Script:** `CollectionLevelManager.cs`
- **Inspector Setup:**
  ```
  Panel Selesai: [Drag] Canvas/Panel_Selesai
  Data Level Ini: [Drag] LevelData_Kantin (ScriptableObject)
  ```

- **Cara Membuat LevelData:**
  1. Klik kanan di Project Window
  2. Create > PjBL > Level Data
  3. Beri nama: `LevelData_Kantin`
  4. Isi field:
     ```
     Nama Level: "Fase 1: Pengumpulan Sampah"
     Baris Dialog Guru: 
       - "Selamat datang di kantin sekolah!"
       - "Tugasmu adalah mengumpulkan sampah yang berserakan."
       - "Klik sampah untuk memasukkannya ke tas."
     Target Jumlah Sampah: 8 (atau sesuai jumlah sampah di scene)
     Batas Waktu Detik: 0 (tidak pakai timer)
     ```

---

#### **3. Canvas > Panel_DialogGuru (Prefab)**
- **Fungsi:** Menampilkan briefing/instruksi di awal level
- **Script:** `BriefingSequence.cs` (sudah ada di Prefab)
- **Struktur Prefab:**
  ```
  Panel_DialogGuru
  ├── Panel_Intro (Fade in dengan judul level)
  │   ├── Text_Judul
  │   └── Text_Info
  │
  └── Panel_Dialog (Dialog guru dengan next button)
      ├── Image_Guru
      ├── Text_Dialog
      ├── Button_Next
      └── Button_Mulai (muncul di akhir dialog)
  ```

- **Inspector Setup BriefingSequence (di Prefab):**
  ```
  Panel Intro: [Drag] Panel_Intro
  Text Judul: [Drag] Panel_Intro/Text_Judul
  Text Info: [Drag] Panel_Intro/Text_Info
  Durasi Animasi: 3
  
  Panel Dialog: [Drag] Panel_Dialog
  Text Dialog Isi: [Drag] Panel_Dialog/Text_Dialog
  Tombol Next: [Drag] Panel_Dialog/Button_Next
  Tombol Mulai: [Drag] Panel_Dialog/Button_Mulai
  ```

- **Setup Button_Mulai:**
  ```
  OnClick() → CollectionLevelManager.MulaiMain
  ```

---

#### **4. Panel_Selesai**
- **Fungsi:** Muncul saat pemain mengumpulkan semua sampah
- **Struktur:**
  ```
  Panel_Selesai
  ├── Text_Judul: "Berhasil!"
  ├── Text_Pesan: "Kamu telah mengumpulkan semua sampah!"
  └── Button_Lanjut
      └── OnClick: CollectionLevelManager.PindahKeSortir
  ```

---

## 🎮 SETUP HIERARCHY SCENE 03_GAME_PROCESSING {#setup-scene-03}

### **Struktur Hierarchy:**

```
Scene: 03_Game_Processing
├── 📁 === MANAGERS === ⚠️ CRITICAL SECTION
│   ├── 🎯 LevelManager (Script: ProcessingLevelManager.cs) ⭐ WAJIB ADA
│   │   └── [Inspector Settings]
│   │       ├── Mesin Spawner: → Link ke SpawnerManager
│   │       ├── Briefing Script: → Link ke Canvas/Panel_DialogGuru
│   │       ├── Data Level Ini: → Drag ScriptableObject "LevelData_Processing"
│   │       ├── Panel Win Scene2: → Link ke Canvas/Panel_Menang
│   │       ├── Text Skor Akhir Scene2: → Link ke Panel_Menang/Text_Skor
│   │       └── Text Waktu Akhir Scene2: → Link ke Panel_Menang/Text_Waktu
│   │
│   └── 🎯 SpawnerManager (Script: WasteSpawner.cs) ⭐ WAJIB ADA
│       └── [Inspector Settings]
│           ├── Prefab Sampah: → Drag Prefabs/Sampah_Draggable
│           ├── Titik Spawn: → Link ke Scene/TitikSpawn (Empty GameObject)
│           ├── Interval Spawn: 2.5
│           └── Daftar Sampah Test: (List - Isi 3-5 WasteData untuk testing)
│
├── 📁 === UI ===
│   └── Canvas
│       ├── Panel_DialogGuru (Prefab) ⭐ SAMA SEPERTI SCENE 02
│       │   └── (Script: BriefingSequence.cs)
│       │       └── [Inspector: Pastikan semua referensi tersambung]
│       │
│       ├── Panel_Menang (Win Panel untuk Scene 03)
│       │   ├── Text_Skor (TMP_Text)
│       │   ├── Text_Waktu (TMP_Text)
│       │   └── Button_Menu (Kembali ke main menu)
│       │
│       └── HUD
│           ├── Text_Skor (TMP_Text) ⭐ NAMA OBJEK HARUS PERSIS "Text_Skor"
│           └── Text_Timer (TMP_Text) ⭐ NAMA OBJEK HARUS PERSIS "Text_Timer"
│
├── 📁 === GAMEPLAY ===
│   ├── TitikSpawn (Empty GameObject) → Transform untuk spawn position
│   ├── Conveyor_Belt (Sprite)
│   └── Tempat_Sampah (Container)
│       ├── Kotak_Organik (Script: TrashBinTarget.cs)
│       ├── Kotak_Anorganik (Script: TrashBinTarget.cs)
│       └── Kotak_B3 (Script: TrashBinTarget.cs)
│
└── 📁 === ENVIRONMENT ===
    └── (Background, dekorasi, dll)
```

---

### **Penjelasan GameObject Scene 03:**

#### **1. LevelManager (ProcessingLevelManager)** ⭐ CRITICAL
- **Fungsi:** Koordinator utama Scene 03, menerima data dari GameManager
- **Script:** `ProcessingLevelManager.cs`
- **Inspector Setup (WAJIB DIISI SEMUA):**
  ```
  Mesin Spawner: [Drag] SpawnerManager (GameObject di hierarchy)
  Briefing Script: [Drag] Canvas/Panel_DialogGuru (GetComponent<BriefingSequence>)
  Data Level Ini: [Drag] LevelData_Processing (ScriptableObject)
  
  Panel Win Scene2: [Drag] Canvas/Panel_Menang
  Text Skor Akhir Scene2: [Drag] Canvas/Panel_Menang/Text_Skor
  Text Waktu Akhir Scene2: [Drag] Canvas/Panel_Menang/Text_Waktu
  ```

- **⚠️ KESALAHAN UMUM:**
  - Lupa drag SpawnerManager → Spawner tidak jalan
  - Lupa drag BriefingSequence → Briefing tidak muncul
  - LevelData tidak terisi → Crash atau skip briefing

---

#### **2. SpawnerManager (WasteSpawner)** ⭐ CRITICAL
- **Fungsi:** Men-spawn sampah dari inventory GameManager ke conveyor belt
- **Script:** `WasteSpawner.cs`
- **Inspector Setup:**
  ```
  Prefab Sampah: [Drag] Assets/Prefabs/Sampah_Draggable
  Titik Spawn: [Drag] TitikSpawn (GameObject di Hierarchy)
  Interval Spawn: 2.5 (detik)
  Daftar Sampah Test: (Isi 3-5 WasteData ScriptableObject)
     - [0] WasteData_BotolPlastik
     - [1] WasteData_KertasBekas
     - [2] WasteData_BateraiBekas
     - [3] WasteData_SisaMakanan
     - [4] WasteData_KalengSoda
  ```

- **Cara Membuat WasteData:**
  1. Klik kanan di Project Window
  2. Create > EcoQuest > Waste Data
  3. Isi field:
     ```
     Nama Sampah: "Botol Plastik"
     Tipe Sampah: Anorganik
     Icon Sampah: [Drag sprite botol]
     
     ── Scoring (Scene 03 - Pemilahan) ──
     Skor Benar: 10   (default, bisa diubah sesuai kebutuhan)
     Skor Salah: 5    (angka positif, akan otomatis jadi -5 di game)
     ```
  
  **Catatan Penting:**
  - `skorSalah` gunakan angka **positif** (misal: 5 untuk penalty -5 poin)
  - Field skor opsional, jika tidak diisi akan pakai default (10 dan 5)
  - Setiap sampah bisa punya nilai berbeda untuk game balancing

---

#### **3. Canvas > Panel_DialogGuru (Prefab)** - SAMA SEPERTI SCENE 02
- Pastikan Prefab yang sama digunakan
- Inspector BriefingSequence harus tersambung semua
- **PENTING:** Tombol "Mulai" di prefab ini akan di-override oleh `ProcessingLevelManager.Start()`
  ```csharp
  // Di ProcessingLevelManager.Start():
  briefingScript.tombolMulai.onClick.RemoveAllListeners();
  briefingScript.tombolMulai.onClick.AddListener(MulaiMain);
  ```

---

#### **4. HUD (Text_Skor & Text_Timer)** ⚠️ NAMA HARUS PERSIS
- **PENTING:** GameObject HARUS bernama **persis** seperti ini:
  ```
  Text_Skor  (bukan "TextSkor" atau "Text Skor")
  Text_Timer (bukan "TextTimer" atau "Text Timer")
  ```

- **Alasan:** GameManager mencari UI dengan `GameObject.Find()`:
  ```csharp
  // Di GameManager.SetupLevelBaru():
  GameObject objSkor = GameObject.Find("Text_Skor");
  GameObject objTimer = GameObject.Find("Text_Timer");
  ```

- **Setup:**
  ```
  Text_Skor:
    - Component: TextMeshPro - Text (UI)
    - Text: "Skor: 0"
    - Font Size: 24
    - Color: White
    - Alignment: Left
  
  Text_Timer:
    - Component: TextMeshPro - Text (UI)
    - Text: "00:00"
    - Font Size: 24
    - Color: White
    - Alignment: Right
  ```

---

#### **5. LevelData_Processing (ScriptableObject)**
- **Cara Membuat:**
  1. Klik kanan di Project: Create > PjBL > Level Data
  2. Nama: `LevelData_Processing`
  3. **ISI FIELD (PENTING):**
  ```
  Nama Level: "Fase 2: Pengolahan Sampah"
  
  Baris Dialog Guru: (KOSONGKAN - tidak dipakai di fase 2)
  
  Baris Dialog Sortir: (ISI INI - Khusus fase 2)
    - "Bagus! Kamu sudah mengumpulkan sampah."
    - "Sekarang, tugasmu adalah memilah sampah tersebut."
    - "Geser sampah ke kotak yang sesuai dengan jenisnya."
    - "Organik ke kotak hijau, Anorganik ke kotak kuning, B3 ke kotak merah."
    - "Hati-hati, salah pilih akan mengurangi skor!"
  
  Batas Waktu Detik: 60
  Target Jumlah Sampah: 0 (akan di-override oleh ProcessingLevelManager)
  
  Daftar Sampah Level Ini: (KOSONGKAN - akan diambil dari inventory GameManager)
  ```

---

## ✅ CHECKLIST VERIFIKASI {#checklist}

### **Scene 02_Game_Kantin:**
- [ ] Ada GameObject "GameManager" dengan script `GameManager.cs`
- [ ] GameManager.Awake() memanggil `DontDestroyOnLoad(gameObject)`
- [ ] Ada GameObject "LevelManager" dengan script `CollectionLevelManager.cs`
- [ ] CollectionLevelManager.dataLevelIni terisi (drag ScriptableObject)
- [ ] Ada Prefab "Panel_DialogGuru" di Canvas
- [ ] BriefingSequence.tombolMulai.OnClick → CollectionLevelManager.MulaiMain
- [ ] Ada Panel_Selesai dengan Button_Lanjut
- [ ] Button_Lanjut.OnClick → CollectionLevelManager.PindahKeSortir
- [ ] CollectionItem (sampah) script berjalan dan memanggil GameManager.AddTrashToInventory()

### **Scene 03_Game_Processing:**
- [ ] Ada GameObject "LevelManager" dengan script `ProcessingLevelManager.cs`
- [ ] ProcessingLevelManager.mesinSpawner terisi (drag SpawnerManager)
- [ ] ProcessingLevelManager.briefingScript terisi (drag Panel_DialogGuru)
- [ ] ProcessingLevelManager.dataLevelIni terisi (drag LevelData_Processing)
- [ ] ProcessingLevelManager.panelWinScene2 terisi (drag Panel_Menang)
- [ ] ProcessingLevelManager.textSkorAkhirScene2 terisi
- [ ] ProcessingLevelManager.textWaktuAkhirScene2 terisi
- [ ] Ada GameObject "SpawnerManager" dengan script `WasteSpawner.cs`
- [ ] WasteSpawner.prefabSampah terisi (drag Prefab sampah)
- [ ] WasteSpawner.titikSpawn terisi (drag TitikSpawn GameObject)
- [ ] WasteSpawner.daftarSampahTest terisi minimal 3 data (untuk testing)
- [ ] **PENTING:** Semua WasteData sudah punya nilai `skorBenar` dan `skorSalah` (cek di Inspector)
- [ ] Ada Prefab "Panel_DialogGuru" di Canvas (sama seperti Scene 02)
- [ ] LevelData_Processing.barisDialogSortir terisi (minimal 3 kalimat)
- [ ] LevelData_Processing.barisDialogSortir **TIDAK NULL** (jika tidak ada dialog, buat array kosong [] bukan null)
- [ ] Ada GameObject bernama "Text_Skor" (NAMA HARUS PERSIS)
- [ ] Ada GameObject bernama "Text_Timer" (NAMA HARUS PERSIS)
- [ ] **TIDAK ADA** GameObject "GameManager" di Scene 03 (akan terbawa dari Scene 02)

### **Testing:**
- [ ] Play Scene 02 dari awal
- [ ] Kumpulkan semua sampah
- [ ] Panel Selesai muncul
- [ ] Klik tombol "Lanjut"
- [ ] Scene 03 terbuka
- [ ] Panel_DialogGuru muncul dengan dialog sortir
- [ ] Setelah dialog selesai, tombol "Mulai" muncul
- [ ] Klik "Mulai" → Game dimulai, timer jalan
- [ ] Sampah mulai spawn dari conveyor belt
- [ ] Drag sampah ke kotak, skor bertambah
- [ ] Semua sampah selesai → Panel Menang muncul

---

## 📊 FLOW DIAGRAM TRANSISI SCENE {#flow-diagram}

```
╔═══════════════════════════════════════════════════════════════╗
║                    SCENE 02: KANTIN                           ║
╚═══════════════════════════════════════════════════════════════╝

[Scene Load]
    ↓
┌─────────────────────────┐
│ GameManager.Awake()     │ ← Singleton + DontDestroyOnLoad
│ - Instance = this       │
│ - trashInventory = []   │
└──────────┬──────────────┘
           ↓
┌─────────────────────────┐
│ CollectionLevelManager. │
│ Start()                 │
│ - Setup Briefing        │
│ - isGamePlaying = false │
└──────────┬──────────────┘
           ↓
┌─────────────────────────┐
│ BriefingSequence.Start()│ ← Auto detect CollectionLevelManager
│ - Tampilkan Intro       │
│ - Tampilkan Dialog Guru │
└──────────┬──────────────┘
           ↓
   [User klik "Mulai"]
           ↓
┌─────────────────────────┐
│ CollectionLevelManager. │
│ MulaiMain()             │
│ - isGamePlaying = true  │
└──────────┬──────────────┘
           ↓
   [User klik Sampah]
           ↓
┌─────────────────────────┐
│ CollectionItem.OnClick()│
│ - GameManager.Instance. │
│   AddTrashToInventory() │ ← PENTING: Inventory terisi di sini
└──────────┬──────────────┘
           ↓
   [Semua sampah terkumpul]
           ↓
┌─────────────────────────┐
│ CollectionLevelManager. │
│ MisiSelesai()           │
│ - Panel Selesai muncul  │
└──────────┬──────────────┘
           ↓
   [User klik "Lanjut"]
           ↓
┌─────────────────────────┐
│ CollectionLevelManager. │
│ PindahKeSortir()        │
│ - LoadScene("03_...")   │ ← Scene transition
└──────────┬──────────────┘
           ↓
           ║
           ║  ⚡ TRANSITION ⚡
           ║  GameManager TERBAWA (DontDestroyOnLoad)
           ║  trashInventory MASIH ADA ISINYA
           ║
           ↓

╔═══════════════════════════════════════════════════════════════╗
║                 SCENE 03: PROCESSING                          ║
╚═══════════════════════════════════════════════════════════════╝

[Scene Load]
    ↓
┌─────────────────────────┐
│ ProcessingLevelManager. │
│ Awake()                 │
│ - Instance = this       │
└──────────┬──────────────┘
           ↓
┌─────────────────────────┐
│ ProcessingLevelManager. │
│ Start() - IEnumerator   │ ⭐ CRITICAL POINT
│                         │
│ [1] Wait 0.1s           │ ← Tunggu GameManager landing
│ [2] Time.timeScale = 1  │ ← Reset freeze dari Scene 02
│                         │
│ [3] Cek GameManager     │
│     ✅ Instance != null │
│                         │
│ [4] Hitung target:      │
│     count = GameManager │
│     .trashInventory.    │
│     Count               │ ← Ambil data dari Scene 02
│                         │
│ [5] Setup GameManager:  │
│     SetupLevelBaru(     │
│       timer: true,      │
│       target: count,    │
│       durasi: 60,       │
│       panelWin: ...,    │
│       txtSkor: ...,     │
│       txtWaktu: ...     │
│     )                   │
└──────────┬──────────────┘
           ↓
┌─────────────────────────┐
│ GameManager.            │
│ SetupLevelBaru()        │ ⭐ SINKRONISASI STATE
│                         │
│ - Update level settings │
│ - Link UI baru          │
│ - Find("Text_Skor")     │ ← Cari UI HUD otomatis
│ - Find("Text_Timer")    │
│ - Time.timeScale = 1    │
│ - isGameActive = false  │
└──────────┬──────────────┘
           ↓
┌─────────────────────────┐
│ ProcessingLevelManager. │
│ Start() [lanjutan]      │
│                         │
│ [6] Matikan Spawner     │
│ [7] Setup Briefing:     │
│     briefingScript.     │
│     SetupSequenceKhusus │
│     (dataLevelIni,      │
│      barisDialogSortir) │
│                         │
│ [8] Override tombol:    │
│     tombolMulai.onClick │
│     = MulaiMain         │
│                         │
│ [9] Time.timeScale = 0  │ ← Freeze untuk briefing
└──────────┬──────────────┘
           ↓
┌─────────────────────────┐
│ BriefingSequence.       │
│ SetupSequenceKhusus()   │ ⭐ FUNGSI KHUSUS FASE 2
│                         │
│ - textJudul = "FASE     │
│   PENGOLAHAN"           │
│ - barisKalimat =        │
│   dialogKhusus          │ ← Pakai barisDialogSortir
│ - StartCoroutine(       │
│   MainkanIntro())       │
└──────────┬──────────────┘
           ↓
   [Intro Animation 3s]
           ↓
   [Dialog Guru Sequence]
           ↓
   [Tombol "Mulai" muncul]
           ↓
   [User klik "Mulai"]
           ↓
┌─────────────────────────┐
│ ProcessingLevelManager. │
│ MulaiMain()             │ ⭐ GAME START
│                         │
│ - Time.timeScale = 1    │
│ - Hide briefing panels  │
│ - Enable WasteSpawner   │
│ - GameManager.Instance. │
│   MulaiLevel()          │ ← Start timer
└──────────┬──────────────┘
           ↓
┌─────────────────────────┐
│ WasteSpawner.Start()    │
│                         │
│ IF GameManager.Instance │
│    .trashInventory      │
│    .Count > 0:          │
│                         │
│   daftarSampahFinal =   │
│   trashInventory        │ ← Pakai data dari Kantin
│                         │
│ ELSE:                   │
│   daftarSampahFinal =   │
│   daftarSampahTest      │ ← Pakai data test
└──────────┬──────────────┘
           ↓
┌─────────────────────────┐
│ WasteSpawner.Update()   │
│                         │
│ - timer += deltaTime    │
│ - IF timer >= interval: │
│   SpawnSampah()         │ ← Spawn sampah dari list
└──────────┬──────────────┘
           ↓
   [Game berjalan normal]
           ↓
   [Semua sampah selesai]
           ↓
┌─────────────────────────┐
│ GameManager.            │
│ KurangiJumlahSampah()   │ ← Dipanggil saat sampah benar
│                         │
│ IF totalSampahLevelIni  │
│    <= 0:                │
│   LevelSelesai()        │
└──────────┬──────────────┘
           ↓
┌─────────────────────────┐
│ GameManager.            │
│ LevelSelesai()          │
│                         │
│ - isGameActive = false  │
│ - winPanel muncul       │
│ - Update skor & waktu   │
│ - Time.timeScale = 0    │
└─────────────────────────┘
```

---

## 🔧 TROUBLESHOOTING TAMBAHAN

### **Debug Log yang Harus Muncul (Jika Setup Benar):**

**Scene 02 (Kantin):**
```
[BRIEFING] Mode Kantin Terdeteksi (CollectionLevelManager ada).
GAME DIMULAI! (Pemain sekarang bisa klik sampah)
(Saat klik sampah) → Tidak ada log khusus, tapi inventory bertambah
```

**Scene 03 (Processing):**
```
==================================================
[1] ProcessingLevelManager: Menunggu GameManager siap...
[2] GameManager Ditemukan. Melakukan Setup Level Baru...
GameManager: Setup Level Baru dimulai...
[3] Memulai Briefing...
[BRIEFING] SetupSequenceKhusus dipanggil.
[BRIEFING] Mainkan Intro...
[BRIEFING] Tombol Mulai Dimunculkan.
(Saat klik Mulai:)
[GAME START] Game Dimulai.
Spawner: Menggunakan Data dari Inventaris Pemain (Fase 1).
(Timer mulai jalan, sampah mulai spawn)

(Saat drag sampah ke tong:)
BENAR! Botol Plastik masuk ke tong yang pas. +10 poin  ← Skor dinamis dari WasteData
Sisa Sampah Target: 7

SALAH! Sisa Nasi jangan dibuang di sini! -3 poin  ← Penalty dinamis dari WasteData
```

### **Jika Tidak Ada Log:**
1. **Tidak ada log sama sekali di Scene 03:**
   → ProcessingLevelManager tidak ada atau disabled

2. **Log berhenti di "[1] ProcessingLevelManager...":**
   → GameManager tidak ditemukan (tidak persistent dari Scene 02)

3. **Log berhenti di "[3] Memulai Briefing...":**
   → BriefingSequence tidak tersambung atau null

4. **Spawner log: "Tidak ada data inventaris/GameManager":**
   → GameManager.trashInventory kosong (pemain tidak kumpulkan sampah di Scene 02)

---

## 📝 LANGKAH IMPLEMENTASI (STEP BY STEP)

### **Fase 1: Perbaiki Scene 02 (Jika Belum Benar)**

1. Buka Scene 02_Game_Kantin
2. Cek Hierarchy, pastikan ada GameObject "GameManager"
3. Select GameManager, cek Inspector:
   - Script: GameManager.cs ✅
   - Trash Inventory: List (biarkan kosong) ✅
4. Cek GameObject "LevelManager":
   - Script: CollectionLevelManager.cs ✅
   - Data Level Ini: Drag LevelData_Kantin ✅
   - Panel Selesai: Drag Canvas/Panel_Selesai ✅
5. Play test: Kumpulkan sampah → Panel Selesai muncul ✅

### **Fase 2: Setup Scene 03 dari Nol**

1. Buka Scene 03_Game_Processing

2. **Buat GameObject Manager:**
   - Hierarchy > Klik kanan > Create Empty
   - Nama: "LevelManager"
   - Add Component > ProcessingLevelManager
   - **JANGAN** create GameManager (akan otomatis terbawa dari Scene 02)

3. **Buat GameObject Spawner:**
   - Hierarchy > Klik kanan > Create Empty
   - Nama: "SpawnerManager"
   - Add Component > WasteSpawner

4. **Buat TitikSpawn:**
   - Hierarchy > Klik kanan > Create Empty
   - Nama: "TitikSpawn"
   - Position: (sesuai posisi conveyor belt, misal X:0, Y:2, Z:0)

5. **Drag Prefab Panel_DialogGuru ke Canvas:**
   - Dari Project Window: Assets/Prefabs/UI/Panel_DialogGuru
   - Drag ke Canvas di Hierarchy

6. **Buat Panel Menang:**
   - Canvas > Klik kanan > UI > Panel
   - Nama: "Panel_Menang"
   - Tambahkan 2 TextMeshPro: "Text_Skor" dan "Text_Waktu"

7. **Buat HUD:**
   - Canvas > Klik kanan > UI > TextMeshPro
   - Nama: "Text_Skor" (PERSIS, tanpa spasi/underscore salah)
   - Ulangi untuk "Text_Timer"

8. **Setup Inspector LevelManager:**
   - Mesin Spawner: Drag "SpawnerManager"
   - Briefing Script: Drag "Panel_DialogGuru"
   - Data Level Ini: Drag "LevelData_Processing" (buat dulu jika belum ada)
   - Panel Win Scene2: Drag "Panel_Menang"
   - Text Skor/Waktu: Drag dari Panel_Menang

9. **Setup Inspector SpawnerManager:**
   - Prefab Sampah: Drag dari Assets/Prefabs
   - Titik Spawn: Drag "TitikSpawn"
   - Interval: 2.5
   - Daftar Sampah Test: Add 3-5 WasteData

10. **Save Scene!**

### **Fase 2.5: Update Semua WasteData (PENTING SETELAH PERBAIKAN SCRIPT)**

⚠️ **WAJIB DILAKUKAN** setelah perbaikan script!

1. **Buka folder WasteData:**
   - Di Project Window: `Assets/_Scripts/Gameplay/Data/` (atau folder tempat WasteData Anda)

2. **Update setiap WasteData:**
   - Klik file `WasteData_xxx.asset`
   - Lihat Inspector, akan ada section baru:
   ```
   ── Scoring (Scene 03 - Pemilahan) ──
   Skor Benar: 10
   Skor Salah: 5
   ```

3. **Isi nilai sesuai desain game:**
   
   **Contoh Easy (Organik biasa):**
   ```
   WasteData_SisaNasi:
     Skor Benar: 5
     Skor Salah: 3
   ```
   
   **Contoh Medium (Anorganik standar):**
   ```
   WasteData_BotolPlastik:
     Skor Benar: 10
     Skor Salah: 5
   ```
   
   **Contoh Hard (B3 berbahaya):**
   ```
   WasteData_Baterai:
     Skor Benar: 20
     Skor Salah: 15
   ```

4. **Save Project:**
   - Ctrl + S atau File > Save Project

5. **Verifikasi:**
   - Buka WasteSpawner di Inspector
   - Cek `Daftar Sampah Test` → Expand tiap item
   - Pastikan skorBenar dan skorSalah terisi (tidak 0)

**Catatan:**
- Jika tidak diisi, akan pakai default: skorBenar=10, skorSalah=5
- Nilai skorSalah adalah **positif** (misal: tulis 5, di game jadi -5)
- Tooltip akan muncul saat hover di Inspector untuk panduan

---

### **Fase 3: Testing Akhir**

1. **Test dari Scene 02:**
   - Play Scene 02
   - Kumpulkan semua sampah
   - Klik "Lanjut"
   - ✅ Scene 03 load
   - ✅ Briefing muncul
   - ✅ Game berjalan

2. **Test langsung di Scene 03 (Testing Mode):**
   - Play Scene 03 langsung
   - Briefing harus muncul (pakai data test)
   - Spawner pakai daftarSampahTest

---

## ⚠️ KESALAHAN UMUM & SOLUSINYA

| Masalah | Penyebab | Solusi |
|---------|----------|--------|
| Panel_DialogGuru tidak muncul | BriefingSequence tidak tersambung ke LevelManager | Drag Panel_DialogGuru ke field `briefingScript` di Inspector |
| Sampah tidak spawn | WasteSpawner disabled atau tidak ada di scene | Cek GameObject SpawnerManager ada dan enabled |
| Timer tidak jalan | Text_Skor/Text_Timer nama salah atau null | Rename GameObject jadi **persis** "Text_Skor" dan "Text_Timer" |
| GameManager tidak persisten | DontDestroyOnLoad tidak jalan atau ada 2 GameManager | Pastikan hanya 1 GameManager di Scene 02, cek Awake() |
| Inventory kosong di Scene 03 | Pemain tidak klik sampah atau AddTrashToInventory tidak dipanggil | Cek CollectionItem.OnClick() memanggil GameManager.AddTrashToInventory() |
| Game freeze di Scene 03 | Time.timeScale = 0 tidak ter-reset | Cek ProcessingLevelManager.Start() panggil Time.timeScale = 1 |
| Skor tidak bertambah/berkurang | WasteData.skorBenar/skorSalah = 0 atau null | Buka WasteData di Inspector, isi field Skor Benar dan Skor Salah |
| Debug log error "NullReferenceException" di Scene 03 | barisDialogSortir = null di LevelData | Buka LevelData_Processing, pastikan array barisDialogSortir minimal [] kosong, bukan null |
| Semua sampah punya skor sama | Lupa update WasteData setelah perbaikan script | Follow Fase 2.5 untuk update semua WasteData |

---

## 📞 SUPPORT & CREDITS

---

## 🔄 PERUBAHAN SETELAH PERBAIKAN SCRIPT (Update: 4 Desember 2025)

### **📝 Summary Perbaikan:**

Setelah analisis mendalam, telah dilakukan perbaikan pada **8 masalah** (4 CRITICAL, 3 HIGH, 1 MEDIUM):

#### **✅ CRITICAL Fixes (Sudah Diperbaiki):**
1. ✅ **BinController.cs** - Ganti `CollectionItem` → `WasteItem`
2. ✅ **DragController.cs** - Tambah `KurangiJumlahSampah()` saat benar
3. ✅ **CollectionItem.cs** - Hapus `KurangiJumlahSampah()` yang salah tempat
4. ✅ **DragController.cs** - Tambah null check untuk GameManager

#### **✅ HIGH Fixes (Sudah Diperbaiki):**
5. ✅ **WasteData.cs** - Tambah field `skorBenar` dan `skorSalah`
6. ✅ **DragController.cs** - Pakai skor dinamis dari WasteData
7. ✅ **BinController.cs** - Pakai skor dinamis dari WasteData

#### **✅ MEDIUM Fixes (Sudah Diperbaiki):**
8. ✅ **ProcessingLevelManager.cs** - Tambah null check `barisDialogSortir`

---

### **⚠️ YANG HARUS DILAKUKAN SETELAH PERBAIKAN:**

#### **1. Update Semua WasteData (WAJIB!):**

Karena ada penambahan field baru di `WasteData.cs`, Anda **HARUS** update semua file WasteData yang sudah ada:

**Langkah:**
1. Buka folder: `Assets/_Scripts/Gameplay/Data/`
2. Klik setiap file `WasteData_xxx.asset`
3. Di Inspector, akan muncul section baru:
   ```
   ── Scoring (Scene 03 - Pemilahan) ──
   Skor Benar: 10
   Skor Salah: 5
   ```
4. Isi nilai sesuai tingkat kesulitan sampah
5. Save Project (Ctrl + S)

**Default Values:**
- Jika tidak diisi, otomatis pakai: `skorBenar = 10`, `skorSalah = 5`
- Backward compatible dengan WasteData lama

**Contoh Isian:**
```
Sampah Mudah (Organik):
  skorBenar = 5
  skorSalah = 3

Sampah Standar (Anorganik):
  skorBenar = 10
  skorSalah = 5

Sampah Sulit (B3):
  skorBenar = 20
  skorSalah = 15
```

---

#### **2. Verifikasi LevelData_Processing:**

Pastikan `barisDialogSortir` **tidak NULL**:

**Cara Cek:**
1. Buka `LevelData_Processing.asset`
2. Lihat field `Baris Dialog Sortir`
3. **Jika kosong:** Klik ikon list, tambah minimal 1 element (bisa string kosong)
4. **Jangan biarkan:** Array = None (null) ❌
5. **Harus:** Array = [] atau Array dengan isi ✅

**Alasan:**
- Setelah perbaikan, ada null check di `ProcessingLevelManager`
- Jika array null, briefing akan di-skip (langsung main)
- Ini fitur, bukan bug - untuk testing tanpa briefing

---

### **📊 Perbandingan Setup Sebelum vs Sesudah:**

| Aspek | Sebelum Perbaikan | Sesudah Perbaikan |
|-------|-------------------|-------------------|
| **WasteData Fields** | Hanya nama, tipe, icon | + skorBenar, skorSalah |
| **Scoring System** | Hardcode (10, -5) | Dinamis per sampah |
| **BinController** | Cari CollectionItem ❌ | Cari WasteItem ✅ |
| **Counter Sampah** | Berkurang di Scene 02 ❌ | Berkurang di Scene 03 ✅ |
| **Null Safety** | Tidak ada | Ada di semua akses GameManager |
| **barisDialogSortir** | Bisa crash jika null | Aman dengan null check |
| **Debug Log** | "BENAR!" / "SALAH!" | "BENAR! +10 poin" / "SALAH! -5 poin" |

---

### **🧪 Testing Checklist Tambahan:**

Setelah perbaikan script, tambahkan test case ini:

- [ ] **Test Skor Dinamis:**
  - Buat 3 WasteData dengan skor berbeda (5, 10, 20)
  - Buang ke tong benar → Cek skor sesuai WasteData
  - Buang ke tong salah → Cek penalty sesuai WasteData

- [ ] **Test Null Safety:**
  - Buka Scene 03 langsung (tanpa GameManager dari Scene 02)
  - Pastikan tidak crash, muncul log error tapi game jalan

- [ ] **Test Null barisDialogSortir:**
  - Set `barisDialogSortir = null` di LevelData_Processing
  - Play Scene 03 → Harus langsung main tanpa briefing

- [ ] **Test Backward Compatibility:**
  - Pakai WasteData lama (belum ada field skor)
  - Pastikan otomatis pakai default (10 dan 5)

---

### **📚 Dokumentasi Lengkap Perbaikan:**

Untuk detail lengkap setiap perbaikan, lihat file:

1. **`PERBAIKAN_CRITICAL_BUGS.md`** - Detail perbaikan CRITICAL (4 masalah)
2. **`PERBAIKAN_HIGH_MEDIUM.md`** - Detail perbaikan HIGH & MEDIUM (4 masalah)
3. **File ini** - Setup hierarchy & workflow

---

### **🎯 Status Akhir:**

| Kategori | Status | Keterangan |
|----------|--------|------------|
| **Script Fixes** | ✅ 100% Done | 8/8 masalah diperbaiki |
| **Documentation** | ✅ Up to Date | Dokumentasi sudah disesuaikan |
| **Testing Required** | ⏳ Pending | Butuh testing dari developer |
| **WasteData Update** | ⚠️ Action Needed | Harus update manual di Inspector |
| **Hierarchy Setup** | ⏳ Pending | Follow guide di dokumentasi ini |

---

### **🚀 Next Action Items:**

**Untuk Developer:**
1. ✅ Read dokumentasi perbaikan (done)
2. ⏳ Update semua WasteData di Inspector
3. ⏳ Setup Hierarchy Scene 03 (follow guide di atas)
4. ⏳ Testing gameplay Scene 02 → Scene 03
5. ⏳ Fix hierarchy jika ada yang kurang

**Untuk Game Designer:**
1. ⏳ Review nilai skor tiap WasteData
2. ⏳ Balance difficulty per level
3. ⏳ Tulis dialog untuk `barisDialogSortir`
4. ⏳ Playtesting & feedback

**Untuk QA:**
1. ⏳ Test semua test case di checklist
2. ⏳ Test edge cases (null, empty, etc)
3. ⏳ Report bug jika masih ada

---

**Developer:** daffarobbani18  
**Project:** Eco-Quest - Game Edukasi Pemilahan Sampah  
**Unity Version:** (sesuai ProjectVersion.txt)  
**Last Updated:** December 4, 2025  
**Documentation Version:** 2.0 (Updated after script fixes)

---

**🎮 Selamat Coding! Semoga dokumentasi ini membantu menyelesaikan bug dan mempermudah development!**
