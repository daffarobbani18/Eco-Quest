# 🎮 TAHAP 2: SETUP SCENE 02 (KANTIN)
## Tutorial Setup Unity Eco-Quest

⏱️ **Estimasi Waktu:** 30 menit  
🎯 **Tujuan:** Memastikan Scene Kantin (pengumpulan sampah) bekerja dengan benar

---

## 📋 CHECKLIST TAHAP INI

- [ ] Setup GameManager (Singleton)
- [ ] Buat/verifikasi LevelData_Kantin
- [ ] Setup CollectionLevelManager
- [ ] Setup Panel_DialogGuru
- [ ] Setup Panel Selesai
- [ ] Test Scene 02 standalone

---

## 🔧 LANGKAH 1: BUKA SCENE 02

### **1.1. Load Scene:**
```
Di Project Window:
Assets → _Scenes → 02_Game_Kantin.unity

Double-click untuk membuka
```

### **1.2. Lihat Hierarchy:**
```
Cek apakah sudah ada:
- GameManager (GameObject)
- LevelManager (GameObject)
- Canvas
  - Panel_DialogGuru
  - Panel_Selesai (atau nama lain untuk win panel)
  - HUD (opsional untuk scene ini)
```

**❗ Catatan:**
- Jika sudah ada GameManager dan LevelManager → Lanjut ke verifikasi
- Jika belum ada → Ikuti langkah pembuatan di bawah

---

## 🔧 LANGKAH 2: SETUP GAMEMANAGER (SINGLETON)

### **Mengapa GameManager Penting?**
- **Singleton** yang persisten di semua scene
- Menyimpan **inventory sampah** dari Scene 02 ke Scene 03
- Mengelola **skor, timer, dan state game**

---

### **2.1. Cek Apakah GameManager Sudah Ada:**

**Di Hierarchy Scene 02, cari GameObject bernama "GameManager"**

**Kasus A: GameManager SUDAH ADA**
```
✅ Skip ke bagian 2.3 (Verifikasi Inspector)
```

**Kasus B: GameManager BELUM ADA**
```
⚠️ Lanjut ke 2.2 (Membuat GameObject Baru)
```

---

### **2.2. Membuat GameManager Baru:**

**Step 1: Create GameObject**
```
1. Di Hierarchy, klik kanan di area kosong
2. Pilih: Create Empty
3. Rename menjadi: "GameManager" (tanpa tanda kutip)
```

**Step 2: Add Script**
```
1. Pastikan GameManager selected di Hierarchy
2. Lihat Inspector window
3. Klik tombol "Add Component" di bagian bawah Inspector
4. Ketik: "GameManager" (akan muncul autocomplete)
5. Klik script "Game Manager" untuk menambahkannya
```

**Visual Guide:**
```
Inspector saat ini:
┌─────────────────────────┐
│ 🔷 GameManager          │  ← Nama GameObject
├─────────────────────────┤
│ ✅ Transform            │
│    Position: (0, 0, 0)  │
│    Rotation: (0, 0, 0)  │
│    Scale: (1, 1, 1)     │
├─────────────────────────┤
│ ✅ Game Manager (Script)│  ← Script baru ditambahkan
│    (Field-field...)     │
├─────────────────────────┤
│ 🔘 Add Component        │  ← Tombol ini dipakai tadi
└─────────────────────────┘
```

---

### **2.3. Setup Inspector GameManager:**

**Select GameManager di Hierarchy, lalu di Inspector isi field berikut:**

#### **Section: Level Settings**
```
Level Pakai Timer: ☐ UNCHECK (Scene 02 tidak pakai timer)

Alasan: Di scene kantin, pemain bebas mengumpulkan sampah tanpa batas waktu
```

#### **Section: Win Panel (KOSONGKAN untuk Scene 02)**
```
Win Panel: [Kosong/None]

Alasan: Scene 02 tidak pakai GameManager.LevelSelesai(),
        Win panel dikelola langsung oleh CollectionLevelManager
```

#### **Section: UI References (KOSONGKAN untuk Scene 02)**
```
Score Text UI: [Kosong/None]
Timer Text UI: [Kosong/None]

Alasan: Scene 02 tidak menampilkan HUD skor/timer
```

