# 🎨 TUTORIAL LENGKAP - TONG MARAH FEEDBACK
## Panduan Setup Unity untuk Anak SD (Usia 7-12 Tahun)

---

## 🌟 **APA ITU TONG MARAH?**

Hai adik-adik! Kalau kalau salah buang sampah, tong sampah bakal **marah** lho! 😠

Bayangkan:
- **Tong Hijau** (Organik) → Kalo dikasih baterai, dia bilang: "Hei! Aku bukan tempat baterai! 😢"
- **Tong Kuning** (Anorganik) → Kalo dikasih apel, dia bilang: "Aduh! Apel bukan plastik! 😠"  
- **Tong Merah** (B3) → Kalo dikasih kertas, dia bilang: "Aku cuma untuk sampah berbahaya! 😤"

**Ini adalah feedback edukatif yang FUN untuk belajar!** 🎓✨

---

## 📂 **PERSIAPAN: APA YANG SUDAH DIBUAT?**

### ✅ **File Script (Sudah Selesai)**

Kamu **TIDAK PERLU** coding! Semua script sudah siap:

1. **GameManager.cs** → Otak game yang nyimpen data kesalahan
2. **DragController.cs** → Catat sampah yang salah masuk ke tong
3. **ProcessingLevelManager.cs** → Manager yang ngatur kapan tong marah muncul
4. **JudgmentSlideshow.cs** → Script yang bikin tong marah tampil

### 🎨 **Yang Perlu Kamu Siapkan:**

#### **Asset Gambar (3 File PNG):**

Kamu perlu 3 gambar **FULL ILLUSTRATION** tong marah:

1. **Tong_Organik_Marah.png** (Tong Hijau yang marah)
   - Ukuran: 512x512px atau 1024x1024px
   - Dalam gambar sudah ada:
     - Tong hijau dengan wajah marah 😠
     - Dialog box (balon kata seperti komik)
     - Background transparan (PNG)

2. **Tong_Anorganik_Marah.png** (Tong Kuning yang marah)
   - Ukuran: 512x512px atau 1024x1024px
   - Dalam gambar sudah ada:
     - Tong kuning dengan wajah marah 😤
     - Dialog box (balon kata seperti komik)
     - Background transparan (PNG)

3. **Tong_B3_Marah.png** (Tong Merah yang marah)
   - Ukuran: 512x512px atau 1024x1024px
   - Dalam gambar sudah ada:
     - Tong merah dengan wajah marah 😡
     - Dialog box (balon kata seperti komik)
     - Background transparan (PNG)

**❓ Belum Punya Gambar?**
- Bisa minta orang tua/guru bantuin gambar
- Atau pakai aplikasi: Canva, Photoshop, Paint.NET
- Atau AI art generator: Leonardo.ai, Bing Image Creator

---

## 🏗️ **LANGKAH 1: IMPORT GAMBAR TONG MARAH**

### **1.1. Buka Unity**
1. Buka project **Eco-Quest** di Unity
2. Tunggu sampai loading selesai

### **1.2. Buat Folder Baru**
1. Di bawah (panel **Project**), lihat folder **Assets**
2. Klik kanan **Assets** → **Create** → **Folder**
3. Nama folder: **`TongMarah`**
4. Tekan Enter

### **1.3. Import Gambar**
1. Cari 3 file gambar tong marah di komputer kamu (File Explorer)
2. **Drag** (seret) 3 gambar ke folder **TongMarah** di Unity
3. Tunggu Unity import gambar (muncul loading bar)

### **1.4. Setup Gambar**
Untuk **SETIAP gambar** (lakukan 3x):

1. **Klik** gambar di folder TongMarah
2. Di sebelah kanan (panel **Inspector**), ubah setting:
   ```
   Texture Type: Sprite (2D and UI)  ← Klik dropdown, pilih ini
   Sprite Mode: Single
   Pixels Per Unit: 100
   ```
3. **Klik tombol "Apply"** di bawah (PENTING!)

---

## 🎮 **LANGKAH 2: BUKA SCENE PROCESSING**

### **2.1. Buka Scene**
1. Di panel **Project** (bawah), cari folder **`Assets/_Scenes`**
2. **Double-click** file **`03_Game_Processing`**
3. Scene akan terbuka (lihat di panel **Scene** di tengah)

