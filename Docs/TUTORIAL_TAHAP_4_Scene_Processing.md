# 🎯 TAHAP 4: SETUP SCENE 03 (PROCESSING)
## Tutorial Setup Unity Eco-Quest - TAHAP TERPENTING!

⏱️ **Estimasi Waktu:** 45 menit  
🎯 **Tujuan:** Membuat Scene Processing dari awal hingga berfungsi penuh

⚠️ **PENTING:** Ini adalah tahap yang paling krusial! Ikuti dengan teliti.

---

## 📋 CHECKLIST TAHAP INI

- [ ] Buat LevelData_Processing
- [ ] Setup GameObject LevelManager (ProcessingLevelManager)
- [ ] Setup GameObject SpawnerManager (WasteSpawner)
- [ ] Buat TitikSpawn untuk spawn position
- [ ] Setup Panel_DialogGuru (Prefab)
- [ ] Buat HUD (Text_Skor & Text_Timer)
- [ ] Buat Panel Menang (Win Panel)
- [ ] Setup tempat sampah (bin targets)
- [ ] Test Scene 03 standalone
- [ ] Test transisi Scene 02 → 03

---

## 🔧 LANGKAH 1: BUKA SCENE 03

### **1.1. Load Scene:**

```
Di Project Window:
Assets → _Scenes → 03_Game_Processing.unity

Double-click untuk membuka
```

---

### **1.2. Lihat Hierarchy Saat Ini:**

```
Cek apakah sudah ada:
- Canvas (untuk UI)
- Camera
- (Mungkin ada GameObject lain)

⚠️ Yang TIDAK BOLEH ADA:
- GameManager (akan otomatis terbawa dari Scene 02)
```

**❗ Jika Ada GameManager di Scene 03:**
```
DELETE GameManager tersebut!
Alasan: Akan conflict dengan GameManager dari Scene 02
```

---

## 🔧 LANGKAH 2: BUAT LEVELDATA_PROCESSING

### **2.1. Create ScriptableObject:**

```
1. Di Project Window, navigate ke:
   Assets → _Scripts → Gameplay → Data

2. Klik kanan di area kosong
3. Pilih: Create → EcoQuest → Waste Data
   atau: Create → PjBL → Level Data

4. Rename: "LevelData_Processing"
```

---

### **2.2. Isi Field LevelData_Processing:**

**Select LevelData_Processing di Project, lihat Inspector:**

#### **Nama Level:**
```
Nama Level: "Fase 2: Pengolahan Sampah"
```

---

#### **Baris Dialog Guru (KOSONGKAN):**
```
Baris Dialog Guru:
  Size: 0

Alasan: Scene 03 tidak pakai dialog biasa, 
        pakai "Baris Dialog Sortir" khusus
```

---

#### **Baris Dialog Sortir (ISI INI - PENTING!):**
```
Baris Dialog Sortir:
  Size: 5  ⬅️ Klik untuk set jumlah

  Element 0: "Bagus! Kamu sudah mengumpulkan sampah."
  Element 1: "Sekarang, tugasmu adalah memilah sampah tersebut."
  Element 2: "Geser sampah ke kotak yang sesuai dengan jenisnya."
  Element 3: "Organik ke kotak hijau, Anorganik ke kotak kuning, B3 ke kotak merah."
  Element 4: "Hati-hati, salah pilih akan mengurangi skor! Selamat mencoba!"
```

**💡 Tips Dialog:**
- Jelaskan mekanik drag & drop
- Sebutkan warna kotak per tipe sampah
- Ingatkan ada konsekuensi salah pilih

**⚠️ SANGAT PENTING:**
```
Jika barisDialogSortir NULL atau tidak diisi:
- Briefing akan di-skip
- Game langsung mulai tanpa instruksi
- Pemain bingung cara main

Minimal isi 3-5 dialog!
```

---

#### **Batas Waktu Detik:**
```
Batas Waktu Detik: 60

💡 Rekomendasi waktu:
- Easy: 90 detik
- Medium: 60 detik
- Hard: 45 detik

Sesuaikan dengan jumlah sampah di inventory
```

---

#### **Target Jumlah Sampah:**
```
Target Jumlah Sampah: 0

Alasan: Akan di-override otomatis oleh ProcessingLevelManager
        dengan jumlah sampah dari inventory Scene 02
```

---

#### **Daftar Sampah Level Ini (KOSONGKAN):**
```
Daftar Sampah Level Ini:
  Size: 0

Alasan: Sampah akan diambil dari GameManager.trashInventory
```

---

### **2.3. Save LevelData:**

```
Ctrl + S atau klik di tempat lain untuk auto-save
```

---