#### **Section: Trash Inventory (BIARKAN KOSONG)**
```
Trash Inventory: Size: 0

⚠️ JANGAN ISI MANUAL!
List ini akan otomatis terisi saat pemain klik sampah
```

---

### **2.4. Verifikasi DontDestroyOnLoad:**

**Pastikan script GameManager punya kode ini:**

```csharp
// Di file GameManager.cs, fungsi Awake()
void Awake()
{
    if (Instance == null)
    {
        Instance = this;
        DontDestroyOnLoad(gameObject); // ⭐ INI PENTING!
        trashInventory = new List<WasteData>();
    }
    else
    {
        Destroy(gameObject);
    }
}
```

**Cara Cek:**
```
1. Di Inspector, double-click script "Game Manager"
2. File GameManager.cs akan terbuka di code editor
3. Cari fungsi Awake()
4. Pastikan ada baris: DontDestroyOnLoad(gameObject);
```

**✅ Jika ADA:** Bagus! GameManager akan persisten  
**❌ Jika TIDAK ADA:** Script salah, restore dari backup

---

### **2.5. Testing GameManager:**

**Test Apakah Singleton Bekerja:**

```
1. Play Scene 02 (tekan tombol Play ▶️)
2. Buka Hierarchy saat Play Mode
3. Lihat di root Hierarchy (paling atas):
   
   Harus muncul GameObject: "GameManager (DontDestroyOnLoad)"
   
4. Stop Play Mode

✅ Jika muncul teks "DontDestroyOnLoad": Singleton bekerja!
❌ Jika tidak muncul: Ada masalah di Awake()
```

**⚠️ PENTING:**
- Hanya boleh ada **1 GameManager** di Scene 02
- Jangan tambahkan GameManager di Scene 03 (akan otomatis terbawa)

---

## 🔧 LANGKAH 3: BUAT LEVELDATA_KANTIN

### **Apa itu LevelData?**
ScriptableObject yang menyimpan data level:
- Nama level
- Dialog briefing
- Target sampah
- Batas waktu

---

### **3.1. Create ScriptableObject:**

```
1. Di Project Window, navigate ke folder Data:
   Assets → _Scripts → Gameplay → Data
   
   (Atau folder lain tempat Anda simpan ScriptableObject)

2. Klik kanan di area kosong Project Window
3. Pilih: Create → PjBL → Level Data
   
   ⚠️ Jika "PjBL" tidak muncul:
   - Coba: Create → Scriptable Objects → Level Data
   - Atau cek apakah ada [CreateAssetMenu] di LevelData.cs

4. Rename file menjadi: "LevelData_Kantin"
```

**Visual Guide:**
```
Project Window:
Assets/_Scripts/Gameplay/Data/
├── WasteData.cs
├── LevelData.cs
├── LevelData_Kantin.asset  ⭐ FILE BARU INI
└── (WasteData lainnya...)
```

---

### **3.2. Isi Field LevelData_Kantin:**

**Select LevelData_Kantin.asset di Project Window, lalu lihat Inspector:**

#### **Nama Level:**
```
Nama Level: "Fase 1: Pengumpulan Sampah"
```

#### **Baris Dialog Guru (untuk Briefing):**
```
Baris Dialog Guru:
  Size: 4  ⬅️ Klik ini untuk set jumlah dialog

  Element 0: "Selamat datang di kantin sekolah!"
  Element 1: "Hari ini kita akan belajar tentang pengelolaan sampah."
  Element 2: "Tugasmu adalah mengumpulkan semua sampah yang berserakan."
  Element 3: "Klik sampah untuk memasukkannya ke dalam tas. Selamat mencoba!"
```

**💡 Tips Menulis Dialog:**
- Pendek dan jelas (1-2 kalimat per element)
- Gunakan bahasa yang ramah anak
- Total 3-5 dialog sudah cukup

---

#### **Baris Dialog Sortir (KOSONGKAN):**
```
Baris Dialog Sortir:
  Size: 0

Alasan: Field ini hanya dipakai di Scene 03 (Processing)
```

---

#### **Batas Waktu Detik:**
```
Batas Waktu Detik: 0

Alasan: Scene 02 tidak pakai timer (pemain bebas explore)
```

---