### **2.2. Lihat Hierarchy**
1. Di sebelah kiri, ada panel **Hierarchy** (daftar semua object di scene)
2. Cari object bernama **`Canvas`** (ini adalah tempat semua UI)

---

## 🖼️ **LANGKAH 3: BUAT PANEL TONG MARAH**

### **3.1. Buat Panel Utama**

1. **Klik kanan** pada **Canvas** di Hierarchy
2. Pilih **UI** → **Panel**
3. Rename (ganti nama):
   - Klik panel baru yang muncul
   - Tekan **F2** (atau klik 2x pelan)
   - Ketik: **`Panel_TongMarah`**
   - Tekan **Enter**

4. **Setting Panel** (di Inspector sebelah kanan):
   - Cari **Rect Transform** (yang pertama)
   - Klik gambar **kotak di tengah** (anchor preset)
   - **Tekan Alt + Shift** sambil klik **kotak kanan bawah** (stretch stretch)
   - Sekarang panel full screen!

5. **Warna Background:**
   - Scroll ke bawah, cari **Image** component
   - Klik **Color** (kotak warna)
   - Ubah jadi **Hitam Semi-Transparan**:
     - R: 0
     - G: 0
     - B: 0
     - **A: 200** ← Ini penting! (Biar tidak terlalu gelap)
   - Klik OK

---

### **3.2. Buat Container Slide**

1. **Klik kanan** pada **Panel_TongMarah** (yang baru kamu buat)
2. Pilih **UI** → **Image**
3. Rename jadi: **`Image_TongMarah`**

4. **Setting di Inspector:**
   - **Rect Transform:**
     - Width: **600** ← Ketik di kotak Width
     - Height: **600** ← Ketik di kotak Height
     - Anchor: **Middle Center** (klik kotak tengah-tengah)
     - Pos X: **0**
     - Pos Y: **0**

5. **Image Component:**
   - Cari **Source Image** (dropdown)
   - **JANGAN ISI DULU** (nanti diisi script otomatis)
   - Color: **Putih** (R:255, G:255, B:255, A:255)
   - **Centang** kotak **Preserve Aspect** ✅

---

### **3.3. Buat Text Dialog Tong**

1. **Klik kanan** pada **Image_TongMarah**
2. Pilih **UI** → **Text - TextMeshPro**
3. **PENTING:** Jika muncul popup "Import TMP Essentials":
   - Klik tombol **"Import TMP Essentials"**
   - Tunggu selesai
   - Klik **"Import TMP Examples & Extras"** (optional)
4. Rename jadi: **`Text_Dialog`**

5. **Setting di Inspector:**
   - **Rect Transform:**
     - Width: **500**
     - Height: **300**
     - Anchor: **Bottom Center** (klik kotak tengah-bawah)
     - Pos X: **0**
     - Pos Y: **-50**

   - **TextMeshProUGUI Component:**
     - **Text:** Ketik: `Dialog tong akan muncul di sini...` (sementara)
     - **Font Size:** **24**
     - **Color:** **Hitam** (R:0, G:0, B:0)
     - **Alignment:** Klik tombol **Center & Top** (tengah-atas)
     - **Wrapping:** Centang **Enabled** ✅
     - **Overflow:** Pilih **Overflow** (dari dropdown)

---

### **3.4. Buat Container Icon Sampah**

Ini tempat icon sampah akan "menumpuk" nanti!

1. **Klik kanan** pada **Image_TongMarah**
2. Pilih **Create Empty** (object kosong)
3. Rename jadi: **`Container_WasteIcons`**

4. **Setting di Inspector:**
   - **Rect Transform:**
     - Width: **400**
     - Height: **200**
     - Anchor: **Top Center**
     - Pos X: **0**
     - Pos Y: **-250** ← Posisi di bawah gambar tong

---

### **3.5. Buat Prefab Icon Sampah**

Ini adalah "cetakan" untuk icon sampah yang akan di-spawn.

1. **Klik kanan** di panel **Project** → folder **Assets**
2. **Create** → **Folder** → Nama: **`Prefabs`** (jika belum ada)
3. **Klik kanan** di dalam folder **Prefabs**
4. Pilih **Create** → **Folder** → Nama: **`UI`**

Sekarang buat prefab:

1. Di **Hierarchy**, **klik kanan** pada **Canvas**
2. **UI** → **Image**
3. Rename jadi: **`Prefab_WasteIcon`**