## 🔧 LANGKAH 3: BUAT LEVELMANAGER (PROCESSLEVELMANAGER)

### **3.1. Create GameObject:**

```
1. Di Hierarchy Scene 03, klik kanan di area kosong
2. Pilih: Create Empty
3. Rename: "LevelManager"
```

---

### **3.2. Add Component ProcessingLevelManager:**

```
1. Pastikan LevelManager selected
2. Klik "Add Component" di Inspector
3. Ketik: "ProcessingLevelManager"
4. Klik script untuk menambahkannya
```

**⚠️ PERHATIAN:**
```
JANGAN pakai CollectionLevelManager!
Scene 03 harus pakai: ProcessingLevelManager
```

---

### **3.3. Setup Inspector (SEBAGIAN - Lengkap Nanti):**

**Untuk sekarang, isi yang bisa diisi:**

#### **Data Level Ini:**
```
1. Lihat field "Data Level Ini"
2. Drag LevelData_Processing dari Project Window
```

**Visual:**
```
┌─────────────────────────────────────┐
│ 🔷 LevelManager                     │
├─────────────────────────────────────┤
│ ✅ Processing Level Manager (Script)│
│                                     │
│ Mesin Spawner: [None]               │  ⬅️ Isi nanti (LANGKAH 4)
│ Briefing Script: [None]             │  ⬅️ Isi nanti (LANGKAH 6)
│ Data Level Ini:                     │
│   📄 LevelData_Processing           │  ✅ ISI SEKARANG
│                                     │
│ Panel Win Scene2: [None]            │  ⬅️ Isi nanti (LANGKAH 8)
│ Text Skor Akhir Scene2: [None]      │  ⬅️ Isi nanti (LANGKAH 8)
│ Text Waktu Akhir Scene2: [None]     │  ⬅️ Isi nanti (LANGKAH 8)
└─────────────────────────────────────┘
```

**❗ Catatan:**
```
Field lain akan diisi setelah GameObject terkait dibuat
Jangan khawatir jika masih ada [None], akan diisi bertahap
```

---

## 🔧 LANGKAH 4: BUAT SPAWNERMANAGER (WASTESPAWNER)

### **4.1. Create GameObject:**

```
1. Di Hierarchy Scene 03, klik kanan
2. Pilih: Create Empty
3. Rename: "SpawnerManager"
```

---

### **4.2. Add Component WasteSpawner:**

```
1. Select SpawnerManager
2. Klik "Add Component"
3. Ketik: "WasteSpawner"
4. Klik script untuk add
```

---

### **4.3. Setup Inspector WasteSpawner:**

**Select SpawnerManager, lihat Inspector:**

#### **Prefab Sampah:**
```
1. Di Project Window, cari prefab sampah draggable
   Lokasi kemungkinan:
   - Assets/Prefabs/Sampah_Draggable.prefab
   - Assets/Prefabs/Gameplay/Sampah.prefab
   - Atau nama lain sesuai project Anda

2. Drag prefab tersebut ke field "Prefab Sampah"
```

**❗ Jika Prefab Tidak Ada:**
```
⚠️ Anda perlu membuat prefab sampah draggable dulu:

1. Buat GameObject baru di scene: "Sampah_Prefab"
2. Add Component:
   - Sprite Renderer (untuk visual)
   - Box Collider 2D atau Circle Collider 2D
   - Rigidbody 2D (Body Type: Kinematic)
   - WasteItem (script data sampah)
   - DragController (script drag & drop)

3. Setup WasteItem:
   - Data Sampah: [akan di-set runtime oleh spawner]

4. Setup DragController:
   - (Tidak perlu isi apa-apa, otomatis)

5. Drag GameObject ke Project Window untuk jadi Prefab
6. Hapus GameObject dari scene (prefab sudah tersimpan)
7. Drag prefab ke field "Prefab Sampah" di WasteSpawner
```

---

#### **Titik Spawn:**
```
⚠️ Belum diisi sekarang
Akan dibuat di LANGKAH 5
```

---

#### **Interval Spawn:**
```
Interval Spawn: 2.5

💡 Rekomendasi:
- Fast: 2.0 detik (challenging)
- Medium: 2.5 detik (balanced)
- Slow: 3.0 detik (casual)
```

---

#### **Daftar Sampah Test:**
```
Daftar Sampah Test:
  Size: 5  ⬅️ Set jumlah untuk testing

  Element 0: [Drag WasteData_SisaNasi]
  Element 1: [Drag WasteData_BotolPlastik]
  Element 2: [Drag WasteData_BateraiBekas]
  Element 3: [Drag WasteData_KertasBekas]
  Element 4: [Drag WasteData_KulitPisang]
```

