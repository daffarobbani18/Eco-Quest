# 🎬 TUTORIAL SETUP ANIMASI TRANSISI JUDGMENT PHASE

**Target:** Anak SD & Pemula Unity (bahasa sederhana dengan emoji)  
**Waktu Setup:** 15-20 menit  
**Hasil:** Animasi smooth "Ayo Kita Lihat Kesalahanmu!" sebelum panel tong marah muncul

---

## 📋 APA YANG AKAN KITA BUAT?

Sebelum panel tong marah muncul, akan ada:
1. **Layar hitam fade in** (gelap pelan-pelan)
2. **Teks muncul** (membesar + fade in): "Ayo Kita Lihat Kesalahanmu! 👀"
3. **Tahan 2 detik** (biar anak sempat baca)
4. **Fade out** (hilang pelan-pelan)
5. **Mulai slideshow** tong marah

**Contoh Flow:**
```
Sampah selesai diproses → Fade in hitam (0.5s) → 
Teks muncul "Ayo Kita Lihat Kesalahanmu!" (0.5s) → 
Tahan 2s → Fade out (0.5s) → 
Panel tong marah muncul dengan icon sampah animasi
```

---

## 🛠️ LANGKAH 1: BUAT UI TRANSISI DI HIERARCHY

### 1.1. Buka Scene Game
1. Buka Unity
2. Double-klik scene `03_Game_Processing.unity` (di folder `Assets/_Scenes/`)
3. Tunggu sampai scene terbuka di editor

### 1.2. Cari Panel Judgment yang Sudah Ada
1. Di panel **Hierarchy** (kiri), cari GameObject bernama `PanelJudgment` atau `Panel_TongMarah`
2. Klik GameObject tersebut untuk select

### 1.3. Buat Panel Overlay Transisi (Layar Hitam)
1. **Klik kanan** `PanelJudgment` → pilih **UI → Panel**
2. Rename panel baru jadi: `Panel_TransisiOverlay`
3. Di **Inspector** (kanan), setting:
   
   **RectTransform:**
   - Klik anchor preset (kotak kecil kiri atas)
   - Tahan **Alt + Shift** → Klik kotak **stretch full** (kanan bawah)
   - Sekarang panel akan full screen (ikuti ukuran parent)
   
   **Image Component:**
   - **Color**: Hitam penuh (R:0, G:0, B:0, A:255)
   - Atau bisa pakai warna lain sesuai tema (misal: biru gelap, ungu)
   
   **CanvasGroup (Tambah Component Baru):**
   - Klik **Add Component** di bawah Inspector
   - Cari dan pilih **CanvasGroup**
   - Ini akan dipakai untuk fade in/out

4. **PENTING:** Matikan panel ini dulu:
   - Centang ✅ di sebelah nama `Panel_TransisiOverlay` (di Inspector bagian atas)
   - Jadi checkbox-nya **TIDAK TERCENTANG** (disable)
   - Panel akan otomatis nyala saat transisi jalan

### 1.4. Buat Text Transisi (Teks Informatif)
1. **Klik kanan** `Panel_TransisiOverlay` → pilih **UI → Text - TextMeshPro**
2. Kalau muncul popup "Import TMP Essentials", klik **Import**
3. Rename text jadi: `Text_TransisiInfo`
4. Di **Inspector**, setting:

   **RectTransform:**
   - **Anchors**: Center-Middle (preset tengah-tengah)
   - **Width**: 800
   - **Height**: 200
   - **Pos X**: 0
   - **Pos Y**: 0

   **TextMeshPro Component:**
   - **Text**: (Kosongkan dulu, nanti diisi otomatis oleh script)
   - **Font Style**: Bold
   - **Font Size**: 60 (besar supaya terlihat jelas)
   - **Alignment**: Center (Horizontal + Vertical)
   - **Color**: Putih (atau kuning cerah untuk lebih ceria)
   - **Enable Auto Size**: ✅ (biar auto menyesuaikan)
   - **Min Font Size**: 40
   - **Max Font Size**: 80

   **Outline (Opsional - Biar Teks Lebih Jelas):**
   - Scroll ke bawah di TextMeshPro component
   - Cari **Outline** → centang ✅
   - **Color**: Hitam atau abu gelap
   - **Size**: 0.3

---

## 🔗 LANGKAH 2: LINK UI KE SCRIPT

### 2.1. Cari Script JudgmentSlideshow
1. Di **Hierarchy**, cari GameObject yang punya script `JudgmentSlideshow`
   - Biasanya ada di `PanelJudgment` atau child-nya
   - Kalau bingung: Cari semua object → Lihat Inspector → Cek ada component `JudgmentSlideshow` nggak
2. Klik GameObject tersebut

### 2.2. Drag & Drop UI ke Inspector
Di **Inspector**, scroll ke bagian **"UI References - Transisi Intro"**:

**Panel Transisi Overlay:**
- Drag `Panel_TransisiOverlay` (dari Hierarchy) → field **Panel Transisi Overlay**
- Pastikan yang di-drag adalah **Panel dengan CanvasGroup**

**Text Transisi Info:**
- Drag `Text_TransisiInfo` (dari Hierarchy) → field **Text Transisi Info**

### 2.3. Atur Settings Animasi (Opsional)
Scroll ke **"Settings - Animasi Transisi"**:

**Durasi (dalam detik):**
- **Transisi Fade In Duration**: `0.5` (seberapa cepat layar gelap)
- **Transisi Text Duration**: `2.0` (seberapa lama teks ditahan supaya anak sempat baca)
- **Transisi Fade Out Duration**: `0.5` (seberapa cepat layar terang lagi)

**Warna Overlay:**
- **Transisi Overlay Color**: Default hitam (R:0, G:0, B:0, A:0.9)
- Kalau mau ubah: Klik kotak warna → Pilih warna favorit
- **A (Alpha)**: 0.9 = 90% gelap (biar tidak terlalu hitam pekat)

**Total Waktu Transisi:** 0.5s (fade in) + 2.0s (tahan) + 0.5s (fade out) = **3 detik**

---

## ✅ LANGKAH 3: CHECKLIST FINAL

Sebelum test, pastikan:

### ✅ Hierarchy Structure
```
Canvas
└── PanelJudgment (atau Panel_TongMarah)
    ├── Panel_TransisiOverlay ← NEW! (disable di awal)
    │   ├── CanvasGroup component ada
    │   └── Text_TransisiInfo ← NEW!
    ├── ImageTongMarah
    ├── TextDialog
    ├── ContainerWasteIcons
    └── TextProgress
```

### ✅ Inspector JudgmentSlideshow
**UI References - Transisi Intro:**
- ✅ Panel Transisi Overlay → `Panel_TransisiOverlay` (ada CanvasGroup)
- ✅ Text Transisi Info → `Text_TransisiInfo`

**Settings - Animasi Transisi:**
- ✅ Fade In: 0.5s
- ✅ Text Duration: 2.0s
- ✅ Fade Out: 0.5s
- ✅ Overlay Color: Hitam 90%

**UI References - Slide Components** (yang lama, harus tetap ada):
- ✅ Image Tong Marah
- ✅ Text Dialog
- ✅ Container Waste Icons
- ✅ Prefab Waste Icon
- ✅ Text Progress

**Full Illustration Sprites:**
- ✅ Sprite Tong Organik Marah
- ✅ Sprite Tong Anorganik Marah
- ✅ Sprite Tong B3 Marah

---

## 🎮 LANGKAH 4: TESTING

### 4.1. Test di Play Mode
1. **Save scene** (Ctrl + S atau File → Save)
2. Klik tombol **Play ▶️** di atas scene view
3. Main game → **Sengaja salah pilah beberapa sampah**
4. Tunggu sampai semua sampah selesai diproses

### 4.2. Yang Harus Terjadi
**Jika Ada Kesalahan:**
1. ✅ Layar fade in hitam (0.5 detik)
2. ✅ Teks muncul: "Ayo Kita Lihat Kesalahanmu! 👀" (atau teks random lain)
3. ✅ Teks membesar dari kecil (animasi scale)
4. ✅ Teks fade in dari transparan
5. ✅ Tahan 2 detik (bisa baca)
6. ✅ Layar fade out
7. ✅ Panel tong marah muncul dengan icon sampah animasi
8. ✅ Slideshow berjalan normal (tong marah 1, 2, dst)
9. ✅ Setelah selesai, Win Panel muncul

**Jika Tidak Ada Kesalahan:**
- ❌ Transisi **TIDAK MUNCUL** (langsung Win Panel)
- ✅ Ini normal! Transisi hanya untuk kesalahan

### 4.3. Troubleshooting

**❌ MASALAH 1: Panel TransisiOverlay muncul permanen (tidak hilang)**
- **Penyebab:** Panel tidak di-disable di awal
- **Solusi:** 
  1. Stop Play Mode
  2. Select `Panel_TransisiOverlay` di Hierarchy
  3. Di Inspector, **un-check** checkbox di samping nama panel (disable)
  4. Play lagi

**❌ MASALAH 2: Teks tidak muncul (layar hitam kosong)**
- **Penyebab:** Text_TransisiInfo tidak ter-link atau warna teks sama dengan background
- **Solusi:**
  1. Cek Inspector JudgmentSlideshow → `Text Transisi Info` terisi?
  2. Cek warna teks (harus putih/cerah, bukan hitam)
  3. Cek **Alpha** teks awal = 0 di script (normal, biar bisa fade in)