4. **Setting di Inspector:**
   - **Rect Transform:**
     - Width: **80**
     - Height: **80**
   - **Image Component:**
     - Source Image: **KOSONGKAN** (akan diisi script)
     - Color: Putih (R:255, G:255, B:255, A:255)
     - **Centang** Preserve Aspect ✅

5. **Bikin Jadi Prefab:**
   - Di **Hierarchy**, **drag** object **Prefab_WasteIcon** ke folder **Assets/Prefabs/UI**
   - Sekarang ada file prefab di Project window
   - **DELETE** object **Prefab_WasteIcon** di Hierarchy (klik kanan → Delete)
   - Prefab sudah tersimpan, tidak perlu di scene

---

### **3.6. Buat Text Progress**

1. **Klik kanan** pada **Panel_TongMarah**
2. **UI** → **Text - TextMeshPro**
3. Rename jadi: **`Text_Progress`**

4. **Setting di Inspector:**
   - **Rect Transform:**
     - Width: **300**
     - Height: **50**
     - Anchor: **Top Center**
     - Pos X: **0**
     - Pos Y: **-30** ← Di pojok atas

   - **TextMeshProUGUI:**
     - Text: `Tong 1 dari 2` (contoh)
     - Font Size: **28**
     - Color: **Putih** (R:255, G:255, B:255)
     - Alignment: **Center & Middle**
     - Font Style: **Bold** (klik tombol **B**)

---

### **3.7. Matikan Panel (PENTING!)**

1. **Klik** object **Panel_TongMarah** di Hierarchy
2. Di **Inspector**, lihat di paling atas (nama object)
3. **Uncheck** kotak di samping nama (disable object)
4. Panel sekarang **tidak aktif** (warnanya abu-abu di Hierarchy)
   - Ini benar! Panel akan diaktifkan script nanti saat level selesai

---

## 🔗 **LANGKAH 4: LINK SCRIPT KE UI**

### **4.1. Attach Script ke Panel**

1. **Klik** object **Panel_TongMarah** di Hierarchy
2. Di **Inspector**, scroll ke bawah
3. **Klik tombol "Add Component"**
4. Ketik: **`JudgmentSlideshow`** di kotak search
5. **Klik** script **JudgmentSlideshow** untuk attach

---

### **4.2. Link UI References**

Sekarang kita **hubungkan** semua UI ke script.

Di **Inspector**, scroll ke section **JudgmentSlideshow** component.

**A. UI References - Slide Components:**

Ini yang perlu di-drag (seret) dari Hierarchy ke Inspector:

```
┌──────────────────────────────────────────────────┐
│ Image Tong Marah:    [Image_TongMarah]          │ ← Drag dari Hierarchy
│ Text Dialog:         [Text_Dialog]              │ ← Drag dari Hierarchy
│ Container Waste Icons: [Container_WasteIcons]   │ ← Drag dari Hierarchy
│ Prefab Waste Icon:   [Prefab_WasteIcon]         │ ← Drag dari Project (folder Prefabs/UI)
│ Text Progress:       [Text_Progress]            │ ← Drag dari Hierarchy
└──────────────────────────────────────────────────┘
```

**Cara Drag:**
1. Klik object di **Hierarchy** (contoh: `Image_TongMarah`)
2. **Tahan** mouse (jangan lepas!)
3. **Seret** ke kotak field di **Inspector** yang sesuai
4. **Lepas** mouse
5. Field akan berubah biru → **Berhasil!** ✅

**Untuk Prefab_WasteIcon:**
1. Buka folder **Assets/Prefabs/UI** di panel **Project**
2. **Drag** file **Prefab_WasteIcon** (biru) ke field **Prefab Waste Icon**

---

**B. Full Illustration Sprites - Tong Marah:**

Link 3 gambar tong marah yang sudah kamu import:

```
┌──────────────────────────────────────────────────┐
│ Sprite Tong Organik Marah:   [Tong_Organik_Marah.png]   │
│ Sprite Tong Anorganik Marah: [Tong_Anorganik_Marah.png] │
│ Sprite Tong B3 Marah:        [Tong_B3_Marah.png]        │
└──────────────────────────────────────────────────┘
```