**Fungsi:**
```
List ini dipakai jika:
- Test Scene 03 langsung (tidak dari Scene 02)
- GameManager.trashInventory kosong

Saat play dari Scene 02 → Scene 03:
- List ini TIDAK dipakai
- Pakai inventory dari GameManager (sampah yang dikumpulkan)
```

**💡 Tips:**
```
Isi dengan variasi sampah (Organik, Anorganik, B3)
Untuk testing game balance
```

---

## 🔧 LANGKAH 5: BUAT TITIKSPAWN

### **5.1. Create Empty GameObject:**

```
1. Di Hierarchy Scene 03, klik kanan
2. Pilih: Create Empty
3. Rename: "TitikSpawn"
```

---

### **5.2. Atur Position:**

```
Select TitikSpawn, lihat Inspector → Transform:

Position:
  X: 0 (atau sesuai posisi conveyor belt Anda)
  Y: 2 (di atas conveyor, agar sampah jatuh ke conveyor)
  Z: 0

Rotation: (0, 0, 0)
Scale: (1, 1, 1)
```

**Visual Guide:**
```
        🔽 TitikSpawn (Y: 2)
        
    ════════════════════════
    ║  Conveyor Belt      ║ (Y: 0)
    ════════════════════════
    
    ┌─────┐  ┌─────┐  ┌─────┐
    │ ORG │  │ ANG │  │ B3  │ (Y: -2)
    └─────┘  └─────┘  └─────┘
```

**💡 Tips:**
```
Atur position sambil lihat Scene View
Pastikan sampah spawn di lokasi yang visible
Tidak terlalu tinggi (lama jatuh) atau terlalu rendah (langsung tertimpa)
```

---

### **5.3. Link TitikSpawn ke WasteSpawner:**

```
1. Select SpawnerManager di Hierarchy
2. Lihat Inspector → WasteSpawner script
3. Lihat field "Titik Spawn"
4. Drag GameObject "TitikSpawn" ke field tersebut
```

**Verifikasi:**
```
┌─────────────────────────────────────┐
│ 🔷 SpawnerManager                   │
├─────────────────────────────────────┤
│ ✅ Waste Spawner (Script)           │
│                                     │
│ Prefab Sampah: 🔗 Sampah_Draggable  │  ✅
│ Titik Spawn: 🔗 TitikSpawn          │  ✅ BARU DILINK
│ Interval Spawn: 2.5                 │  ✅
│ Daftar Sampah Test: (5 items)      │  ✅
└─────────────────────────────────────┘
```

---

### **5.4. Link SpawnerManager ke LevelManager:**

```
1. Select LevelManager di Hierarchy
2. Lihat Inspector → ProcessingLevelManager script
3. Lihat field "Mesin Spawner"
4. Drag GameObject "SpawnerManager" ke field tersebut
```

**Verifikasi:**
```
┌─────────────────────────────────────┐
│ 🔷 LevelManager                     │
├─────────────────────────────────────┤
│ ✅ Processing Level Manager (Script)│
│                                     │
│ Mesin Spawner: 🔗 SpawnerManager    │  ✅ BARU DILINK
│ Briefing Script: [None]             │  ⬅️ Nanti (LANGKAH 6)
│ Data Level Ini: 📄 LevelData_...    │  ✅
│ ...                                 │
└─────────────────────────────────────┘
```

---

## 🔧 LANGKAH 6: SETUP PANEL_DIALOGGURU

### **6.1. Tambah Prefab ke Canvas:**

```
1. Di Project Window, cari:
   Assets → Prefabs → UI → Panel_DialogGuru.prefab

2. Drag prefab ke Canvas di Hierarchy

Hierarchy setelah drag:
Canvas
├── Panel_DialogGuru  ⭐ BARU DITAMBAHKAN
└── (UI lainnya...)
```

---

### **6.2. Verifikasi Prefab:**

**Select Panel_DialogGuru, expand di Hierarchy:**

```
Panel_DialogGuru
├── Panel_Intro
│   ├── Text_Judul
│   └── Text_Info
└── Panel_Dialog
    ├── Image_Guru
    ├── Text_Dialog
    ├── Button_Next
    └── Button_Mulai
```

**✅ Jika struktur sama:** OK!  
**❌ Jika beda:** Restore prefab dari backup atau TAHAP 2

---

### **6.3. Verifikasi Script BriefingSequence:**

```
1. Select Panel_DialogGuru (root)
2. Lihat Inspector, harus ada: BriefingSequence (Script)
3. Pastikan semua field terisi (sudah dicek di TAHAP 2)
```

---

### **6.4. Link BriefingSequence ke LevelManager:**