#### **Target Jumlah Sampah:**
```
Target Jumlah Sampah: 8

⚠️ GANTI ANGKA INI sesuai jumlah sampah di scene Anda!

Cara hitung:
1. Di Hierarchy Scene 02, cari semua GameObject sampah
2. Filter dengan script "CollectionItem"
3. Hitung manual atau pakai search
```

**Cara Hitung Otomatis:**
```
1. Di Hierarchy, klik search bar
2. Ketik: "t:CollectionItem"
3. Lihat jumlah hasil di bawah search bar
4. Isi angka tersebut di Target Jumlah Sampah
```

---

#### **Daftar Sampah Level Ini (KOSONGKAN):**
```
Daftar Sampah Level Ini:
  Size: 0

Alasan: Scene 02 tidak pakai list ini (sampah sudah manual di scene)
```

---

### **3.3. Save LevelData:**

```
1. Setelah isi semua field
2. Tekan: Ctrl + S (untuk save)
3. Atau klik di tempat lain untuk auto-save
```

**✅ Verifikasi:**
- LevelData_Kantin.asset icon berubah (tidak abu-abu)
- Semua field sudah terisi sesuai instruksi

---

## 🔧 LANGKAH 4: SETUP COLLECTIONLEVELMANAGER

### **4.1. Cek Apakah LevelManager Sudah Ada:**

**Di Hierarchy Scene 02, cari GameObject bernama "LevelManager"**

**Kasus A: LevelManager SUDAH ADA**
```
✅ Skip ke bagian 4.3 (Setup Inspector)
```

**Kasus B: LevelManager BELUM ADA**
```
⚠️ Lanjut ke 4.2 (Membuat GameObject Baru)
```

---

### **4.2. Membuat LevelManager Baru:**

```
1. Di Hierarchy, klik kanan di area kosong
2. Pilih: Create Empty
3. Rename menjadi: "LevelManager"
4. Pastikan LevelManager selected
5. Klik "Add Component" di Inspector
6. Ketik: "CollectionLevelManager"
7. Klik script untuk menambahkannya
```

**⚠️ PERHATIAN:**
- Jangan pakai script "ProcessingLevelManager" di Scene 02!
- Scene 02 pakai: **CollectionLevelManager**
- Scene 03 pakai: **ProcessingLevelManager**

---

### **4.3. Setup Inspector CollectionLevelManager:**

**Select LevelManager di Hierarchy, lalu di Inspector:**

#### **Panel Selesai (Win Panel):**
```
1. Lihat field "Panel Selesai" di Inspector
2. Klik icon lingkaran di sebelah kanan field
3. Window "Select GameObject" muncul
4. Ketik: "Panel_Selesai" (atau nama panel win Anda)
5. Double-click panel tersebut untuk link
```

**Cara Manual (Drag & Drop):**
```
1. Di Hierarchy, cari Canvas → Panel_Selesai
2. Drag Panel_Selesai ke field "Panel Selesai" di Inspector
```

**❗ Jika Panel_Selesai Belum Ada:**
```
Lanjut ke LANGKAH 6 untuk membuatnya dulu
Kemudian kembali ke sini untuk link
```

---

#### **Data Level Ini:**
```
1. Lihat field "Data Level Ini" di Inspector
2. Klik icon lingkaran di sebelah kanan field
3. Window "Select LevelData" muncul
4. Ketik: "LevelData_Kantin"
5. Double-click untuk link
```

**Cara Manual (Drag & Drop):**
```
1. Di Project Window, cari LevelData_Kantin.asset
2. Drag ke field "Data Level Ini" di Inspector
```

---

### **4.4. Verifikasi Inspector Lengkap:**

**Inspector LevelManager harus terlihat seperti ini:**

```
┌─────────────────────────────────────┐
│ 🔷 LevelManager                     │
├─────────────────────────────────────┤
│ ✅ Collection Level Manager (Script)│
│                                     │
│ Panel Selesai:                      │
│   🔗 Panel_Selesai                  │  ← Harus terisi
│                                     │
│ Data Level Ini:                     │
│   📄 LevelData_Kantin               │  ← Harus terisi
└─────────────────────────────────────┘
```

**✅ Checklist:**
- [ ] Panel Selesai: Linked (ada icon + nama)
- [ ] Data Level Ini: Linked (ada icon + nama)
- [ ] Tidak ada tulisan "None" atau "Missing"

---

## 🔧 LANGKAH 5: SETUP PANEL_DIALOGGURU (BRIEFING)