**Cara:**
1. Buka folder **Assets/TongMarah** di panel Project
2. **Drag** gambar **Tong_Organik_Marah** ke field pertama
3. **Drag** gambar **Tong_Anorganik_Marah** ke field kedua
4. **Drag** gambar **Tong_B3_Marah** ke field ketiga

---

**C. Settings - Tampilan Sampah:**

Ini untuk atur tampilan icon sampah yang menumpuk:

```
┌──────────────────────────────────────────────────┐
│ Waste Icon Size:         [80]                    │ ← Ukuran icon (pixel)
│ Random Offset Range:     [30]                    │ ← Jarak antar icon (efek menumpuk)
│ Random Rotation Range:   [15]                    │ ← Rotasi acak (derajat)
└──────────────────────────────────────────────────┘
```

**Penjelasan:**
- **Waste Icon Size:** Berapa besar icon sampah (80px = pas untuk anak SD)
- **Random Offset Range:** Berapa jauh icon bergeser (30 = menumpuk tapi tidak terlalu berantakan)
- **Random Rotation Range:** Berapa miring icon (15° = sedikit miring, terlihat natural)

**Kamu bisa ubah angka ini nanti kalau mau!**

---

**D. Settings - Timing:**

```
┌──────────────────────────────────────────────────┐
│ Slide Duration:          [5.0]                   │ ← Berapa lama tampil (detik)
│ Judgment Panel:          [Panel_TongMarah]       │ ← Drag Panel_TongMarah dari Hierarchy
└──────────────────────────────────────────────────┘
```

**Slide Duration:**
- **5 detik** = Pas untuk anak SD (cukup waktu baca dialog)
- Kalau terlalu cepat, naikan jadi **6** atau **7** detik
- Kalau terlalu lambat, turunkan jadi **4** detik

**Judgment Panel:**
- Drag object **Panel_TongMarah** dari Hierarchy ke field ini

---

## 🎯 **LANGKAH 5: LINK KE PROCESSING LEVEL MANAGER**

Sekarang kita hubungkan Panel_TongMarah ke manager level.

### **5.1. Cari Level Manager**

1. Di **Hierarchy**, cari object bernama:
   - **`LevelManager`** ATAU
   - **`ProcessingManager`** ATAU
   - Object yang punya script **ProcessingLevelManager**

2. **Klik** object tersebut

---

### **5.2. Link Judgment Slideshow**

1. Di **Inspector**, scroll ke component **ProcessingLevelManager**
2. Cari section: **Judgment Phase System**
3. Ada field: **Judgment Slideshow**

4. **Drag** object **Panel_TongMarah** (yang punya component JudgmentSlideshow) ke field ini

Unity akan otomatis detect component JudgmentSlideshow!

---

## ✅ **LANGKAH 6: SAVE SCENE**

**PENTING! Jangan lupa save!**

1. **Tekan Ctrl + S** (Windows) atau **Cmd + S** (Mac)
2. Atau klik **File** → **Save**
3. Lihat bintang (*) di tab scene hilang = **Tersimpan!** ✅

---

## 🧪 **LANGKAH 7: TESTING - UJI COBA TONG MARAH**

### **7.1. Play Scene**

1. **Klik tombol PLAY** (▶️) di atas Unity Editor
2. Game akan mulai di Scene view

---

### **7.2. Skip Briefing**

1. Saat muncul dialog guru (briefing):
   - **Klik "Next"** sampai selesai
   - **Klik "Mulai"**

2. Saat muncul **Panduan Sortir** (panel kuning):
   - **Klik "Lanjut"**

3. Sekarang game mulai! Sampah akan muncul di conveyor belt (ban berjalan)

---

### **7.3. Buat Kesalahan (Sengaja!)**

**PENTING:** Kita harus **sengaja salah** untuk test tong marah!

**Cara Salah Pilah:**
1. Lihat sampah yang muncul (contoh: **Apel**)
2. **Drag** (seret) sampah ke tong yang **SALAH**
   - Apel (Organik) → **Seret ke Tong Kuning (Anorganik)** ❌
   - Botol Plastik (Anorganik) → **Seret ke Tong Merah (B3)** ❌
   - Baterai (B3) → **Seret ke Tong Hijau (Organik)** ❌

3. Lakukan **3-5 kali** untuk sampah **BERBEDA**
   - Jangan sama (contoh: jangan 5x Apel semua)
   - Coba variasi (Apel, Botol, Baterai, Kertas, dll)

