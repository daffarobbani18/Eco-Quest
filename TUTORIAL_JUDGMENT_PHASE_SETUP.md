# 🎓 TUTORIAL SETUP UNITY - JUDGMENT PHASE
## Panduan Lengkap Implementasi Slideshow Edukasi Kesalahan

---

## 📋 **DAFTAR ISI**
1. [Pengantar Judgment Phase](#pengantar)
2. [Persiapan: File Script](#persiapan-script)
3. [Setup Hierarchy (UI Canvas)](#setup-hierarchy)
4. [Setup Inspector (Linking References)](#setup-inspector)
5. [Setup Sprites (Icon Tong Sampah)](#setup-sprites)
6. [Testing & Debug](#testing)
7. [Troubleshooting](#troubleshooting)

---

## 🎯 **PENGANTAR JUDGMENT PHASE** {#pengantar}

### **Apa itu Judgment Phase?**
Judgment Phase adalah fitur edukatif yang menampilkan **slideshow kesalahan** setelah level selesai, sebelum Panel Win muncul.

### **Alur Flow Game:**
```
Level Selesai → Cek Kesalahan → [Ada Kesalahan?]
                                      ↓
                            [Ya]               [Tidak]
                              ↓                   ↓
                    Judgment Slideshow      Langsung Win Panel
                    (3s per kesalahan)
                              ↓
                         Win Panel
```

### **Manfaat Edukatif:**
- ✅ **Immediate Feedback:** Pemain tahu kesalahan spesifik mereka
- ✅ **Visual Learning:** Icon sampah + tong (salah vs benar)
- ✅ **Penjelasan Kontekstual:** Database 30+ penjelasan edukatif
- ✅ **Deduplication:** Hanya tampilkan kesalahan unik (5x salah Apel = 1 slide)
- ✅ **Non-Intrusive:** Tidak mengganggu gameplay (muncul di akhir)

---

## 📂 **PERSIAPAN: FILE SCRIPT** {#persiapan-script}

### **File yang Sudah Dibuat:**

1. **GameManager.cs** (Modified)
   - ✅ Class `MistakeRecord` ditambahkan
   - ✅ Fungsi `RecordMistake()`, `ClearMistakes()`, `ShowWinPanel()`
   - ✅ List tracking: `mistakesList`, `recordedMistakes`

2. **DragController.cs** (Modified)
   - ✅ Panggil `GameManager.RecordMistake()` saat salah pilah

3. **ProcessingLevelManager.cs** (Modified)
   - ✅ Fungsi `StartJudgmentPhase()` untuk trigger slideshow
   - ✅ Field `judgmentSlideshow` untuk link script

4. **JudgmentSlideshow.cs** (New File)
   - ✅ Script lengkap untuk slideshow UI
   - ✅ Database 30+ penjelasan edukatif
   - ✅ Coroutine slideshow otomatis

### **Lokasi File:**
```
Assets/
└── _Scripts/
    ├── Manager/
    │   └── GameManager.cs ✅
    ├── Gameplay/
    │   ├── DragController.cs ✅
    │   └── ProcessingLevelManager.cs ✅
    └── UI/
        └── JudgmentSlideshow.cs ✅ (BARU)
```

---

## 🏗️ **SETUP HIERARCHY (UI CANVAS)** {#setup-hierarchy}

### **LANGKAH 1: Buka Scene Processing**
1. Di Unity Editor, buka scene **`03_Game_Processing`**
2. Di Hierarchy window, cari object **`Canvas`**

---

### **LANGKAH 2: Buat Panel Judgment Phase**

#### **2.1. Buat Panel Utama**
1. Klik kanan pada **Canvas** → **UI** → **Panel**
2. Rename jadi **`Panel_JudgmentPhase`**
3. Di Inspector, setting **Rect Transform:**
   ```
   Anchor: Stretch (Full Screen)
   Left: 0, Right: 0, Top: 0, Bottom: 0
   ```
4. **Image Component** (Background):
   ```
   Color: Hitam (R:0, G:0, B:0, A:200)  ← Semi-transparan
   ```

---

#### **2.2. Buat Container Slide**
1. Klik kanan **Panel_JudgmentPhase** → **UI** → **Panel**
2. Rename jadi **`Container_Slide`**
3. **Rect Transform:**
   ```
   Width: 800
   Height: 600
   Anchor: Middle Center
   Pos X: 0, Pos Y: 0
   ```
4. **Image Component** (Background slide):
   ```
   Color: Putih (R:255, G:255, B:255, A:255)
   Sprite: UI Sprite (rounded corners jika ada)
   ```

---

#### **2.3. Buat Header Text**
Di dalam **Container_Slide**, buat:

1. Klik kanan **Container_Slide** → **UI** → **Text - TextMeshPro**
2. Rename jadi **`Text_Header`**
3. **Rect Transform:**
   ```
   Width: 700
   Height: 80
   Anchor: Top Center
   Pos X: 0, Pos Y: -50
   ```
4. **TextMeshProUGUI Component:**
   ```
   Text: "❌ KESALAHAN DITEMUKAN"
   Font: Bold
   Font Size: 48
   Color: Merah (R:255, G:50, B:50)
   Alignment: Center & Middle
   ```

---

#### **2.4. Buat Icon Sampah**

1. Klik kanan **Container_Slide** → **UI** → **Image**
2. Rename jadi **`Image_WasteIcon`**
3. **Rect Transform:**
   ```
   Width: 150
   Height: 150
   Anchor: Top Center
   Pos X: 0, Pos Y: -150
   ```
4. **Image Component:**
   ```
   Sprite: (Kosongkan - akan diisi runtime)
   Color: Putih (R:255, G:255, B:255, A:255)
   Preserve Aspect: ✅ (Centang)
   ```

---

#### **2.5. Buat Text Nama Sampah**

1. Klik kanan **Container_Slide** → **UI** → **Text - TextMeshPro**
2. Rename jadi **`Text_WasteName`**
3. **Rect Transform:**
   ```
   Width: 400
   Height: 50
   Anchor: Top Center
   Pos X: 0, Pos Y: -320
   ```
4. **TextMeshProUGUI Component:**
   ```
   Text: "Nama Sampah"
   Font Size: 36
   Color: Hitam
   Alignment: Center & Middle
   Font Style: Bold
   ```

---

#### **2.6. Buat Section "Kamu Pilih" (SALAH)**

**A. Container Salah:**
1. Klik kanan **Container_Slide** → **Create Empty**
2. Rename jadi **`Container_Wrong`**
3. **Rect Transform:**
   ```
   Width: 300
   Height: 200
   Anchor: Middle Left
   Pos X: 150, Pos Y: -100
   ```

**B. Icon Tong Salah:**
1. Klik kanan **Container_Wrong** → **UI** → **Image**
2. Rename jadi **`Image_WrongBin`**
3. **Rect Transform:**
   ```
   Width: 120
   Height: 120
   Anchor: Top Center
   Pos X: 0, Pos Y: 0
   ```
4. **Image Component:**
   ```
   Sprite: (Kosongkan - akan diisi runtime)
   Preserve Aspect: ✅
   ```

**C. Text Salah:**
1. Klik kanan **Container_Wrong** → **UI** → **Text - TextMeshPro**
2. Rename jadi **`Text_WrongBin`**
3. **Rect Transform:**
   ```
   Width: 280
   Height: 100
   Anchor: Bottom Center
   Pos X: 0, Pos Y: -80
   ```
4. **TextMeshProUGUI Component:**
   ```
   Text: "Kamu pilih:\n🟡 Anorganik"
   Font Size: 24
   Color: Merah (R:200, G:50, B:50)
   Alignment: Center & Top
   ```

**D. Icon X Merah (Indikator Salah):**
1. Klik kanan **Container_Wrong** → **UI** → **Text - TextMeshPro**
2. Rename jadi **`Icon_Wrong`**
3. **Rect Transform:**
   ```
   Width: 50
   Height: 50
   Anchor: Top Right
   Pos X: 20, Pos Y: 20
   ```
4. **TextMeshProUGUI Component:**
   ```
   Text: "❌"
   Font Size: 40
   Alignment: Center & Middle
   ```

---

#### **2.7. Buat Section "Seharusnya" (BENAR)**

**A. Container Benar:**
1. Klik kanan **Container_Slide** → **Create Empty**
2. Rename jadi **`Container_Correct`**
3. **Rect Transform:**
   ```
   Width: 300
   Height: 200
   Anchor: Middle Right
   Pos X: -150, Pos Y: -100
   ```

**B. Icon Tong Benar:**
1. Klik kanan **Container_Correct** → **UI** → **Image**
2. Rename jadi **`Image_CorrectBin`**
3. **Rect Transform:**
   ```
   Width: 120
   Height: 120
   Anchor: Top Center
   Pos X: 0, Pos Y: 0
   ```
4. **Image Component:**
   ```
   Sprite: (Kosongkan - akan diisi runtime)
   Preserve Aspect: ✅
   ```

**C. Text Benar:**
1. Klik kanan **Container_Correct** → **UI** → **Text - TextMeshPro**
2. Rename jadi **`Text_CorrectBin`**
3. **Rect Transform:**
   ```
   Width: 280
   Height: 100
   Anchor: Bottom Center
   Pos X: 0, Pos Y: -80
   ```
4. **TextMeshProUGUI Component:**
   ```
   Text: "Seharusnya:\n🟢 Organik"
   Font Size: 24
   Color: Hijau (R:50, G:180, B:50)
   Alignment: Center & Top
   ```

**D. Icon Checkmark Hijau:**
1. Klik kanan **Container_Correct** → **UI** → **Text - TextMeshPro**
2. Rename jadi **`Icon_Correct`**
3. **Rect Transform:**
   ```
   Width: 50
   Height: 50
   Anchor: Top Right
   Pos X: 20, Pos Y: 20
   ```
4. **TextMeshProUGUI Component:**
   ```
   Text: "✅"
   Font Size: 40
   Alignment: Center & Middle
   ```

---

#### **2.8. Buat Panel Penjelasan**

1. Klik kanan **Container_Slide** → **UI** → **Panel**
2. Rename jadi **`Panel_Explanation`**
3. **Rect Transform:**
   ```
   Width: 700
   Height: 150
   Anchor: Bottom Center
   Pos X: 0, Pos Y: 100
   ```
4. **Image Component:**
   ```
   Color: Kuning Muda (R:255, G:250, B:200, A:255)
   ```

**Text Penjelasan:**
1. Klik kanan **Panel_Explanation** → **UI** → **Text - TextMeshPro**
2. Rename jadi **`Text_Explanation`**
3. **Rect Transform:**
   ```
   Anchor: Stretch (Fill Parent)
   Left: 20, Right: 20, Top: 20, Bottom: 20
   ```
4. **TextMeshProUGUI Component:**
   ```
   Text: "💡 Penjelasan akan muncul di sini..."
   Font Size: 20
   Color: Hitam
   Alignment: Left & Top
   Wrapping: Enabled ✅
   ```

---

#### **2.9. Buat Text Progress**

1. Klik kanan **Container_Slide** → **UI** → **Text - TextMeshPro**
2. Rename jadi **`Text_Progress`**
3. **Rect Transform:**
   ```
   Width: 200
   Height: 50
   Anchor: Bottom Center
   Pos X: 0, Pos Y: 20
   ```
4. **TextMeshProUGUI Component:**
   ```
   Text: "Kesalahan 1/3"
   Font Size: 24
   Color: Abu-abu (R:100, G:100, B:100)
   Alignment: Center & Middle
   ```

---

### **LANGKAH 3: Matikan Panel Awal**
1. Pilih **Panel_JudgmentPhase** di Hierarchy
2. Di Inspector, **uncheck** kotak di samping nama object (disable)
3. Panel akan di-enable runtime oleh script

---

### **📐 HASIL AKHIR HIERARCHY:**

```
Canvas
└── Panel_JudgmentPhase (INACTIVE)
    └── Container_Slide
        ├── Text_Header
        ├── Image_WasteIcon
        ├── Text_WasteName
        ├── Container_Wrong
        │   ├── Image_WrongBin
        │   ├── Text_WrongBin
        │   └── Icon_Wrong
        ├── Container_Correct
        │   ├── Image_CorrectBin
        │   ├── Text_CorrectBin
        │   └── Icon_Correct
        ├── Panel_Explanation
        │   └── Text_Explanation
        └── Text_Progress
```

---

## 🔗 **SETUP INSPECTOR (LINKING REFERENCES)** {#setup-inspector}

### **LANGKAH 4: Setup JudgmentSlideshow Component**

#### **4.1. Attach Script ke Panel**
1. Pilih **Panel_JudgmentPhase** di Hierarchy
2. Di Inspector, klik **Add Component**
3. Search: **`JudgmentSlideshow`**
4. Klik untuk attach script

---

#### **4.2. Link UI References**

Sekarang kita link semua UI ke script. Di Inspector **JudgmentSlideshow** component:

**A. UI References - Slide Components:**
```
┌─────────────────────────────────────────────────┐
│ Waste Icon:           [Drag: Image_WasteIcon]  │
│ Waste Name Text:      [Drag: Text_WasteName]   │
│ Wrong Bin Icon:       [Drag: Image_WrongBin]   │
│ Wrong Bin Text:       [Drag: Text_WrongBin]    │
│ Correct Bin Icon:     [Drag: Image_CorrectBin] │
│ Correct Bin Text:     [Drag: Text_CorrectBin]  │
│ Explanation Text:     [Drag: Text_Explanation] │
│ Progress Text:        [Drag: Text_Progress]    │
└─────────────────────────────────────────────────┘
```

**Cara Drag:**
- Klik object di Hierarchy (contoh: `Image_WasteIcon`)
- Drag ke field yang sesuai di Inspector
- Field akan berubah biru saat hover

---

#### **4.3. Setup Bin Sprites**

**⚠️ PENTING:** Kamu perlu 3 sprite icon tong sampah!

**B. Bin Sprites (Assign di Inspector):**
```
┌─────────────────────────────────────────────────┐
│ Bin Organik Sprite:   [🟢 Icon Tong Hijau]     │
│ Bin Anorganik Sprite: [🟡 Icon Tong Kuning]    │
│ Bin B3 Sprite:        [🔴 Icon Tong Merah]     │
└─────────────────────────────────────────────────┘
```

**Cara Assign:**
1. Di Project window, cari folder **`Assets/Art/Sprites/`** (atau folder sprites kamu)
2. Cari sprite icon tong (contoh: `Sprite_TongOrganik.png`)
3. Drag ke field **Bin Organik Sprite**
4. Ulangi untuk Anorganik dan B3

**❓ Jika Belum Ada Sprite:**
- Buat sprite sederhana di Photoshop/GIMP (128x128px)
- Atau gunakan emoji/text sementara (🟢🟡🔴)
- Atau screenshot dari game, crop jadi icon

---

#### **4.4. Setup Settings**

**C. Settings:**
```
┌─────────────────────────────────────────────────┐
│ Slide Duration:       [3.5]                     │
│ Judgment Panel:       [Drag: Panel_JudgmentPhase] │
└─────────────────────────────────────────────────┘
```

- **Slide Duration:** Berapa lama (detik) setiap slide ditampilkan
  - Default: **3.5 detik**
  - Bisa diubah sesuai kebutuhan (2-5 detik optimal)
  
- **Judgment Panel:** Drag **Panel_JudgmentPhase** (object parent)

---

### **LANGKAH 5: Link ke ProcessingLevelManager**

#### **5.1. Buka ProcessingLevelManager**
1. Di Hierarchy scene `03_Game_Processing`, cari object **`LevelManager`** (atau nama object yang punya script ProcessingLevelManager)
2. Klik object tersebut
3. Di Inspector, scroll ke component **ProcessingLevelManager**

---

#### **5.2. Link JudgmentSlideshow**

Di Inspector **ProcessingLevelManager**, cari section:
```
┌─────────────────────────────────────────────────┐
│ Judgment Phase System                           │
│                                                 │
│ Judgment Slideshow:  [Drag: Panel_JudgmentPhase]│
└─────────────────────────────────────────────────┘
```

**Cara:**
- Drag **Panel_JudgmentPhase** (yang sudah ada script JudgmentSlideshow) ke field **Judgment Slideshow**
- Unity akan otomatis detect component JudgmentSlideshow dari object tersebut

---

## 🖼️ **SETUP SPRITES (ICON TONG SAMPAH)** {#setup-sprites}

### **LANGKAH 6: Siapkan Sprite Icon Tong**

#### **Opsi A: Gunakan Sprite Existing**
Jika kamu sudah punya sprite tong di game:

1. Di Hierarchy, cari tong sampah di scene (contoh: `Tong_Organik`)
2. Klik object tersebut
3. Di Inspector, lihat component **Sprite Renderer** atau **Image**
4. Klik sprite yang ter-assign (akan highlight di Project window)
5. Catat lokasi sprite tersebut
6. Gunakan sprite yang sama untuk **JudgmentSlideshow**

---

#### **Opsi B: Extract Sprite dari Tong GameObject**
Jika sprite ada di prefab tong:

1. Di Project window, buka folder **`Assets/Prefabs/`**
2. Cari prefab tong (contoh: `Tong_Organik.prefab`)
3. Buka prefab (double click)
4. Lihat Sprite Renderer → Sprite
5. Duplicate sprite untuk icon (klik kanan → Create → Sprite)
6. Resize jadi 128x128px (optional)

---

#### **Opsi C: Buat Sprite Baru (Jika Belum Ada)**

**Tools:** Photoshop, GIMP, Aseprite, atau Paint.NET

**Langkah:**
1. Buat file baru 128x128px
2. Background: Transparan
3. Gambar icon tong sederhana:
   - **Organik:** Tong hijau dengan simbol daun 🌿
   - **Anorganik:** Tong kuning dengan simbol recycle ♻️
   - **B3:** Tong merah dengan simbol tengkorak ☠️
4. Export sebagai PNG (transparansi preserved)
5. Nama file:
   - `Icon_Tong_Organik.png`
   - `Icon_Tong_Anorganik.png`
   - `Icon_Tong_B3.png`

---

#### **Import Sprite ke Unity:**

1. Drag file PNG ke Unity Project window
2. Folder yang disarankan: **`Assets/Art/UI/Icons/`**
3. Pilih sprite di Project window
4. Di Inspector, setting:
   ```
   Texture Type: Sprite (2D and UI)
   Sprite Mode: Single
   Pixels Per Unit: 100
   Filter Mode: Bilinear
   Compression: None (atau Default)
   ```
5. Klik **Apply**

---

## 🧪 **TESTING & DEBUG** {#testing}

### **LANGKAH 7: Test Judgment Phase**

#### **7.1. Setup Testing Scene**
1. Play scene **`03_Game_Processing`**
2. **SKIP BRIEFING** (klik tombol Mulai cepat)
3. **SKIP SORTING GUIDE** (klik tombol Lanjut)

---

#### **7.2. Buat Kesalahan Sengaja**

**Strategi Testing:**
1. Saat sampah spawn di conveyor belt
2. **Drag sampah ke tong YANG SALAH** (sengaja)
   - Contoh: Apel (Organik) → Drag ke Tong Anorganik ❌
   - Contoh: Botol Plastik (Anorganik) → Drag ke Tong B3 ❌
3. Lakukan ini **3-5 kali** untuk sampah BERBEDA
4. Tunggu sampai semua sampah selesai (atau timer habis)

---

#### **7.3. Verifikasi Judgment Phase**

**Setelah level selesai, cek:**

✅ **Panel Judgment muncul** (background gelap)
✅ **Slide pertama ditampilkan** dengan:
   - Icon sampah yang salah
   - Nama sampah
   - Icon tong yang kamu pilih (salah) dengan ❌
   - Icon tong yang benar dengan ✅
   - Penjelasan edukatif di panel kuning
   - Text progress "Kesalahan 1/3"

✅ **Setelah 3.5 detik**, slide **berganti otomatis** ke kesalahan berikutnya

✅ **Setelah semua slide selesai**, Panel Judgment **hilang** dan **Win Panel muncul**

---

#### **7.4. Cek Console Log**

Di Unity Console, kamu harus lihat log seperti ini:

```
❌ [JUDGMENT] Kesalahan dicatat: Apel (Seharusnya: Organik, Dipilih: Anorganik)
📊 [JUDGMENT] Total kesalahan unik: 1

🎉 WIN CONDITION TERCAPAI! Memanggil LevelSelesai()...

==================================================
[JUDGMENT PHASE] START
[JUDGMENT] ❌ Ditemukan 3 kesalahan unik.
[JUDGMENT] 🎬 Memulai slideshow edukatif...
==================================================

[JUDGMENT] Menampilkan slide 1/3: Apel
[JUDGMENT] Menampilkan slide 2/3: Botol Plastik
[JUDGMENT] Menampilkan slide 3/3: Baterai

[JUDGMENT] Slideshow selesai!
[JUDGMENT] 🎉 Slideshow selesai! Menampilkan Win Panel...

==================================================
[SHOW WIN PANEL] Menampilkan Panel Menang
✅ winPanel ditemukan: Panel_Win. Mengaktifkan panel...
✅ Win Panel ditampilkan!
==================================================
```

---

#### **7.5. Test Case: Tidak Ada Kesalahan**

1. Play scene lagi
2. Kali ini, pilah sampah dengan **BENAR** semua
3. Setelah level selesai:
   - ✅ Panel Judgment **TIDAK MUNCUL**
   - ✅ Win Panel **LANGSUNG TAMPIL**
   - ✅ Console log: `[JUDGMENT] ✅ Tidak ada kesalahan! Langsung tampilkan Win Panel.`

---

#### **7.6. Test Deduplication**

1. Play scene lagi
2. Drag **Apel (Organik)** ke **Tong Anorganik** → **5 KALI BERTURUT-TURUT**
3. Cek Console:
   ```
   ❌ [JUDGMENT] Kesalahan dicatat: Apel (Seharusnya: Organik, Dipilih: Anorganik)
   ⚠️ [JUDGMENT] Kesalahan 'Apel' sudah dicatat sebelumnya. Skip duplicate.
   ⚠️ [JUDGMENT] Kesalahan 'Apel' sudah dicatat sebelumnya. Skip duplicate.
   ⚠️ [JUDGMENT] Kesalahan 'Apel' sudah dicatat sebelumnya. Skip duplicate.
   ⚠️ [JUDGMENT] Kesalahan 'Apel' sudah dicatat sebelumnya. Skip duplicate.
   ```
4. Setelah level selesai:
   - ✅ Slideshow hanya menampilkan **1 SLIDE** untuk Apel (bukan 5 slide)

---

## 🔧 **TROUBLESHOOTING** {#troubleshooting}

### **Problem 1: Panel Judgment Tidak Muncul**

**Gejala:**
- Level selesai → Langsung Win Panel (tidak ada slideshow)

**Solusi:**
1. **Cek Console Log:**
   - Apakah ada log `[JUDGMENT PHASE] START`?
   - Jika tidak, berarti `StartJudgmentPhase()` tidak dipanggil

2. **Cek Link di ProcessingLevelManager:**
   - Pilih LevelManager di Hierarchy
   - Inspector → **ProcessingLevelManager** component
   - Field **Judgment Slideshow** harus ter-isi (tidak None)

3. **Cek GameManager Instance:**
   - Play scene
   - Console → Cari error `GameManager TIDAK DITEMUKAN`
   - Pastikan GameManager ada di scene dan DontDestroyOnLoad aktif

4. **Cek Ada Kesalahan:**
   - Judgment hanya muncul jika ada kesalahan
   - Test dengan SENGAJA salah pilah sampah

---

### **Problem 2: Slide Kosong / Icon Tidak Muncul**

**Gejala:**
- Panel Judgment muncul, tapi icon sampah/tong kosong

**Solusi:**
1. **Cek Sprite Assignment:**
   - Pilih **Panel_JudgmentPhase**
   - Inspector → **JudgmentSlideshow** component
   - Section **Bin Sprites** → Semua field harus ter-isi (bukan None)

2. **Cek WasteData ScriptableObject:**
   - Di Project window, cari **`Assets/Data/WasteData_[NamaSampah].asset`**
   - Buka (klik)
   - Field **Icon Sampah** harus ada sprite
   - Jika kosong, assign sprite

3. **Cek Image Component:**
   - Pilih **Image_WasteIcon** di Hierarchy
   - Inspector → **Image** component
   - Pastikan **Color** alpha tidak 0 (harus 255)
   - Pastikan **Enabled** dicentang

---

### **Problem 3: Text Tidak Muncul**

**Gejala:**
- Icon muncul, tapi text nama/penjelasan kosong

**Solusi:**
1. **Cek Link Text:**
   - Pilih **Panel_JudgmentPhase**
   - Inspector → **JudgmentSlideshow**
   - Section **UI References** → Semua Text field harus ter-isi

2. **Cek TextMeshPro:**
   - Pastikan kamu pakai **Text - TextMeshPro** (bukan Text biasa)
   - Jika pakai Text biasa, ganti jadi TextMeshPro

3. **Cek Font Asset:**
   - Pilih text object (contoh: `Text_WasteName`)
   - Inspector → **TextMeshProUGUI**
   - Field **Font Asset** harus ada font (bukan None)
   - Jika None, assign font default: `LiberationSans SDF`

---

### **Problem 4: Slideshow Terlalu Cepat/Lambat**

**Gejala:**
- Slide berganti terlalu cepat, tidak sempat baca
- Atau terlalu lambat, boring

**Solusi:**
1. Pilih **Panel_JudgmentPhase**
2. Inspector → **JudgmentSlideshow** component
3. Ubah **Slide Duration:**
   - Terlalu cepat? Naikan jadi **5.0** detik
   - Terlalu lambat? Turunkan jadi **2.5** detik
   - Sweet spot: **3.0 - 4.0** detik

---

### **Problem 5: Error "NullReferenceException"**

**Gejala:**
- Console error merah:
  ```
  NullReferenceException: Object reference not set to an instance of an object
  JudgmentSlideshow.UpdateSlide (...)
  ```

**Solusi:**
1. **Cek Semua Field di Inspector:**
   - Pilih **Panel_JudgmentPhase**
   - Inspector → **JudgmentSlideshow**
   - **SEMUA FIELD** harus ter-isi (tidak ada yang None)
   
2. **Re-link Field yang None:**
   - Cari field yang bertulisan **None (GameObject)** atau **None (Sprite)**
   - Drag object/sprite yang sesuai dari Hierarchy/Project window

3. **Reset Component (Last Resort):**
   - Klik kanan **JudgmentSlideshow** component → **Remove Component**
   - Add lagi: **Add Component** → **JudgmentSlideshow**
   - Link semua field dari awal (ikuti LANGKAH 4)

---

### **Problem 6: Win Panel Tidak Muncul Setelah Slideshow**

**Gejala:**
- Slideshow selesai → Layar freeze/hitam → Win Panel tidak muncul

**Solusi:**
1. **Cek Console Log:**
   - Apakah ada log `[JUDGMENT] 🎉 Slideshow selesai! Menampilkan Win Panel...`?
   - Jika tidak, callback tidak jalan

2. **Cek GameManager.ShowWinPanel():**
   - Play scene → Pause saat slideshow selesai
   - Console → Cari error `winPanel NULL`
   - Pastikan **Panel Win** di-link di **ProcessingLevelManager** Inspector

3. **Cek Time.timeScale:**
   - Add Debug.Log di `GameManager.ShowWinPanel()`:
     ```csharp
     Debug.Log($"Time.timeScale: {Time.timeScale}");
     ```
   - Jika bukan 0 atau 1, ada bug timing

---

### **Problem 7: Kesalahan Tidak Tercatat**

**Gejala:**
- Sengaja salah pilah → Slideshow tidak muncul (atau kosong)
- Console tidak ada log `❌ [JUDGMENT] Kesalahan dicatat`

**Solusi:**
1. **Cek DragController:**
   - Buka file **`DragController.cs`**
   - Cari fungsi `ProsesPemilahan()`
   - Pastikan ada line:
     ```csharp
     GameManager.Instance.RecordMistake(
         myItem.dataSampah.namaSampah,
         myItem.dataSampah.tipeSampah,
         bin.tipeTongIni,
         myItem.dataSampah.iconSampah
     );
     ```

2. **Test Manual Recording:**
   - Tambah button test di scene
   - OnClick → Call:
     ```csharp
     GameManager.Instance.RecordMistake(
         "Test Sampah",
         WasteType.Organik,
         WasteType.Anorganik,
         null
     );
     ```
   - Cek Console → Harus ada log kesalahan

---

## ✅ **CHECKLIST FINAL**

Sebelum selesai, cek semua ini:

### **1. File Script:**
- [ ] `GameManager.cs` sudah ada class `MistakeRecord`
- [ ] `DragController.cs` panggil `RecordMistake()` saat salah
- [ ] `ProcessingLevelManager.cs` sudah ada `StartJudgmentPhase()`
- [ ] `JudgmentSlideshow.cs` ada di folder `Assets/_Scripts/UI/`

### **2. Hierarchy:**
- [ ] `Panel_JudgmentPhase` ada di Canvas scene 03_Game_Processing
- [ ] Semua child objects (10+ objects) sudah dibuat sesuai tutorial
- [ ] `Panel_JudgmentPhase` di-disable (inactive) di awal

### **3. Inspector - JudgmentSlideshow:**
- [ ] 8 field **UI References** semua ter-link (tidak None)
- [ ] 3 field **Bin Sprites** semua ter-assign (tidak None)
- [ ] **Slide Duration** diset (default: 3.5)
- [ ] **Judgment Panel** ter-link ke Panel_JudgmentPhase

### **4. Inspector - ProcessingLevelManager:**
- [ ] Field **Judgment Slideshow** ter-link ke Panel_JudgmentPhase (dengan component JudgmentSlideshow)

### **5. Sprites:**
- [ ] Sprite icon Tong Organik (hijau) ada
- [ ] Sprite icon Tong Anorganik (kuning) ada
- [ ] Sprite icon Tong B3 (merah) ada
- [ ] Semua WasteData.asset punya **Icon Sampah** ter-assign

### **6. Testing:**
- [ ] Test dengan kesalahan → Slideshow muncul
- [ ] Test tanpa kesalahan → Langsung Win Panel
- [ ] Test deduplication → 5x salah Apel = 1 slide
- [ ] Console log tidak ada error merah
- [ ] Penjelasan edukatif muncul dan sesuai

---

## 🎉 **SELESAI!**

Judgment Phase sudah berhasil diimplementasikan! 

**Fitur yang Sudah Aktif:**
- ✅ Tracking kesalahan otomatis
- ✅ Deduplication (hanya 1x per jenis kesalahan)
- ✅ Slideshow edukatif dengan penjelasan kontekstual
- ✅ Visual feedback (icon sampah + tong salah vs benar)
- ✅ Flow mulus: Level Selesai → Judgment → Win Panel

**Next Steps (Optional):**
1. **Custom Penjelasan:** Edit database di `JudgmentSlideshow.GetExplanation()` untuk penjelasan lebih spesifik
2. **Animasi:** Tambahkan DOTween untuk smooth slide transitions
3. **Audio:** Tambahkan SFX saat slideshow (voice narrator atau beep sound)
4. **Skip Button:** Tambahkan tombol "Lewati" untuk pemain yang sudah paham
5. **Achievement:** Beri badge "Perfect!" jika 0 kesalahan di level

---

**📞 Butuh Bantuan Lebih Lanjut?**
Jika ada error atau bingung, cek section **[TROUBLESHOOTING](#troubleshooting)** di atas!

---

**Tanggal Tutorial:** 7 Desember 2025  
**Versi Unity:** 2021.3 LTS  
**Status:** ✅ **PRODUCTION READY**