### **5.1. Tambahkan Prefab ke Canvas:**

**Jika Panel_DialogGuru BELUM ada di Hierarchy:**

```
1. Di Project Window, cari Prefab:
   Assets → Prefabs → UI → Panel_DialogGuru.prefab

2. Drag prefab tersebut ke Canvas di Hierarchy
   
   Hierarchy setelah drag:
   Canvas
   ├── Panel_DialogGuru  ⭐ BARU DITAMBAHKAN
   ├── Panel_Selesai
   └── (UI lainnya...)

3. Select Panel_DialogGuru di Hierarchy
4. Di Inspector, pastikan active: ☑ (checked)
```

**Jika Panel_DialogGuru SUDAH ada:**
```
✅ Skip ke 5.2
```

---

### **5.2. Verifikasi Struktur Prefab:**

**Select Panel_DialogGuru di Hierarchy, lalu expand (klik arrow):**

```
Panel_DialogGuru
├── Panel_Intro
│   ├── Text_Judul (TMP_Text)
│   └── Text_Info (TMP_Text)
│
└── Panel_Dialog
    ├── Image_Guru (Image)
    ├── Text_Dialog (TMP_Text)
    ├── Button_Next (Button)
    └── Button_Mulai (Button)
```

**✅ Jika struktur sama:** Bagus!  
**❌ Jika ada yang hilang:** Restore prefab dari backup

---

### **5.3. Verifikasi Script BriefingSequence:**

```
1. Select Panel_DialogGuru (root, bukan child)
2. Lihat Inspector, harus ada: BriefingSequence (Script)
3. Pastikan semua field terisi:

   Panel Intro: 🔗 Panel_Intro
   Text Judul: 🔗 Text_Judul
   Text Info: 🔗 Text_Info
   Durasi Animasi: 3
   
   Panel Dialog: 🔗 Panel_Dialog
   Text Dialog Isi: 🔗 Text_Dialog
   Tombol Next: 🔗 Button_Next
   Tombol Mulai: 🔗 Button_Mulai
```

**❗ Jika Ada Field NULL:**
```
1. Cari child object yang sesuai di Hierarchy
2. Drag ke field yang kosong
3. Klik "Apply" di bagian atas Inspector (jika prefab)
```

---

### **5.4. Setup Button_Mulai Event:**

**Tombol ini akan trigger game start:**

```
1. Di Hierarchy, expand: Panel_DialogGuru → Panel_Dialog
2. Select "Button_Mulai"
3. Di Inspector, scroll ke component "Button"
4. Lihat section "On Click ()"
5. Pastikan ada event:
   
   On Click ()
   ├── Runtime
   └── 🔗 LevelManager.MulaiMain
```

**Jika Event KOSONG atau SALAH:**

```
1. Klik tombol "+" di kanan bawah "On Click ()"
2. Drag GameObject "LevelManager" dari Hierarchy ke box event
3. Klik dropdown "No Function"
4. Pilih: CollectionLevelManager → MulaiMain ()
```

**Visual Guide:**
```
On Click () Event:
┌────────────────────────────────┐
│ List is Empty                  │  ← SEBELUM
│ [+] [-]                        │
└────────────────────────────────┘

Setelah setup:
┌────────────────────────────────┐
│ Runtime                        │
│ 🔗 LevelManager                │  ← Drag LevelManager kesini
│ 🔽 CollectionLevelManager      │  ← Pilih dari dropdown
│    MulaiMain ()                │  ← Pilih fungsi ini
│ [+] [-]                        │
└────────────────────────────────┘
```

**✅ Verifikasi:**
- Event "LevelManager.MulaiMain" muncul
- Runtime mode (bukan Editor)

---

## 🔧 LANGKAH 6: SETUP PANEL SELESAI (WIN PANEL)

### **6.1. Cek Apakah Panel Sudah Ada:**

**Di Hierarchy, cari: Canvas → Panel_Selesai (atau nama sejenis)**

**Kasus A: Panel SUDAH ADA**
```
✅ Skip ke 6.3 (Setup Button)
```

**Kasus B: Panel BELUM ADA**
```
⚠️ Lanjut ke 6.2 (Membuat Panel Baru)
```

---

### **6.2. Membuat Panel Selesai Baru:**