4. Tunggu sampai **semua sampah selesai** (atau timer habis)

---

### **7.4. CEK TONG MARAH MUNCUL!**

Setelah level selesai, seharusnya:

✅ **Panel Tong Marah muncul** (background gelap)

✅ **Slide 1: Tong yang Marah**
   - Gambar FULL tong marah (sesuai yang kena salah pilah)
   - Dialog tong marah dengan bahasa anak-anak
   - Icon sampah **menumpuk berserakan** di dalam dialog box
   - Progress text: "Tong 1 dari 2"

✅ **Setelah 5 detik:** Slide berganti ke tong lain (jika ada)

✅ **Setelah semua slide:** Panel hilang → **Panel Win muncul**

---

### **7.5. Cek Console Log**

Di Unity Editor bawah, ada panel **Console**.

**Jika berhasil**, kamu akan lihat log seperti ini:

```
❌ [JUDGMENT] Tong Anorganik salah terima: Apel (Seharusnya: Organik)
📊 [JUDGMENT] Tong Anorganik total salah terima: 1 sampah

❌ [JUDGMENT] Tong B3 salah terima: Botol Plastik (Seharusnya: Anorganik)
📊 [JUDGMENT] Tong B3 total salah terima: 1 sampah

🎉 WIN CONDITION TERCAPAI! Memanggil LevelSelesai()...

😠 Ada 2 tong yang marah! Memulai slideshow...

😠 Slide 1/2: Tong Anorganik menerima 1 sampah salah
😠 Slide 2/2: Tong B3 menerima 1 sampah salah

🎉 Slideshow selesai! Semua tong sudah protes!
```

**Jika ADA ERROR MERAH**, baca section **TROUBLESHOOTING** di bawah!

---

### **7.6. Test Tanpa Kesalahan**

1. **Stop game** (klik tombol STOP ⏹️)
2. **Play lagi** (▶️)
3. Kali ini, **pilah dengan BENAR** semua sampah
   - Apel (Organik) → Tong Hijau ✅
   - Botol (Anorganik) → Tong Kuning ✅
   - Baterai (B3) → Tong Merah ✅

4. Setelah level selesai:
   - ✅ Panel Tong Marah **TIDAK MUNCUL** (karena tidak ada kesalahan)
   - ✅ **Panel Win langsung tampil**
   - ✅ Console log: `🎉 Tidak ada kesalahan! Semua tong senang!`

---

## 🐛 **TROUBLESHOOTING - KALAU ADA MASALAH**

### **Problem 1: Panel Tong Marah Tidak Muncul**

**Gejala:**
- Level selesai → Langsung Panel Win (tidak ada tong marah)

**Cek 1: Apakah Sudah Salah Pilah?**
- Tong marah HANYA muncul kalau ada kesalahan
- Coba sengaja salah pilah sampah
- Cek Console: Harus ada log `❌ [JUDGMENT] Tong ... salah terima ...`

**Cek 2: Link di ProcessingLevelManager**
1. Klik **LevelManager** di Hierarchy
2. Inspector → **ProcessingLevelManager**
3. Field **Judgment Slideshow** harus ter-isi (ada text `Panel_TongMarah (JudgmentSlideshow)`)
4. Jika **None (JudgmentSlideshow)**, ulangi **LANGKAH 5**

**Cek 3: Script Component**
1. Klik **Panel_TongMarah** di Hierarchy
2. Inspector → Harus ada component **JudgmentSlideshow**
3. Jika tidak ada, ulangi **LANGKAH 4.1**

---

### **Problem 2: Gambar Tong Tidak Muncul**

**Gejala:**
- Panel muncul tapi gambar tong kosong/putih

**Solusi:**
1. Klik **Panel_TongMarah** di Hierarchy
2. Inspector → **JudgmentSlideshow** component
3. Section **Full Illustration Sprites:**
   - Semua 3 field harus ter-isi (tidak **None**)
   - Jika **None**, ulangi **LANGKAH 4.2 - B**

**Cek Import Gambar:**
1. Klik gambar di **Assets/TongMarah**
2. Inspector → **Texture Type** harus **Sprite (2D and UI)**
3. Jika salah, ulangi **LANGKAH 1.4**

---

### **Problem 3: Icon Sampah Tidak Muncul**