```
1. Select LevelManager di Hierarchy
2. Lihat Inspector → ProcessingLevelManager script
3. Lihat field "Briefing Script"
4. Drag GameObject "Panel_DialogGuru" ke field tersebut
```

**Verifikasi:**
```
┌─────────────────────────────────────┐
│ 🔷 LevelManager                     │
├─────────────────────────────────────┤
│ ✅ Processing Level Manager (Script)│
│                                     │
│ Mesin Spawner: 🔗 SpawnerManager    │  ✅
│ Briefing Script: 🔗 Panel_DialogGuru│  ✅ BARU DILINK
│ Data Level Ini: 📄 LevelData_...    │  ✅
│ ...                                 │
└─────────────────────────────────────┘
```

**⚠️ PENTING:**
```
Script akan otomatis:
1. Override tombol "Mulai" dengan ProcessingLevelManager.MulaiMain
2. Pakai barisDialogSortir dari LevelData_Processing
3. Tidak perlu setup manual event Button_Mulai

Jika ada event lama di Button_Mulai, BIARKAN.
Akan di-override oleh script.
```

---

## 🔧 LANGKAH 7: BUAT HUD (TEXT_SKOR & TEXT_TIMER)

### **⚠️ SUPER PENTING: NAMA HARUS PERSIS!**

GameObject **HARUS** bernama **PERSIS** seperti ini:
```
Text_Skor  (BUKAN "TextSkor", "Text Skor", atau "text_skor")
Text_Timer (BUKAN "TextTimer", "Text Timer", atau "text_timer")
```

**Alasan:**
```csharp
// GameManager mencari dengan GameObject.Find()
GameObject objSkor = GameObject.Find("Text_Skor");  // Case-sensitive!
GameObject objTimer = GameObject.Find("Text_Timer");

Jika nama salah → NULL → Skor/Timer tidak update
```

---

### **7.1. Buat Text_Skor:**

```
1. Select Canvas di Hierarchy
2. Klik kanan Canvas
3. Pilih: UI → Text - TextMeshPro
4. Rename PERSIS: "Text_Skor" (dengan underscore _)
```

---

### **7.2. Setup Text_Skor di Inspector:**

**Select Text_Skor, lihat Inspector:**

#### **Rect Transform:**
```
Position:
  X: -400 (kiri atas layar)
  Y: 250 (atas)
  Z: 0

Width: 200
Height: 50
```

#### **TextMeshPro - Text (UI):**
```
Text: "Skor: 0"
Font Size: 24
Color: White (atau warna kontras dengan background)
Alignment: Left, Top
```

**💡 Visual:**
```
┌──────────────────────────────┐
│ Skor: 0           Timer: 60  │ ← HUD
├──────────────────────────────┤
│                              │
│       GAMEPLAY AREA          │
│                              │
└──────────────────────────────┘
```

---

### **7.3. Buat Text_Timer:**

```
1. Select Canvas di Hierarchy
2. Klik kanan Canvas
3. Pilih: UI → Text - TextMeshPro
4. Rename PERSIS: "Text_Timer" (dengan underscore _)
```

---

### **7.4. Setup Text_Timer di Inspector:**

**Select Text_Timer, lihat Inspector:**

#### **Rect Transform:**
```
Position:
  X: 400 (kanan atas layar)
  Y: 250 (atas)
  Z: 0

Width: 200
Height: 50
```

#### **TextMeshPro - Text (UI):**
```
Text: "00:00"
Font Size: 24
Color: White
Alignment: Right, Top
```

---

### **7.5. Verifikasi Nama (CRITICAL!):**

**Double-check nama di Hierarchy:**

```
Canvas
├── Text_Skor  ✅ PERSIS "Text_Skor" dengan underscore
├── Text_Timer ✅ PERSIS "Text_Timer" dengan underscore
└── ...
```

**❌ SALAH:**
```
- TextSkor (tanpa underscore) ❌
- Text Skor (pakai spasi) ❌
- text_skor (huruf kecil semua) ❌
- Text_Score (bahasa Inggris) ❌
```

**✅ BENAR:**
```
- Text_Skor ✅
- Text_Timer ✅
```

---

## 🔧 LANGKAH 8: BUAT PANEL MENANG (WIN PANEL)

### **8.1. Create Panel:**

```
1. Select Canvas di Hierarchy
2. Klik kanan Canvas
3. Pilih: UI → Panel
4. Rename: "Panel_Menang"
```

---

### **8.2. Tambah Child UI Elements:**

#### **Text Judul:**
```
1. Klik kanan Panel_Menang
2. Pilih: UI → Text - TextMeshPro
3. Rename: "Text_Judul"
4. Di Inspector:
   Text: "Selamat!"
   Font Size: 48
   Color: Hijau atau Gold
   Alignment: Center, Top
```