**❌ MASALAH 3: Transisi terlalu cepat/lambat**
- **Penyebab:** Durasi tidak pas untuk anak SD
- **Solusi:** Adjust di Inspector:
  - Kalau terlalu cepat: Naikkan `Transisi Text Duration` jadi 3 detik
  - Kalau terlalu lambat: Turunkan jadi 1.5 detik

**❌ MASALAH 4: Error "NullReferenceException: Panel Transisi Overlay is null"**
- **Penyebab:** Panel belum di-assign di Inspector
- **Solusi:** Drag `Panel_TransisiOverlay` ke field di Inspector

**❌ MASALAH 5: Teks terpotong/tidak full**
- **Penyebab:** RectTransform Width terlalu kecil
- **Solusi:**
  1. Select `Text_TransisiInfo`
  2. Di RectTransform → Width: 900 (lebih lebar)
  3. Enable Auto Size di TextMeshPro

**❌ MASALAH 6: Animasi patah-patah (laggy)**
- **Penyebab:** Frame rate rendah atau Time.timeScale tidak 1
- **Solusi:**
  - Script sudah pakai `Time.unscaledDeltaTime`, jadi harusnya smooth
  - Cek apakah ada proses berat lain yang jalan

---

## 🎨 LANGKAH 5: CUSTOMIZE (OPSIONAL)

### 5.1. Ganti Teks Transisi
Di script `JudgmentSlideshow.cs`, cari fungsi `TransitionIntroCoroutine()`:

```csharp
// Baris ~175-183
string[] pesanTransisi = new string[]
{
    "Ayo Kita Lihat Kesalahanmu! 👀",
    "Ada Yang Salah Nih... 🤔",
    "Yuk Belajar Dari Kesalahan! 📚",
    "Tong Sampah Mau Ngomong Nih! 🗣️",
    "Wah, Ada Yang Kurang Tepat! 😅"
};
```

**Tambah Teks Sendiri:**
```csharp
string[] pesanTransisi = new string[]
{
    "Ayo Kita Lihat Kesalahanmu! 👀",
    "Ups, Ada yang Salah Pilah! 😬",
    "Tong Sampah Punya Cerita! 📖",
    "Saatnya Belajar Yuk! 🎓",
    "Kesalahan Adalah Guru Terbaik! 🌟"
};
```

### 5.2. Ubah Warna Overlay (Tidak Harus Hitam)
Di Inspector → **Transisi Overlay Color**:
- **Biru Gelap**: R:0, G:50, B:100, A:230
- **Ungu Gelap**: R:50, G:0, B:100, A:230
- **Hijau Gelap**: R:0, G:100, B:50, A:230 (tema lingkungan!)

### 5.3. Tambah Sound Effect (Opsional - Advance)
Kalau mau ada suara:
1. Import audio file (MP3/WAV) ke folder `Assets/Audio/`
2. Tambah field `AudioSource` di script
3. Play sound saat transisi mulai:
```csharp
if (audioSource != null)
{
    audioSource.Play();
}
```

### 5.4. Animasi Teks Lebih Fancy
Kalau mau teks "bounce" atau "shake":
- Ganti `easedT` di fungsi `TransitionIntroCoroutine()`
- Contoh bounce: `float easedT = Mathf.Sin(t * Mathf.PI * 0.5f);`

---

## 🧪 TESTING CHECKLIST (LENGKAP)

### Test Case 1: Semua Benar (Tidak Ada Kesalahan)
1. ✅ Main game
2. ✅ Sortir **SEMUA SAMPAH BENAR**
3. ✅ **Transisi TIDAK MUNCUL** (langsung Win Panel)
4. ✅ Win Panel tampil normal

### Test Case 2: Ada 1 Kesalahan
1. ✅ Main game
2. ✅ Salah pilah 1 sampah (misal: Apel → Tong Anorganik)
3. ✅ Transisi muncul: Fade in → Teks → Fade out (3 detik total)
4. ✅ Panel tong marah muncul (1 tong saja yang protes)
5. ✅ Icon Apel muncul dengan animasi (drop + fade + scale)
6. ✅ Icon melayang tipis-tipis
7. ✅ Tahan 5 detik
8. ✅ Win Panel muncul

### Test Case 3: Ada 3 Kesalahan (Semua Tong Marah)
1. ✅ Main game
2. ✅ Salah pilah 3 sampah berbeda:
   - Apel (Organik) → Tong B3
   - Botol Plastik (Anorganik) → Tong Organik
   - Baterai (B3) → Tong Anorganik
3. ✅ Transisi muncul
4. ✅ Slideshow 3 tong:
   - Tong 1: Tong B3 marah (Apel salah masuk)
   - Tong 2: Tong Organik marah (Botol salah masuk)
   - Tong 3: Tong Anorganik marah (Baterai salah masuk)