**Step 1: Create Panel**
```
1. Select Canvas di Hierarchy
2. Klik kanan Canvas
3. Pilih: UI → Panel
4. Rename menjadi: "Panel_Selesai"
```

**Step 2: Tambah Text Judul**
```
1. Klik kanan Panel_Selesai
2. Pilih: UI → Text - TextMeshPro
3. Rename: "Text_Judul"
4. Di Inspector, isi Text: "Berhasil!"
5. Ubah Font Size: 48
6. Ubah Alignment: Center
7. Ubah Color: Hijau atau warna cerah
```

**Step 3: Tambah Text Pesan**
```
1. Klik kanan Panel_Selesai
2. Pilih: UI → Text - TextMeshPro
3. Rename: "Text_Pesan"
4. Isi Text: "Kamu telah mengumpulkan semua sampah!"
5. Font Size: 24
6. Alignment: Center
```

**Step 4: Tambah Button Lanjut**
```
1. Klik kanan Panel_Selesai
2. Pilih: UI → Button - TextMeshPro
3. Rename: "Button_Lanjut"
4. Select child "Text (TMP)"
5. Ubah text menjadi: "Lanjut ke Pemilahan"
```

**Step 5: Posisikan UI**
```
Atur position semua element agar terlihat bagus:

Panel_Selesai: (Full screen atau center)
Text_Judul: (Atas, center)
Text_Pesan: (Tengah, center)
Button_Lanjut: (Bawah, center)
```

**Step 6: Set Panel Inactive**
```
1. Select Panel_Selesai
2. Di Inspector, UNCHECK checkbox di samping nama
   ☐ Panel_Selesai  ← Harus unchecked

Alasan: Panel hanya muncul saat level selesai
```

---

### **6.3. Setup Button_Lanjut Event:**

**Button ini akan load Scene 03:**

```
1. Select Button_Lanjut di Hierarchy
2. Di Inspector, scroll ke component "Button (Script)"
3. Lihat section "On Click ()"
4. Klik tombol "+"
5. Drag GameObject "LevelManager" ke box event
6. Klik dropdown "No Function"
7. Pilih: CollectionLevelManager → PindahKeSortir ()
```

**✅ Verifikasi:**
```
On Click () harus ada event:
🔗 LevelManager.PindahKeSortir
```

---

### **6.4. Link Panel ke LevelManager:**

**Sekarang sambungkan Panel_Selesai ke LevelManager:**

```
1. Select LevelManager di Hierarchy
2. Di Inspector (CollectionLevelManager script)
3. Lihat field "Panel Selesai"
4. Drag Panel_Selesai dari Hierarchy ke field tersebut
```

**✅ Verifikasi:**
- Field "Panel Selesai" terisi: Panel_Selesai
- Tidak ada tulisan "None" atau "Missing"

---

## 🔧 LANGKAH 7: VERIFIKASI SAMPAH (COLLECTIONITEM)

### **7.1. Cek Sampah di Scene:**

```
1. Di Hierarchy, cari semua GameObject sampah
2. Biasanya di folder/group tertentu
3. Atau gunakan search: "t:CollectionItem"
```

**Contoh Struktur:**
```
Hierarchy:
├── GameObjects_Sampah
│   ├── Sampah_BotolPlastik (Script: CollectionItem)
│   ├── Sampah_KertasBekas (Script: CollectionItem)
│   ├── Sampah_SisaNasi (Script: CollectionItem)
│   └── ... (sampah lainnya)
```

---

### **7.2. Verifikasi Script CollectionItem:**

**Select salah satu sampah, lihat Inspector:**

```
┌─────────────────────────────────────┐
│ 🔷 Sampah_BotolPlastik              │
├─────────────────────────────────────┤
│ ✅ Collection Item (Script)         │
│                                     │
│ Data Sampah:                        │
│   📄 WasteData_BotolPlastik         │  ← Harus terisi
│                                     │
│ Sprite Sampah:                      │
│   🖼️ sprite_botol                  │  ← Harus terisi
│                                     │
│ Level Manager:                      │
│   🔗 LevelManager                   │  ← Harus terisi
└─────────────────────────────────────┘
```

**✅ Checklist setiap sampah:**
- [ ] Data Sampah: Linked ke WasteData
- [ ] Sprite Sampah: Linked ke sprite visual
- [ ] Level Manager: Linked ke LevelManager GameObject