#### **Text Skor:**
```
1. Klik kanan Panel_Menang
2. Pilih: UI → Text - TextMeshPro
3. Rename: "Text_Skor"
4. Di Inspector:
   Text: "Skor: 0"
   Font Size: 32
   Color: White
   Alignment: Center, Middle
```

#### **Text Waktu:**
```
1. Klik kanan Panel_Menang
2. Pilih: UI → Text - TextMeshPro
3. Rename: "Text_Waktu"
4. Di Inspector:
   Text: "Waktu: 00:00"
   Font Size: 28
   Color: White
   Alignment: Center, Middle
```

#### **Button Menu (Opsional):**
```
1. Klik kanan Panel_Menang
2. Pilih: UI → Button - TextMeshPro
3. Rename: "Button_Menu"
4. Select child "Text (TMP)"
5. Ubah text: "Kembali ke Menu"
6. Setup event (lihat 8.4)
```

---

### **8.3. Atur Layout:**

**Position semua element agar rapi:**

```
Panel_Menang: (Full screen, center)
├── Text_Judul: (Atas, center, Y: 100)
├── Text_Skor: (Tengah, center, Y: 0)
├── Text_Waktu: (Tengah bawah, Y: -50)
└── Button_Menu: (Bawah, center, Y: -150)
```

---

### **8.4. Set Panel Inactive:**

```
1. Select Panel_Menang
2. Di Inspector, UNCHECK checkbox di samping nama:
   ☐ Panel_Menang  ← Harus unchecked

Alasan: Panel hanya muncul saat level selesai
```

---

### **8.5. Setup Button_Menu Event (Opsional):**

```
1. Select Button_Menu
2. Lihat Inspector → Button (Script)
3. On Click () → Klik "+"
4. No Function → UnityEngine.SceneManagement.SceneManager
5. Pilih: LoadScene (string)
6. Isi parameter: "00_MainMenu" (atau nama scene menu Anda)
```

---

### **8.6. Link Panel ke LevelManager:**

**Sekarang link Panel_Menang dan child-nya:**

```
1. Select LevelManager di Hierarchy
2. Lihat Inspector → ProcessingLevelManager script
3. Isi field berikut:

   Panel Win Scene2:
   - Drag Panel_Menang dari Hierarchy

   Text Skor Akhir Scene2:
   - Expand Panel_Menang di Hierarchy
   - Drag Text_Skor (yang di dalam Panel_Menang) ke field ini

   Text Waktu Akhir Scene2:
   - Drag Text_Waktu (yang di dalam Panel_Menang) ke field ini
```

**⚠️ PERHATIAN:**
```
JANGAN drag Text_Skor dari HUD (root Canvas)!
Harus drag Text_Skor yang CHILD dari Panel_Menang!

HUD:
  Canvas → Text_Skor  ❌ JANGAN INI (untuk runtime display)
  
Win Panel:
  Canvas → Panel_Menang → Text_Skor  ✅ DRAG INI (untuk display final)
```

**Verifikasi Final:**
```
┌─────────────────────────────────────┐
│ 🔷 LevelManager                     │
├─────────────────────────────────────┤
│ ✅ Processing Level Manager (Script)│
│                                     │
│ Mesin Spawner: 🔗 SpawnerManager    │  ✅
│ Briefing Script: 🔗 Panel_DialogGuru│  ✅
│ Data Level Ini: 📄 LevelData_...    │  ✅
│                                     │
│ Panel Win Scene2: 🔗 Panel_Menang   │  ✅
│ Text Skor Akhir: 🔗 Text_Skor       │  ✅ (dari Panel_Menang)
│ Text Waktu Akhir: 🔗 Text_Waktu     │  ✅ (dari Panel_Menang)
└─────────────────────────────────────┘
```

---

## 🔧 LANGKAH 9: SETUP TEMPAT SAMPAH (BIN TARGETS)

### **9.1. Cek Apakah Bin Sudah Ada:**

**Di Hierarchy Scene 03, cari:**
```
- Kotak_Organik (atau Bin_Organik)
- Kotak_Anorganik (atau Bin_Anorganik)
- Kotak_B3 (atau Bin_B3)
```

**Kasus A: Bin SUDAH ADA**
```
✅ Skip ke 9.3 (Verifikasi Script)
```

**Kasus B: Bin BELUM ADA**
```
⚠️ Lanjut ke 9.2 (Membuat Bin Baru)
```

---

### **9.2. Buat Tempat Sampah Baru:**

**Untuk SETIAP tipe (Organik, Anorganik, B3):**