5. ✅ Setiap slide 5 detik
6. ✅ Total waktu: 3s (transisi) + 15s (3 slide x 5s) = 18 detik
7. ✅ Win Panel muncul

### Test Case 4: Skor Habis (Game Over)
1. ✅ Main game
2. ✅ Salah pilah banyak sampai skor < 0
3. ✅ **Game Over Panel muncul LANGSUNG** (tidak ada transisi/judgment)
4. ✅ Ini benar! Transisi hanya untuk "level selesai tapi ada kesalahan"

---

## 📊 DIAGRAM FLOW LENGKAP

```
┌─────────────────────────────────────┐
│  Semua Sampah Selesai Diproses      │
└─────────────┬───────────────────────┘
              │
              ▼
     ┌────────┴────────┐
     │  Ada Kesalahan?  │
     └────────┬────────┘
              │
        ┌─────┴─────┐
        │           │
       YES          NO
        │           │
        ▼           ▼
┌───────────────┐  ┌──────────────┐
│ TRANSISI INTRO│  │ WIN PANEL    │
│ (3 detik)     │  │ (langsung)   │
└───────┬───────┘  └──────────────┘
        │
        ▼
┌───────────────────────────┐
│ SLIDESHOW TONG MARAH      │
│ - Tong 1: 5 detik         │
│ - Tong 2: 5 detik (jika ada) │
│ - Tong 3: 5 detik (jika ada) │
└───────────┬───────────────┘
            │
            ▼
    ┌───────────────┐
    │ WIN PANEL     │
    └───────────────┘
```

---

## 🎓 TIPS UNTUK GURU/ORANG TUA

### Pedagogical Reasoning (Alasan Edukatif)
**Kenapa Pakai Transisi?**
1. **Mental Preparation** 🧠
   - Anak butuh "sinyal" bahwa mereka akan melihat feedback
   - Transisi memberikan waktu bersiap mental
   - Tidak kaget langsung lihat tong marah

2. **Attention Reset** 👀
   - Layar hitam = reset perhatian
   - Seperti "adegan baru" di film
   - Anak fokus ke apa yang akan muncul

3. **Positive Framing** ✨
   - Teks "Ayo Kita Lihat Kesalahanmu" = nada positif (bukan menyalahkan)
   - "Yuk Belajar" = growth mindset
   - Kesalahan = kesempatan belajar

4. **Pacing Control** ⏱️
   - Transisi 3 detik = anak punya waktu "napas"
   - Tidak langsung bombardir informasi
   - Membantu anak dengan processing speed lambat

### Adjust untuk Usia Berbeda
**Usia 7-8 tahun (Kelas 1-2 SD):**
- Naikkan `Transisi Text Duration` → 3 detik (baca lebih lambat)
- Pilih teks paling simple: "Ayo Lihat Yuk! 👀"
- Font Size lebih besar: 70-80

**Usia 9-10 tahun (Kelas 3-4 SD):**
- Default settings pas (2 detik)
- Teks bisa lebih bervariasi

**Usia 11-12 tahun (Kelas 5-6 SD):**
- Bisa lebih cepat: 1.5 detik
- Teks bisa lebih challenging: "Mari Kita Evaluasi Pilihanmu! 📊"

---

## 🎉 SELESAI!

Sekarang game kamu punya:
1. ✅ Animasi transisi smooth sebelum feedback
2. ✅ Teks informatif yang ramah anak
3. ✅ Flow yang tidak kaget (mental preparation)
4. ✅ Professional feel (tidak kaku)

**Total Fitur Animasi di Judgment Phase:**
- Transisi intro (fade + teks) ← BARU!
- Panel tong marah muncul
- Icon sampah spawn (drop + fade + scale)
- Icon melayang tipis-tipis (floating idle)
- Slideshow per-tong dengan dialog dinamis
- Win Panel di akhir

**Kalau ada masalah:**
1. Baca troubleshooting di atas
2. Cek Console Log (Window → General → Console) untuk error
3. Screenshot error → tanya dengan detail masalahnya

**Selamat! Game edukasi kamu makin profesional! 🎮✨**

---

## 📚 REFERENSI TAMBAHAN

**File yang Dimodifikasi:**
- `JudgmentSlideshow.cs` - Tambah transisi intro system

**UI yang Ditambah:**
- `Panel_TransisiOverlay` (dengan CanvasGroup)
- `Text_TransisiInfo` (TextMeshPro)

**Total Waktu User Experience:**
```
Kesalahan 1 tong:
Transisi (3s) + Slide (5s) + Win Panel = 8 detik

Kesalahan 3 tong:
Transisi (3s) + Slide 1 (5s) + Slide 2 (5s) + Slide 3 (5s) + Win Panel = 18 detik
```

**Next Level:** Kalau mau tambah animasi lain (panel masuk, button bounce, dll), konsepnya sama! 🚀