---

### **7.3. Link LevelManager ke Sampah (Jika Kosong):**

**Jika field "Level Manager" NULL di CollectionItem:**

**Cara Otomatis (Recommended):**
```csharp
1. Buat script temporary: "LinkAllSampah.cs"
2. Paste kode ini:

using UnityEngine;

public class LinkAllSampah : MonoBehaviour
{
    void Start()
    {
        // Cari LevelManager
        CollectionLevelManager lm = FindObjectOfType<CollectionLevelManager>();
        
        // Cari semua CollectionItem
        CollectionItem[] allSampah = FindObjectsOfType<CollectionItem>();
        
        // Link semua
        foreach (CollectionItem sampah in allSampah)
        {
            sampah.levelManager = lm;
            Debug.Log($"Linked {sampah.name} ke LevelManager");
        }
        
        Debug.Log($"Total {allSampah.Length} sampah di-link!");
    }
}

3. Attach script ini ke GameObject kosong
4. Play Scene (akan otomatis link semua)
5. Stop Play
6. Hapus script temporary
```

**Cara Manual:**
```
1. Select setiap sampah satu per satu
2. Drag LevelManager ke field "Level Manager"
3. Ulangi untuk semua sampah
```

---

### **7.4. Verifikasi Jumlah Sampah:**

```
1. Hitung total sampah di scene (gunakan search "t:CollectionItem")
2. Buka LevelData_Kantin
3. Pastikan "Target Jumlah Sampah" = jumlah yang dihitung
4. Jika beda, update LevelData
```

**Contoh:**
```
Jumlah sampah di scene: 8
Target Jumlah Sampah di LevelData_Kantin: 8  ✅ COCOK
```

---

## 🔧 LANGKAH 8: SAVE SCENE

### **8.1. Save Semua Changes:**

```
1. Tekan: Ctrl + S
2. Atau: File → Save Scene
3. Pastikan tidak ada tanda asterisk (*) di tab Scene
```

---

### **8.2. Save Project:**

```
1. Tekan: Ctrl + Shift + S
2. Atau: File → Save Project
3. Tunggu hingga loading bar selesai
```

---

## 🧪 LANGKAH 9: TESTING SCENE 02

### **9.1. Play Test:**

```
1. Pastikan Scene 02_Game_Kantin terbuka
2. Tekan tombol Play ▶️ (atau F5)
3. Tunggu scene dimuat
```

---

### **9.2. Test Sequence:**

**Test 1: Briefing Muncul**
```
✅ Panel_Intro muncul dengan fade in
✅ Text_Judul: "FASE PENGUMPULAN"
✅ Text_Info: "Fase 1: Pengumpulan Sampah"
✅ Setelah 3 detik, pindah ke Panel_Dialog
```

**Test 2: Dialog Guru**
```
✅ Dialog pertama muncul
✅ Tombol "Next" visible
✅ Klik Next → Dialog berganti
✅ Dialog terakhir → Tombol "Mulai" muncul
```

**Test 3: Gameplay**
```
✅ Klik tombol "Mulai" → Panel briefing hilang
✅ Klik sampah → Sampah terbang ke atas (animasi MasukKeTas)
✅ Sampah hilang setelah animasi
✅ Ulangi hingga semua sampah terkumpul
```

**Test 4: Win Condition**
```
✅ Setelah sampah terakhir diklik → Panel_Selesai muncul
✅ Text: "Berhasil!"
✅ Tombol "Lanjut" visible dan clickable
```

**Test 5: Scene Transition (JANGAN KLIK DULU)**
```
⚠️ JANGAN klik "Lanjut" sekarang!
Alasan: Scene 03 belum di-setup (akan dilakukan di TAHAP 4)

Untuk sekarang, cukup verifikasi Panel_Selesai muncul ✅
```

---

### **9.3. Cek Console Log:**

**Log yang HARUS muncul:**

```
[BRIEFING] Mode Kantin Terdeteksi (CollectionLevelManager ada).
GAME DIMULAI! (Pemain sekarang bisa klik sampah)

(Saat semua sampah terkumpul)
Misi Selesai! Kamu berhasil mengumpulkan semua sampah.
```