#### **Create GameObject:**
```
1. Di Hierarchy, klik kanan
2. Pilih: Create Empty
3. Rename: "Kotak_Organik" (ulangi untuk Anorganik & B3)
```

#### **Add Visual:**
```
1. Klik kanan Kotak_Organik
2. Pilih: 2D Object → Sprite
3. Rename child: "Visual"
4. Select Visual, di Inspector:
   - Sprite Renderer → Sprite: [Drag sprite kotak sampah hijau]
   - Sorting Layer: (pastikan visible di depan background)
```

#### **Add Collider:**
```
1. Select Kotak_Organik (parent)
2. Add Component: Box Collider 2D
3. Di Inspector:
   - Is Trigger: ☑ CHECK INI (harus trigger!)
   - Size: Sesuaikan dengan sprite kotak
```

#### **Add Script:**
```
1. Select Kotak_Organik
2. Add Component: "BinController" atau "TrashBinTarget"
   (tergantung nama script di project Anda)
```

#### **Setup Script:**
```
Select Kotak_Organik, lihat Inspector:

BinController (atau TrashBinTarget):
  Tipe Bin: Organik  ⬅️ Pilih dari dropdown
```

#### **Position:**
```
Atur position di Scene View:

Kotak_Organik:   X: -3, Y: -2, Z: 0
Kotak_Anorganik: X:  0, Y: -2, Z: 0
Kotak_B3:        X:  3, Y: -2, Z: 0

(Sesuaikan dengan layout scene Anda)
```

---

### **9.3. Verifikasi Bin Setup:**

**Untuk SETIAP bin (Organik, Anorganik, B3):**

```
✅ Punya Box Collider 2D dengan Is Trigger = ☑
✅ Punya script BinController atau TrashBinTarget
✅ Field "Tipe Bin" sesuai (Organik, Anorganik, B3)
✅ Position visible di scene
✅ Sprite visual sesuai warna:
   - Organik: Hijau
   - Anorganik: Kuning
   - B3: Merah
```

---

## 🔧 LANGKAH 10: SAVE EVERYTHING

### **10.1. Save Scene:**

```
Ctrl + S
atau File → Save Scene
```

---

### **10.2. Save Project:**

```
Ctrl + Shift + S
atau File → Save Project
```

---

### **10.3. Verifikasi Save:**

```
Pastikan tidak ada tanda asterisk (*) di tab Scene
```

---

## 🧪 LANGKAH 11: TEST SCENE 03 STANDALONE

### **Tujuan:**
Test Scene 03 tanpa melalui Scene 02 (pakai data test)

---

### **11.1. Play Scene 03:**

```
1. Pastikan Scene 03_Game_Processing terbuka
2. Tekan Play ▶️
```

---

### **11.2. Test Sequence:**

**Test 1: Briefing Muncul**
```
✅ Wait 0.1s (loading GameManager)
✅ Panel_Intro muncul dengan fade in
✅ Text_Judul: "FASE PENGOLAHAN"
✅ Text_Info: "Fase 2: Pengolahan Sampah"
✅ Setelah 3 detik → Panel_Dialog
```

**Test 2: Dialog Sortir**
```
✅ Dialog pertama dari barisDialogSortir muncul
✅ Tombol "Next" visible
✅ Klik Next → Dialog berganti
✅ Dialog terakhir → Tombol "Mulai" muncul
```

**Test 3: Game Start**
```
✅ Klik "Mulai" → Panel briefing hilang
✅ HUD muncul: "Skor: 0" dan "Timer: 60"
✅ Timer mulai countdown
✅ Sampah mulai spawn dari TitikSpawn
```

**Test 4: Spawn System**
```
✅ Sampah spawn setiap 2.5 detik (sesuai interval)
✅ Sampah menggunakan sprite dari WasteData
✅ Sampah pakai data dari "Daftar Sampah Test"
✅ Sampah bisa di-drag
```

**Test 5: Drag & Drop**
```
✅ Drag sampah ke kotak yang BENAR:
   - Skor bertambah (sesuai WasteData.skorBenar)
   - Sampah hilang
   - Console log: "BENAR! +10 poin" (atau nilai lain)

✅ Drag sampah ke kotak yang SALAH:
   - Skor berkurang (sesuai WasteData.skorSalah)
   - Sampah hilang
   - Console log: "SALAH! -5 poin"
```

**Test 6: Win Condition**
```
✅ Setelah semua sampah selesai (atau timer habis):
   - Panel_Menang muncul
   - Text_Skor: "Skor: [angka]"
   - Text_Waktu: "Waktu: [sisa waktu]"
   - Game freeze (Time.timeScale = 0)
```