**Gejala:**
- Panel dan tong muncul, tapi tidak ada icon sampah

**Solusi:**
1. Klik **Panel_TongMarah** di Hierarchy
2. Inspector → **JudgmentSlideshow**
3. Field **Prefab Waste Icon** harus ter-isi (ada text `Prefab_WasteIcon`)
4. Jika **None**, ulangi **LANGKAH 4.2 - A** (bagian Prefab)

**Cek Prefab:**
1. Buka folder **Assets/Prefabs/UI** di Project
2. Harus ada file **Prefab_WasteIcon** (icon biru)
3. Jika tidak ada, ulangi **LANGKAH 3.5**

---

### **Problem 4: Text Dialog Kosong**

**Gejala:**
- Gambar tong muncul tapi text dialog kosong

**Solusi:**
1. Klik **Panel_TongMarah** di Hierarchy
2. Inspector → **JudgmentSlideshow**
3. Field **Text Dialog** harus ter-isi
4. Jika **None**, drag **Text_Dialog** dari Hierarchy

**Cek Text Component:**
1. Klik **Text_Dialog** di Hierarchy
2. Inspector → Harus ada component **TextMeshProUGUI**
3. Font Asset harus ter-isi (tidak **None**)
4. Jika None, pilih font: **LiberationSans SDF** dari dropdown

---

### **Problem 5: Error Merah di Console**

**Error: "NullReferenceException"**

**Artinya:** Ada yang tidak ter-link di Inspector

**Solusi:**
1. Klik **Panel_TongMarah** di Hierarchy
2. Inspector → **JudgmentSlideshow**
3. **CEK SEMUA FIELD:**
   - Tidak ada yang bertulisan **None** (kecuali yang memang kosong)
   - Re-drag semua field (ulangi **LANGKAH 4.2**)

---

### **Problem 6: Slideshow Terlalu Cepat/Lambat**

**Gejala:**
- Slide berganti terlalu cepat, anak tidak sempat baca
- Atau terlalu lambat, anak bosan

**Solusi:**
1. **Stop game** (⏹️)
2. Klik **Panel_TongMarah** di Hierarchy
3. Inspector → **JudgmentSlideshow**
4. Ubah **Slide Duration:**
   - Terlalu cepat? Naikan jadi **7.0** atau **8.0** detik
   - Terlalu lambat? Turunkan jadi **3.5** atau **4.0** detik
   - **Rekomendasi untuk anak SD: 5-6 detik** ⭐
5. **Save scene** (Ctrl + S)
6. **Play lagi** untuk test

---

### **Problem 7: Icon Sampah Tidak Menumpuk**

**Gejala:**
- Icon muncul tapi terlalu rapi/grid, tidak berserakan

**Solusi:**
1. Klik **Panel_TongMarah** di Hierarchy
2. Inspector → **JudgmentSlideshow**
3. Section **Settings - Tampilan Sampah:**
   - **Random Offset Range:** Naikan jadi **40** atau **50** (lebih berserakan)
   - **Random Rotation Range:** Naikan jadi **20** atau **25** (lebih miring)
4. Save + Play lagi

**Ingin Lebih Berantakan?**
- Offset: **60** (sangat berserakan)
- Rotation: **30** (sangat miring)

**Ingin Lebih Rapi?**
- Offset: **15** (hampir grid)
- Rotation: **5** (hampir lurus)

---

## 🎨 **TIPS UNTUK GURU/ORANG TUA**

### **Membuat Gambar Tong Marah (Simple)**

#### **Opsi 1: Canva (Gratis)**
1. Buka **Canva.com**
2. Buat desain custom: **1024 x 1024px**
3. Tambahkan:
   - **Ilustrasi tong sampah** (cari di "Elements" → ketik "trash bin")
   - **Wajah marah** (emoji 😠 atau gambar mata + mulut)
   - **Dialog box** (bentuk balon kata dari "Elements" → "Speech bubble")
4. **Export:** PNG dengan **Background transparan** ✅
5. Ulangi 3x untuk 3 warna tong (hijau, kuning, merah)

#### **Opsi 2: AI Art Generator**
1. Buka **Bing Image Creator** (gratis)
2. Prompt (perintah):
   ```
   "Cartoon green trash bin character with angry face and speech bubble,
   simple 2D illustration, PNG transparent background, child-friendly"
   ```