**❌ Jika Log TIDAK muncul:**
```
1. Cek BriefingSequence.Start() dipanggil
2. Cek CollectionLevelManager.MulaiMain() dipanggil
3. Lihat troubleshooting di bawah
```

---

### **9.4. Stop Play Mode:**

```
1. Tekan tombol Stop ⏹️ (atau F5)
2. Tunggu hingga Editor kembali ke Edit Mode
3. Jangan lupa save jika ada perubahan
```

---

## ✅ CHECKLIST AKHIR TAHAP 2

### **GameObject & Script:**
- [ ] Ada GameObject "GameManager" dengan script GameManager.cs
- [ ] GameManager DontDestroyOnLoad aktif (tested)
- [ ] Ada GameObject "LevelManager" dengan script CollectionLevelManager.cs
- [ ] LevelData_Kantin.asset dibuat dan terisi lengkap

### **UI Setup:**
- [ ] Panel_DialogGuru ada di Canvas (prefab)
- [ ] BriefingSequence script tersambung lengkap
- [ ] Button_Mulai event: LevelManager.MulaiMain
- [ ] Panel_Selesai dibuat dan inactive by default
- [ ] Button_Lanjut event: LevelManager.PindahKeSortir

### **Sampah Setup:**
- [ ] Semua sampah punya script CollectionItem
- [ ] Setiap sampah linked ke WasteData
- [ ] Setiap sampah linked ke LevelManager
- [ ] Jumlah sampah = Target di LevelData_Kantin

### **Testing:**
- [ ] Briefing muncul dan berjalan
- [ ] Tombol "Mulai" berfungsi
- [ ] Sampah bisa diklik dan masuk tas
- [ ] Panel_Selesai muncul saat semua sampah terkumpul
- [ ] Console log sesuai ekspektasi

---

## 🚨 TROUBLESHOOTING

### **Problem: Briefing Tidak Muncul**
**Penyebab:**
- Panel_DialogGuru tidak active
- BriefingSequence script missing
- LevelData_Kantin tidak terisi

**Solusi:**
1. Cek Panel_DialogGuru active di Inspector
2. Cek LevelManager.dataLevelIni linked
3. Play test ulang

---

### **Problem: Tombol "Mulai" Tidak Berfungsi**
**Penyebab:**
- Event Button_Mulai tidak di-setup
- LevelManager tidak linked

**Solusi:**
1. Select Button_Mulai
2. Setup On Click () event ke LevelManager.MulaiMain
3. Test ulang

---

### **Problem: Sampah Tidak Bisa Diklik**
**Penyebab:**
- CollectionItem.isGamePlaying = false
- LevelManager tidak call MulaiMain()
- Collider sampah missing

**Solusi:**
1. Pastikan klik "Mulai" dulu sebelum klik sampah
2. Cek Console log "GAME DIMULAI!" muncul
3. Cek setiap sampah punya Collider2D

---

### **Problem: Panel_Selesai Tidak Muncul**
**Penyebab:**
- Panel tidak linked ke LevelManager
- Target sampah tidak match jumlah sebenarnya

**Solusi:**
1. Cek field "Panel Selesai" di LevelManager terisi
2. Hitung ulang sampah, update LevelData_Kantin
3. Test ulang

---

### **Problem: GameManager Ganda di DontDestroyOnLoad**
**Penyebab:**
- Ada 2 GameManager di scene
- Singleton logic error

**Solusi:**
1. Stop Play Mode
2. Di Hierarchy Scene 02, cari semua "GameManager"
3. Hapus yang duplikat (biarkan hanya 1)
4. Play ulang

---

## ⏭️ LANGKAH SELANJUTNYA

**Jika SEMUA checklist ✅:**
- ✅ Scene 02 sudah siap!
- ✅ Lanjut ke **TAHAP 3: Update WasteData (Skor Dinamis)**
- 📄 Buka file: `TUTORIAL_TAHAP_3_WasteData.md`

**Jika Ada yang ❌:**
- ⚠️ Selesaikan dulu masalahnya
- 🔄 Ulangi testing
- 📞 Lihat Troubleshooting atau minta bantuan

---

**🎉 Selamat! TAHAP 2 Selesai!**

**Next:** TAHAP 3 - Update WasteData & Sistem Skor Dinamis

---

**Developer:** daffarobbani18  
**Project:** Eco-Quest  
**Last Updated:** December 4, 2025