---

### **11.3. Cek Console Log:**

**Log yang HARUS muncul:**

```
==================================================
[1] ProcessingLevelManager: Menunggu GameManager siap...
[2] GameManager Ditemukan. Melakukan Setup Level Baru...
GameManager: Setup Level Baru dimulai...
[3] Memulai Briefing...
[BRIEFING] SetupSequenceKhusus dipanggil.
[BRIEFING] Mainkan Intro...
[BRIEFING] Tombol Mulai Dimunculkan.

(Setelah klik Mulai:)
[GAME START] Game Dimulai.
Spawner: Menggunakan Data TEST (Tidak ada inventaris).
WasteSpawner: Spawn sampah ke-1 (Sisa Nasi)

(Setelah drag sampah:)
BENAR! Sisa Nasi masuk ke tong yang pas. +5 poin
Sisa Sampah Target: 4

(Jika salah:)
SALAH! Botol Plastik jangan dibuang di sini! -5 poin
```

**❌ Jika Log TIDAK sesuai:**
```
Lihat Troubleshooting di bawah
```

---

### **11.4. Stop Play:**

```
Tekan Stop ⏹️ setelah testing selesai
```

---

## 🧪 LANGKAH 12: TEST TRANSISI SCENE 02 → 03

### **Tujuan:**
Test flow lengkap: Kumpulkan sampah di Scene 02 → Pilah di Scene 03

---

### **12.1. Buka Scene 02:**

```
Assets → _Scenes → 02_Game_Kantin.unity
```

---

### **12.2. Play dari Scene 02:**

```
1. Tekan Play ▶️
2. Mainkan Scene 02:
   - Skip/lewati briefing (klik Next berkali-kali)
   - Klik tombol "Mulai"
   - Klik semua sampah hingga terkumpul
3. Panel Selesai muncul
4. Klik tombol "Lanjut"
```

---

### **12.3. Verifikasi Transisi:**

**Saat Pindah ke Scene 03:**
```
✅ Scene 03 terbuka
✅ GameManager masih ada (cek Hierarchy → DontDestroyOnLoad)
✅ Briefing muncul dengan barisDialogSortir
✅ Setelah klik "Mulai" → Game jalan
✅ Sampah yang spawn = sampah yang dikumpulkan di Scene 02
✅ Jumlah sampah sesuai dengan yang dikumpulkan
```

**Cek Console Log:**
```
Spawner: Menggunakan Data dari Inventaris Pemain (Fase 1).
WasteSpawner: Menggunakan 8 sampah dari inventory (bukan test data)
```

**❗ Yang TIDAK BOLEH TERJADI:**
```
❌ Spawner pakai "Daftar Sampah Test"
❌ Jumlah sampah berbeda dengan yang dikumpulkan
❌ GameManager hilang (NULL)
❌ Skor/Timer tidak update
```

---

### **12.4. Play hingga Selesai:**

```
1. Pilah semua sampah
2. Panel_Menang muncul dengan skor final
3. Verifikasi skor = hasil pemilahan
```

---

### **12.5. Stop Play:**

```
Tekan Stop ⏹️
```

---

## ✅ CHECKLIST AKHIR TAHAP 4

### **LevelData & Managers:**
- [ ] LevelData_Processing dibuat dan terisi lengkap
- [ ] barisDialogSortir minimal 3-5 kalimat (TIDAK NULL!)
- [ ] GameObject "LevelManager" ada dengan ProcessingLevelManager script
- [ ] GameObject "SpawnerManager" ada dengan WasteSpawner script
- [ ] GameObject "TitikSpawn" ada dan di-position dengan benar

### **Inspector Setup:**
- [ ] LevelManager.mesinSpawner → Linked ke SpawnerManager
- [ ] LevelManager.briefingScript → Linked ke Panel_DialogGuru
- [ ] LevelManager.dataLevelIni → Linked ke LevelData_Processing
- [ ] LevelManager.panelWinScene2 → Linked ke Panel_Menang
- [ ] LevelManager.textSkorAkhirScene2 → Linked ke Panel_Menang/Text_Skor
- [ ] LevelManager.textWaktuAkhirScene2 → Linked ke Panel_Menang/Text_Waktu
- [ ] SpawnerManager.prefabSampah → Linked ke Sampah_Draggable prefab
- [ ] SpawnerManager.titikSpawn → Linked ke TitikSpawn
- [ ] SpawnerManager.daftarSampahTest → Minimal 5 WasteData berbeda