3. Generate → Download
4. Ulangi untuk warna kuning dan merah

#### **Opsi 3: Photoshop/GIMP**
1. Canvas baru: 1024x1024px, background transparan
2. Gambar tong dengan **Pen Tool** atau import gambar
3. Tambah wajah marah (mata + mulut)
4. Tambah dialog box (rounded rectangle + text)
5. Save as **PNG-24** (transparan)

---

### **Mengapa 5 Detik untuk Anak SD?**

Research menunjukkan:
- Anak usia **7-9 tahun:** Kecepatan baca **~100-150 kata/menit**
- Anak usia **10-12 tahun:** Kecepatan baca **~150-200 kata/menit**

Dialog tong marah rata-rata **30-40 kata**.

**Waktu baca:**
- 7-9 tahun: **~15-20 detik** untuk baca + pahami
- 10-12 tahun: **~10-15 detik** untuk baca + pahami

Tapi kita kasih **5 detik** karena:
1. Anak sudah baca briefing sebelumnya (context sudah paham)
2. Visual icon membantu (tidak perlu baca semua)
3. Dialog diulang jika main lagi (spaced repetition)
4. **Terlalu lama = anak bosan**

**Jika untuk usia lebih kecil (5-6 tahun):** Naikan jadi **7-8 detik**.

---

## 📊 **CONTOH DIALOG TONG (Yang Sudah Ada di Script)**

### **Tong Hijau (Organik) Marah:**
```
"Aduh! Aku Tong Hijau untuk sampah organik!

Kok Baterai, Botol Plastik dimasukkan ke sini? 😢

Sampah organik itu yang berasal dari makhluk hidup dan bisa membusuk, 
seperti sisa makanan!"
```

### **Tong Kuning (Anorganik) Marah:**
```
"Hei! Aku Tong Kuning untuk sampah anorganik!

Masa Apel, Kulit Pisang masuk ke sini sih? 😠

Sampah anorganik itu seperti plastik, kertas, dan kaleng 
yang bisa didaur ulang!"
```

### **Tong Merah (B3) Marah:**
```
"AWAS! Aku Tong Merah khusus B3 (Bahan Berbahaya)!

Botol, Kertas bukan sampah berbahaya! 😤

B3 itu seperti baterai, lampu, dan obat-obatan 
yang bisa meracuni lingkungan!"
```

**Dialog ini OTOMATIS berubah** sesuai sampah yang salah masuk! 🎯

---

## ✅ **CHECKLIST FINAL - PASTIKAN SEMUA SUDAH BENAR**

Sebelum selesai, cek semua ini:

### **1. Asset Gambar:**
- [ ] 3 gambar tong marah sudah di-import ke folder **Assets/TongMarah**
- [ ] Semua gambar sudah di-setup **Sprite (2D and UI)**
- [ ] Gambar punya background **transparan** (PNG)

### **2. Hierarchy:**
- [ ] **Panel_TongMarah** ada di Canvas
- [ ] **Image_TongMarah** ada di dalam Panel_TongMarah
- [ ] **Text_Dialog** ada di dalam Image_TongMarah
- [ ] **Container_WasteIcons** ada di dalam Image_TongMarah
- [ ] **Text_Progress** ada di dalam Panel_TongMarah
- [ ] **Panel_TongMarah** di-disable (inactive, warna abu-abu)

### **3. Prefab:**
- [ ] **Prefab_WasteIcon** ada di folder **Assets/Prefabs/UI**
- [ ] Prefab punya component **Image** dengan size **80x80**

### **4. Inspector - JudgmentSlideshow:**
- [ ] Field **Image Tong Marah** → ter-link ke Image_TongMarah ✅
- [ ] Field **Text Dialog** → ter-link ke Text_Dialog ✅
- [ ] Field **Container Waste Icons** → ter-link ke Container_WasteIcons ✅
- [ ] Field **Prefab Waste Icon** → ter-link ke Prefab_WasteIcon ✅
- [ ] Field **Text Progress** → ter-link ke Text_Progress ✅
- [ ] Field **Sprite Tong Organik Marah** → ter-assign gambar hijau ✅
- [ ] Field **Sprite Tong Anorganik Marah** → ter-assign gambar kuning ✅
- [ ] Field **Sprite Tong B3 Marah** → ter-assign gambar merah ✅
- [ ] Field **Slide Duration** → nilai **5.0** (atau sesuai kebutuhan)
- [ ] Field **Judgment Panel** → ter-link ke Panel_TongMarah ✅