### **UI Setup:**
- [ ] Panel_DialogGuru ada di Canvas (prefab)
- [ ] HUD: GameObject "Text_Skor" (NAMA PERSIS dengan underscore)
- [ ] HUD: GameObject "Text_Timer" (NAMA PERSIS dengan underscore)
- [ ] Panel_Menang ada dan inactive by default
- [ ] Panel_Menang punya child: Text_Skor & Text_Waktu

### **Gameplay:**
- [ ] Kotak_Organik ada dengan BinController (Tipe: Organik)
- [ ] Kotak_Anorganik ada dengan BinController (Tipe: Anorganik)
- [ ] Kotak_B3 ada dengan BinController (Tipe: B3)
- [ ] Semua bin punya Box Collider 2D (Is Trigger: ☑)

### **Testing:**
- [ ] Test Scene 03 standalone: Briefing, spawn, drag, win condition
- [ ] Test transisi Scene 02 → 03: Inventory transfer, jumlah sampah match
- [ ] Console log sesuai ekspektasi (no error)
- [ ] Skor dinamis bekerja (nilai berbeda per WasteData)
- [ ] Timer countdown bekerja
- [ ] Panel_Menang muncul dengan data benar

---

## 🚨 TROUBLESHOOTING

### **Problem: Briefing Tidak Muncul**
**Penyebab:**
- barisDialogSortir NULL di LevelData_Processing
- BriefingScript tidak linked ke LevelManager

**Solusi:**
```
1. Buka LevelData_Processing
2. Pastikan barisDialogSortir Size > 0 dan ada isi
3. Cek LevelManager.briefingScript linked
```

---

### **Problem: Sampah Tidak Spawn**
**Penyebab:**
- WasteSpawner disabled
- TitikSpawn tidak linked
- Prefab sampah NULL

**Solusi:**
```
1. Cek SpawnerManager enabled di Hierarchy
2. Cek field titikSpawn terisi
3. Cek prefabSampah terisi
4. Lihat Console log untuk error
```

---

### **Problem: "Text_Skor" atau "Text_Timer" Tidak Update**
**Penyebab:**
- Nama GameObject salah (case-sensitive!)
- GameObject tidak active

**Solusi:**
```
1. Di Hierarchy, cari GameObject
2. Pastikan nama PERSIS:
   - "Text_Skor" (dengan underscore, T kapital)
   - "Text_Timer" (dengan underscore, T kapital)
3. Pastikan GameObject active (☑)
4. Restart scene jika masih error
```

---

### **Problem: Drag & Drop Tidak Berfungsi**
**Penyebab:**
- Collider bin tidak trigger
- DragController script error
- Sampah tidak punya Rigidbody2D

**Solusi:**
```
1. Cek semua bin punya Box Collider 2D
2. Pastikan Is Trigger = ☑
3. Cek prefab sampah punya:
   - Collider2D
   - Rigidbody2D (Kinematic)
   - DragController script
```

---

### **Problem: Skor Tidak Dinamis (Selalu +10/-5)**
**Penyebab:**
- WasteData belum diupdate (TAHAP 3 belum selesai)

**Solusi:**
```
1. Kembali ke TAHAP 3
2. Update semua WasteData dengan skorBenar/skorSalah
3. Test ulang
```

---

### **Problem: Scene 03 Crash saat Load dari Scene 02**
**Penyebab:**
- GameManager tidak persisten dari Scene 02
- Ada 2 GameManager (conflict)

**Solusi:**
```
1. Buka Scene 02, pastikan hanya 1 GameManager
2. Cek GameManager.Awake() ada DontDestroyOnLoad
3. DELETE GameManager dari Scene 03 jika ada
```

---

### **Problem: Panel_Menang Tidak Muncul**
**Penyebab:**
- Panel tidak linked ke LevelManager
- GameManager.LevelSelesai() tidak dipanggil

**Solusi:**
```
1. Cek LevelManager.panelWinScene2 terisi
2. Cek skor sampah benar → KurangiJumlahSampah dipanggil
3. Lihat Console log untuk trace
```

---

## ⏭️ LANGKAH SELANJUTNYA

**Jika SEMUA checklist ✅:**
- ✅ Scene 03 sudah lengkap dan berfungsi!
- ✅ Lanjut ke **TAHAP 5: Testing & Debugging Final**
- 📄 Buka file: `TUTORIAL_TAHAP_5_Testing.md`

**Jika Ada yang ❌:**
- ⚠️ Selesaikan dulu masalahnya
- 🔄 Ulangi testing standalone dan transisi
- 📞 Lihat Troubleshooting atau minta bantuan

---

**🎉 Selamat! TAHAP 4 Selesai!**

**Next:** TAHAP 5 - Testing Final & Polish

---

**Developer:** daffarobbani18  
**Project:** Eco-Quest  
**Last Updated:** December 4, 2025