### **5. Inspector - ProcessingLevelManager:**
- [ ] Field **Judgment Slideshow** → ter-link ke Panel_TongMarah (JudgmentSlideshow) ✅

### **6. Testing:**
- [ ] Play scene → Sengaja salah pilah → Panel muncul ✅
- [ ] Gambar tong muncul sesuai yang salah terima ✅
- [ ] Dialog tong muncul dengan nama sampah ✅
- [ ] Icon sampah menumpuk berserakan ✅
- [ ] Progress text update (Tong 1 dari 2) ✅
- [ ] Setelah 5 detik, slide berganti ✅
- [ ] Setelah semua slide, Panel Win muncul ✅
- [ ] Console tidak ada error merah ✅

---

## 🎉 **SELESAI! TONG MARAH SUDAH AKTIF!**

**Apa yang Sudah Kamu Buat:**
- ✅ Sistem feedback edukatif dengan **karakter tong yang punya personality**
- ✅ Dialog **dinamis** yang berubah sesuai kesalahan
- ✅ Visual **fun** dengan icon sampah menumpuk berserakan
- ✅ Timing yang **pas untuk anak SD** (5 detik untuk baca + pahami)
- ✅ Pembelajaran **interaktif** dan **tidak membosankan**

**Manfaat untuk Anak:**
- 🧠 Belajar dari kesalahan dengan cara yang **fun** (tidak menakuti)
- 👁️ Visual feedback yang **jelas** (tong marah + icon sampah)
- 📖 Penjelasan **edukatif** dengan bahasa anak-anak
- 😊 **Gamifikasi** (tong sebagai karakter, bukan sekadar UI)
- 🔁 **Immediate feedback** (langsung tahu salah setelah level selesai)

---

## 🚀 **NEXT LEVEL - IDE PENGEMBANGAN**

### **Tambahan yang Bisa Dikembangkan:**

1. **Animasi Tong:**
   - Tong "goyang-goyang" saat marah (DOTween animation)
   - Icon sampah "jatuh" dari atas dengan bouncing

2. **Sound Effect:**
   - Tong bicara dengan **voice acting** (rekam suara anak)
   - SFX "boing" saat icon sampah muncul
   - Background music sedih/lucu saat tong protes

3. **Multiple Expressions:**
   - Tong sedikit marah (1-2 sampah salah) 😐
   - Tong sangat marah (5+ sampah salah) 😡
   - Tong menangis (banyak sekali salah) 😭

4. **Reward System:**
   - Jika **TIDAK ADA** kesalahan:
     - Tong **SENANG** (gambar tong tersenyum)
     - Dialog: "Terima kasih! Kamu hebat sekali!" 🎉
     - Bonus poin +50

5. **Save Mistakes:**
   - Catat kesalahan di PlayerPrefs
   - Di menu utama, ada **"Buku Kesalahan"** yang bisa dibaca lagi
   - Anak bisa review materi yang masih salah

---

## 📞 **BUTUH BANTUAN?**

**Jika masih bingung atau ada error:**
1. **Cek TROUBLESHOOTING** (Problem 1-7 di atas)
2. **Cek Console log** (panel bawah Unity) untuk error merah
3. **Screenshot error** dan tanyakan ke guru/orang tua
4. **Ulangi langkah** yang masih kurang jelas

**Tips:**
- Jangan terburu-buru, ikuti langkah **satu per satu**
- **Save scene** setelah setiap langkah besar (Ctrl + S)
- **Test berkala** (jangan tunggu sampai akhir)

---

**Tanggal Tutorial:** 7 Desember 2025  
**Untuk:** Anak SD (7-12 tahun) & Pemula Unity  
**Bahasa:** Indonesia (Ramah Anak)  
**Status:** ✅ **SIAP DIGUNAKAN!**

**Semangat belajar! Kamu pasti bisa! 💪✨**

---

*Catatan untuk Guru/Orang Tua:*  
Tutorial ini dirancang dengan bahasa yang **sangat sederhana** dan **step-by-step** yang detail untuk anak yang **TIDAK PUNYA PENGALAMAN** Unity sama sekali. Estimasi waktu setup: **30-45 menit** dengan pendampingan.
