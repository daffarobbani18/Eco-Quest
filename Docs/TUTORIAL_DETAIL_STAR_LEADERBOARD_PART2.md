# 🎓 TUTORIAL LENGKAP: STAR RATING & LEADERBOARD SYSTEM
## 📚 PART 2: Leaderboard Panel, Level Selection, Testing, Troubleshooting

---

# 📖 LANJUTAN DARI PART 1

Ini adalah **Part 2** dari tutorial. Pastikan kamu sudah selesai **Part 1** (Bagian 1-3):
- ✅ Google Sheets setup & API deployed
- ✅ LeaderboardManager di Unity scene Main Menu
- ✅ Win Panel dengan star icons & upload button

Sekarang kita lanjut ke:
- **Bagian 4**: Setup Leaderboard Panel (ScrollView untuk display rankings)
- **Bagian 5**: Setup Level Selection (display stars per level)
- **Bagian 6**: Testing complete flow
- **Bagian 7**: Troubleshooting problems

---
---
---

# BAGIAN 4: UNITY SETUP - LEADERBOARD PANEL
## ⏱️ Estimasi: 45 menit

Sekarang kita akan buat panel leaderboard untuk display ranking pemain.

---

## Step 4.1: Create Leaderboard Panel

### 4.1.1 - Open Main Menu Scene
1. Jika belum buka, double-click scene **`00_MainMenu.unity`**
2. Scene Main Menu terbuka di Scene view

### 4.1.2 - Create Canvas Panel
1. Di **Hierarchy**, cari **Canvas** GameObject
2. **Klik kanan** Canvas → **UI** → **Panel**
3. GameObject "Panel" muncul sebagai child Canvas
4. **Rename** jadi: **`LeaderboardPanel`**

**Hierarchy:**
```
Canvas
├── MainMenuButtons
├── TitleText
├── LeaderboardPanel  ← Panel baru
└── ...
```

### 4.1.3 - Set Panel Full Screen
1. Select **LeaderboardPanel**
2. Di **Rect Transform** component:
   - Klik **Anchor Presets** (icon kotak di kiri atas)
   - Tekan **Alt+Shift** sambil klik preset **"Stretch Stretch"** (kanan bawah)
   - Panel sekarang full screen

**Alternative manual:**
```
Rect Transform
  Anchors:
    Min: X 0, Y 0
    Max: X 1, Y 1
  Left: 0, Right: 0, Top: 0, Bottom: 0
```

### 4.1.4 - Change Panel Background
1. Di component **Image**:
   ```
   Color: R 0, G 0, B 0, A 200  (Hitam semi-transparent)
   ```
2. Ini buat overlay backdrop

### 4.1.5 - Set Panel Inactive by Default
1. Di **Inspector** paling atas, **uncheck** checkbox ☐
2. Panel jadi abu-abu di Hierarchy (inactive)
3. Panel akan di-activate via code saat button diklik

**✅ Checklist Step 4.1:**
- [ ] LeaderboardPanel dibuat sebagai child Canvas
- [ ] Panel full screen (stretch stretch anchor)
- [ ] Background hitam semi-transparent
- [ ] Panel set inactive ☐

---

## Step 4.2: Create Header

Sekarang kita buat header "LEADERBOARD" di atas panel.

### 4.2.1 - Create Container Panel
1. **Klik kanan** LeaderboardPanel → **UI** → **Panel**
2. Rename jadi: **`Container`**

### 4.2.2 - Set Container Size & Position
1. Di **Rect Transform**:
   ```
   Anchor Presets: Top Center (top row, middle)
   Pivot: X 0.5, Y 1
   
   Pos X: 0
   Pos Y: 0
   Width: 800
   Height: 700
   ```

### 4.2.3 - Change Container Color
1. Di **Image** component:
   ```
   Color: R 255, G 255, B 255, A 255  (Putih solid)
   ```

### 4.2.4 - Create Header Text
1. **Klik kanan** Container → **UI** → **Text - TextMeshPro**
2. Rename jadi: **`HeaderText`**

### 4.2.5 - Set Header Text Properties
1. Di **Rect Transform**:
   ```
   Anchor Presets: Top Stretch (top row, right side)
   
   Left: 0, Right: 0, Top: 0
   Height: 80
   ```

2. Di **TextMeshPro - Text**:
   ```
   Text: 🏆 LEADERBOARD 🏆
   Font Size: 48
   Alignment: Center + Middle
   Color: R 255, G 193, B 7, A 255  (Gold)
   Font Style: Bold
   ```

**Scene View:**
```
╔════════════════════════════════════╗
║   Container (White Box)            ║
║  ┌──────────────────────────────┐  ║
║  │   🏆 LEADERBOARD 🏆          │  ║ ← Header
║  └──────────────────────────────┘  ║
║                                    ║
║  (Space untuk ScrollView)          ║
║                                    ║
╚════════════════════════════════════╝
```

**✅ Checklist Step 4.2:**
- [ ] Container panel dibuat (800x700, white)
- [ ] HeaderText dibuat di atas container
- [ ] Text: "🏆 LEADERBOARD 🏆" (gold, size 48, bold)

---

## Step 4.3: Create ScrollView

### 4.3.1 - Create ScrollView GameObject
1. **Klik kanan** Container → **UI** → **Scroll View**
2. GameObject "Scroll View" muncul, **rename** jadi: **`ScrollViewLeaderboard`**

**Hierarchy sekarang:**
```
Container
├── HeaderText
└── ScrollViewLeaderboard
    ├── Viewport
    │   └── Content
    └── Scrollbar Vertical
```

### 4.3.2 - Position ScrollView
1. Select **ScrollViewLeaderboard**
2. Di **Rect Transform**:
   ```
   Anchor Presets: Stretch Stretch (Alt+Shift+klik)
   
   Left: 20
   Right: 20
   Top: 100  (beri space untuk header)
   Bottom: 80  (beri space untuk button)
   ```

### 4.3.3 - Configure Scroll Rect Component
1. Masih di ScrollViewLeaderboard
2. Di component **Scroll Rect**:
   ```
   ☐ Horizontal  (Uncheck - tidak perlu scroll horizontal)
   ☑ Vertical    (Check - scroll vertical)
   
   Movement Type: Elastic
   Elasticity: 0.1
   Inertia: ☑
   Scroll Sensitivity: 30
   ```

### 4.3.4 - Configure Content
1. Expand **ScrollViewLeaderboard** → **Viewport**
2. Select **Content** (child of Viewport)
3. Di **Rect Transform**:
   ```
   Anchor Presets: Top Stretch
   Pivot: X 0.5, Y 1
   
   Left: 0, Right: 0, Top: 0
   Height: 1000  (Akan auto-expand nanti)
   ```

4. **Add Component** → Search **Vertical Layout Group**
5. Di **Vertical Layout Group** component:
   ```
   ☑ Child Controls Size - Height
   ☐ Child Controls Size - Width  (Uncheck)
   ☑ Child Force Expand - Height
   ☑ Child Force Expand - Width
   
   Padding: Left 10, Right 10, Top 10, Bottom 10
   Spacing: 5
   Child Alignment: Upper Center
   ```

6. **Add Component** → Search **Content Size Fitter**
7. Di **Content Size Fitter**:
   ```
   Horizontal Fit: Unconstrained
   Vertical Fit: Preferred Size
   ```

### 4.3.5 - Configure Viewport Mask
1. Select **Viewport** (parent of Content)
2. Di component **Mask**:
   ```
   ☑ Show Mask Graphic  (Uncheck jika tidak mau background)
   ```

### 4.3.6 - Remove Scrollbar (Optional)
Jika tidak mau scrollbar visual:
1. Select **Scrollbar Vertical** (child ScrollViewLeaderboard)
2. **Delete** (tekan Delete key)
3. Select **ScrollViewLeaderboard**
4. Di **Scroll Rect**, field **"Verticalbar"**: Drag **None**

**Atau biarkan scrollbar** jika mau visual feedback.

**Scene View:**
```
╔════════════════════════════════════╗
║   🏆 LEADERBOARD 🏆                ║
║  ┌──────────────────────────────┐  ║
║  │ ┌──────────────────────────┐ │  ║
║  │ │ [Player Rows akan muncul] │ │  ║ ← ScrollView area
║  │ │                           │ │  ║
║  │ │                           │ │  ║
║  │ └──────────────────────────┘ │  ║
║  └──────────────────────────────┘  ║
╚════════════════════════════════════╝
```

**✅ Checklist Step 4.3:**
- [ ] ScrollViewLeaderboard dibuat dengan Viewport & Content
- [ ] Scroll Rect: Horizontal OFF, Vertical ON
- [ ] Content memiliki Vertical Layout Group
- [ ] Content memiliki Content Size Fitter (Vertical: Preferred Size)
- [ ] Scrollbar dihapus atau dibiarkan (your choice)

---

## Step 4.4: Create Player Row Prefab

Sekarang kita buat prefab untuk satu row pemain di leaderboard.

### 4.4.1 - Create Row GameObject (Temporary)
1. **Klik kanan** Content → **UI** → **Panel**
2. Rename jadi: **`PlayerRow`**

### 4.4.2 - Set Row Size
1. Di **Rect Transform**:
   ```
   Anchor Presets: Top Stretch
   
   Left: 0, Right: 0, Top: 0
   Height: 80
   ```

### 4.4.3 - Change Row Background Color
1. Di **Image** component:
   ```
   Color: R 255, G 255, B 255, A 25  (Putih semi-transparent)
   ```

### 4.4.4 - Add Horizontal Layout Group
1. PlayerRow terpilih, **Add Component** → **Horizontal Layout Group**
2. Configure:
   ```
   ☑ Child Controls Size - Width
   ☑ Child Controls Size - Height
   ☐ Child Force Expand - Width
   ☑ Child Force Expand - Height
   
   Padding: Left 20, Right 20, Top 10, Bottom 10
   Spacing: 10
   Child Alignment: Middle Left
   ```

### 4.4.5 - Create Rank Text
1. **Klik kanan** PlayerRow → **UI** → **Text - TextMeshPro**
2. Rename jadi: **`RankText`**
3. Di **Rect Transform** → **Layout Element** (Add Component):
   ```
   Preferred Width: 80
   ```
4. Di **TextMeshPro - Text**:
   ```
   Text: 1
   Font Size: 36
   Alignment: Center + Middle
   Color: White
   Font Style: Bold
   ```

### 4.4.6 - Create Name Text
1. **Klik kanan** PlayerRow → **UI** → **Text - TextMeshPro**
2. Rename: **`NameText`**
3. **Layout Element**:
   ```
   Flexible Width: 1  (Auto-expand)
   ```
4. **TextMeshPro - Text**:
   ```
   Text: Player Name
   Font Size: 28
   Alignment: Left + Middle
   Color: White
   ```

### 4.4.7 - Create Score Text
1. **Klik kanan** PlayerRow → **UI** → **Text - TextMeshPro**
2. Rename: **`ScoreText`**
3. **Layout Element**:
   ```
   Preferred Width: 100
   ```
4. **TextMeshPro - Text**:
   ```
   Text: 100
   Font Size: 32
   Alignment: Center + Middle
   Color: Gold (R 255, G 223, B 0)
   Font Style: Bold
   ```

### 4.4.8 - Create Stars Text
1. **Klik kanan** PlayerRow → **UI** → **Text - TextMeshPro**
2. Rename: **`StarsText`**
3. **Layout Element**:
   ```
   Preferred Width: 120
   ```
4. **TextMeshPro - Text**:
   ```
   Text: ⭐⭐⭐
   Font Size: 24
   Alignment: Center + Middle
   Color: White
   ```

**Hierarchy PlayerRow:**
```
PlayerRow
├── RankText     (Width 80)
├── NameText     (Flexible - expand)
├── ScoreText    (Width 100)
└── StarsText    (Width 120)
```

**Preview Row:**
```
┌────────────────────────────────────────────┐
│  1   │  Player Name        │  100  │ ⭐⭐⭐  │
└────────────────────────────────────────────┘
```

### 4.4.9 - Convert to Prefab
1. Dari **Hierarchy**, **drag** PlayerRow ke **Project window**
2. Target folder: `Assets/_Prefabs/` (atau buat folder "UI" di dalamnya)
3. Prefab PlayerRow dibuat (icon biru di Project)
4. **DELETE** PlayerRow dari Hierarchy (klik kanan → Delete)

**⚠️ PENTING**: Hapus dari Hierarchy karena ini cuma template. Nanti akan di-instantiate via code.

**✅ Checklist Step 4.4:**
- [ ] PlayerRow GameObject dibuat dengan 4 child texts
- [ ] Horizontal Layout Group dikonfigurasi
- [ ] RankText (80px), NameText (flex), ScoreText (100px), StarsText (120px)
- [ ] PlayerRow disave sebagai Prefab di Assets/_Prefabs/
- [ ] PlayerRow DIHAPUS dari Hierarchy (penting!)

---

## Step 4.5: Create Buttons

### 4.5.1 - Create Refresh Button
1. **Klik kanan** Container → **UI** → **Button - TextMeshPro**
2. Rename: **`ButtonRefresh`**

3. Di **Rect Transform**:
   ```
   Anchor Presets: Bottom Center
   Pivot: X 0.5, Y 0
   
   Pos X: -100
   Pos Y: 20
   Width: 180
   Height: 50
   ```

4. Di **Image** (Button background):
   ```
   Color: Blue (R 33, G 150, B 243, A 255)
   ```

5. Expand **ButtonRefresh**, select **Text (TMP)**
6. **TextMeshPro - Text**:
   ```
   Text: 🔄 Refresh
   Font Size: 24
   Alignment: Center + Middle
   Color: White
   Font Style: Bold
   ```

### 4.5.2 - Create Close Button
1. **Klik kanan** Container → **UI** → **Button - TextMeshPro**
2. Rename: **`ButtonClose`**

3. Di **Rect Transform**:
   ```
   Anchor Presets: Bottom Center
   Pivot: X 0.5, Y 0
   
   Pos X: 100
   Pos Y: 20
   Width: 180
   Height: 50
   ```

4. **Image**:
   ```
   Color: Red (R 244, G 67, B 54, A 255)
   ```

5. **Text**:
   ```
   Text: ✖ Close
   Font Size: 24
   Alignment: Center + Middle
   Color: White
   Font Style: Bold
   ```

**Scene View:**
```
╔════════════════════════════════════╗
║   🏆 LEADERBOARD 🏆                ║
║  ┌──────────────────────────────┐  ║
║  │ [ScrollView area]            │  ║
║  │                              │  ║
║  └──────────────────────────────┘  ║
║                                    ║
║   [🔄 Refresh]     [✖ Close]       ║ ← Buttons
╚════════════════════════════════════╝
```

**✅ Checklist Step 4.5:**
- [ ] ButtonRefresh dibuat (biru, text "🔄 Refresh")
- [ ] ButtonClose dibuat (merah, text "✖ Close")
- [ ] Kedua button positioned di bottom center container

---

## Step 4.6: Add LeaderboardUI Component

### 4.6.1 - Select LeaderboardPanel
1. Di **Hierarchy**, select **LeaderboardPanel** (parent root)

### 4.6.2 - Add Component
1. Di **Inspector**, klik **"Add Component"**
2. Search: `LeaderboardUI`
3. Klik **"Leaderboard UI (Script)"**

**Inspector sekarang:**
```
Leaderboard UI (Script)
  Script: LeaderboardUI
  
  UI References
    Content Parent: None
    Player Row Prefab: None
    Refresh Button: None
    Close Button: None
    Refresh Button Text: None
    
  Level Filter
    Level Filter: 0
    Level Dropdown: None
    
  Highlight Settings
    Highlight Color: (Gold)
    Normal Color: (White transparent)
    
  Loading State
    Loading Indicator: None
    Empty State Panel: None
    
  Audio
    Sfx Source: None
    Refresh Sound: None
```

**✅ Checklist Step 4.6:**
- [ ] LeaderboardUI component ditambahkan ke LeaderboardPanel
- [ ] Inspector menampilkan fields (masih None/empty)

---

## Step 4.7: Configure Inspector

### 4.7.1 - Link Content Parent
1. Field **"Content Parent"**
2. Dari Hierarchy: **Expand** ScrollViewLeaderboard → Viewport
3. **Drag** GameObject **Content** ke field

**Result:**
```
Content Parent: Content (Transform) ✓
```

### 4.7.2 - Link Player Row Prefab
1. Field **"Player Row Prefab"**
2. Dari **Project window**, navigate ke `Assets/_Prefabs/`
3. **Drag** prefab **PlayerRow** ke field

**Result:**
```
Player Row Prefab: PlayerRow (GameObject) ✓
```

### 4.7.3 - Link Refresh Button & Text
1. Field **"Refresh Button"**
2. Dari Hierarchy, **drag** ButtonRefresh

3. Field **"Refresh Button Text"**
4. Expand ButtonRefresh, **drag** child **Text (TMP)**

**Result:**
```
Refresh Button: ButtonRefresh (Button) ✓
Refresh Button Text: Text (TMP) (TextMeshPro) ✓
```

### 4.7.4 - Link Close Button
1. Field **"Close Button"**
2. Drag **ButtonClose** dari Hierarchy

**Result:**
```
Close Button: ButtonClose (Button) ✓
```

### 4.7.5 - Configure Level Filter (Optional)
```
Level Filter
  Level Filter: 0  ← 0 = All levels, 1-10 = specific level
  Level Dropdown: None  ← Optional, biarkan None
```

Biarkan default **0** (all levels). Nanti bisa adjust via code atau dropdown.

### 4.7.6 - Configure Highlight Colors
Default colors sudah OK:
```
Highlight Settings
  Highlight Color: R 1, G 0.92, B 0.016, A 0.3  (Gold semi-transparent)
  Normal Color: R 1, G 1, B 1, A 0.1  (White semi-transparent)
```

Bisa adjust jika mau warna berbeda.

### 4.7.7 - Create Loading Indicator (Optional)

#### Simple Text Loading
1. **Klik kanan** Container → **UI** → **Text - TextMeshPro**
2. Rename: **`LoadingText`**
3. **Rect Transform**: Center screen
   ```
   Anchor Presets: Middle Center
   Pos X: 0, Pos Y: 0
   Width: 300, Height: 100
   ```
4. **TextMeshPro - Text**:
   ```
   Text: Loading...
   Font Size: 36
   Alignment: Center + Middle
   Color: White
   ```
5. Set **Inactive** ☐ (default hidden)

#### Link to Inspector
1. Field **"Loading Indicator"**
2. Drag **LoadingText** dari Hierarchy

**Alternative**: Buat spinner/animation (advanced, skip untuk sekarang).

### 4.7.8 - Create Empty State Panel (Optional)

1. **Klik kanan** Container → **UI** → **Panel**
2. Rename: **`EmptyStatePanel`**
3. **Rect Transform**: Center, size 400x200
4. **Image**: White background

5. **Klik kanan** EmptyStatePanel → **UI** → **Text - TextMeshPro**
6. **Text**:
   ```
   No scores yet.
   Be the first to play!
   ```
7. Alignment: Center + Middle

8. Set **EmptyStatePanel** inactive ☐

#### Link to Inspector
1. Field **"Empty State Panel"**
2. Drag **EmptyStatePanel**

### 4.7.9 - Audio (Optional)
Jika ada audio:
```
Audio
  Sfx Source: [Drag AudioSource]
  Refresh Sound: [Drag AudioClip]
```

Biarkan None jika tidak ada.

### 4.7.10 - Final Verification

**Inspector Complete:**
```
Leaderboard UI (Script)
  UI References
    Content Parent: Content ✓
    Player Row Prefab: PlayerRow ✓
    Refresh Button: ButtonRefresh ✓
    Close Button: ButtonClose ✓
    Refresh Button Text: Text (TMP) ✓
    
  Level Filter
    Level Filter: 0 ✓
    Level Dropdown: None (Optional)
    
  Highlight Settings
    ✓ (Default colors OK)
    
  Loading State
    Loading Indicator: LoadingText ✓ (Optional)
    Empty State Panel: EmptyStatePanel ✓ (Optional)
    
  Audio
    (Optional - None OK)
```

### 4.7.11 - Save Scene
**Ctrl+S** - Save!

**✅ Checklist Step 4.7:**
- [ ] Content Parent linked ke ScrollView Content
- [ ] Player Row Prefab linked (dari Project window)
- [ ] Refresh Button & Text linked
- [ ] Close Button linked
- [ ] Loading Indicator & Empty State (optional) linked
- [ ] Scene saved

---

## 🎉 BAGIAN 4 SELESAI!

**Apa yang sudah kita capai:**
- ✅ Leaderboard Panel dibuat (full screen overlay)
- ✅ Header "🏆 LEADERBOARD 🏆" dibuat
- ✅ ScrollView dengan Vertical Layout Group configured
- ✅ PlayerRow Prefab dibuat (Rank, Name, Score, Stars)
- ✅ Refresh & Close buttons dibuat
- ✅ LeaderboardUI component ditambahkan & configured
- ✅ Loading & empty states (optional) dibuat

**Next:** Bagian 5 - Setup Level Selection dengan Star Display

---
---
---

# BAGIAN 5: UNITY SETUP - LEVEL SELECTION
## ⏱️ Estimasi: 20 menit

Sekarang kita akan tambahkan display bintang di setiap level button.

---

## Step 5.1: Locate Level Buttons

### 5.1.1 - Open Hub/Level Selection Scene
1. Di **Project window**, navigate: `Assets > _Scenes`
2. Double-click **`01_Hub_Klub.unity`**
   (Atau scene lain dimana ada level selection buttons)

### 5.1.2 - Find Level Buttons in Hierarchy
1. Di **Hierarchy**, search: `level` atau `button`
2. Atau expand Canvas dan cari manual
3. Biasanya ada multiple GameObjects: `LevelButton1`, `LevelButton2`, dll.

**Example Hierarchy:**
```
Canvas
├── LevelSelectionUI
│   ├── LevelButton1  ← Yang akan kita edit
│   ├── LevelButton2
│   ├── LevelButton3
│   └── ...
```

### 5.1.3 - Select First Level Button
1. Klik **LevelButton1** (atau nama yang sesuai)
2. **Inspector** menampilkan components

**Typical LevelButton structure:**
```
LevelButton1
├── Icon/Image Level
├── Text Level Name ("Level 1")
├── PadlockOverlay (jika level locked)
└── Button component
```

**✅ Checklist Step 5.1:**
- [ ] Scene Hub/Level Selection sudah terbuka
- [ ] Level buttons ditemukan di Hierarchy
- [ ] LevelButton1 (atau first button) terpilih

---

## Step 5.2: Add Star Icons

Sekarang kita tambahkan 3 icon bintang di setiap level button.

### 5.2.1 - Create Star Container (Optional)
1. **Klik kanan** LevelButton1 → **Create Empty**
2. Rename: **`StarsContainer`**
3. Di **Rect Transform**:
   ```
   Anchor Presets: Bottom Center
   Pivot: X 0.5, Y 0
   
   Pos X: 0
   Pos Y: 10
   Width: 100
   Height: 30
   ```

**Alternative**: Buat stars langsung sebagai child LevelButton1 (skip container).

### 5.2.2 - Create Star1
1. **Klik kanan** StarsContainer (atau LevelButton1) → **UI** → **Image**
2. Rename: **`Star1`**
3. **Rect Transform**:
   ```
   Pos X: -35
   Pos Y: 0
   Width: 25
   Height: 25
   ```
4. **Image** component:
   - **Source Image**: Pilih star sprite (atau placeholder "Knob")
   - **Color**: Gold (R 255, G 215, B 0)

### 5.2.3 - Create Star2 & Star3
1. Select Star1, tekan **Ctrl+D** (duplicate)
2. Rename: **`Star2`**
3. **Pos X: 0** (center)

4. Star2 terpilih, **Ctrl+D** lagi
5. Rename: **`Star3`**
6. **Pos X: 35** (right)

**Hierarchy LevelButton1:**
```
LevelButton1
├── Icon
├── Text Level Name
├── StarsContainer
│   ├── Star1  (Pos X: -35)
│   ├── Star2  (Pos X: 0)
│   └── Star3  (Pos X: 35)
└── PadlockOverlay
```

**Preview Button:**
```
┌────────────────┐
│  [Level Icon]  │
│   Level 1      │
│                │
│   ⭐ ⭐ ⭐    │ ← Stars di bawah
└────────────────┘
```

### 5.2.4 - Set Stars Inactive by Default
1. Select **Star1**, uncheck ☐ di Inspector (set inactive)
2. Repeat untuk **Star2** dan **Star3**
3. Stars akan di-activate via code berdasarkan saved data

**✅ Checklist Step 5.2:**
- [ ] StarsContainer (optional) dibuat
- [ ] 3 star icons (Star1, Star2, Star3) dibuat
- [ ] Position: X = -35, 0, 35
- [ ] Semua stars set inactive ☐ (default hidden)

---

## Step 5.3: Add Best Score Text

### 5.3.1 - Create Text GameObject
1. **Klik kanan** LevelButton1 → **UI** → **Text - TextMeshPro**
2. Rename: **`BestScoreText`**

### 5.3.2 - Position & Style Text
1. **Rect Transform**:
   ```
   Anchor Presets: Bottom Center
   Pivot: X 0.5, Y 0
   
   Pos X: 0
   Pos Y: 40  (di atas stars)
   Width: 120
   Height: 25
   ```

2. **TextMeshPro - Text**:
   ```
   Text: Best: 95
   Font Size: 18
   Alignment: Center + Middle
   Color: White (atau gray untuk subtle)
   Font Style: Normal (tidak bold)
   ```

### 5.3.3 - Set Text Default
Default text bisa "Not Played" atau kosong. Nanti di-update via code.

**Preview Button with Score:**
```
┌────────────────┐
│  [Level Icon]  │
│   Level 1      │
│   Best: 95     │ ← Best score
│   ⭐ ⭐ ⭐    │ ← Stars
└────────────────┘
```

**✅ Checklist Step 5.3:**
- [ ] BestScoreText dibuat di LevelButton1
- [ ] Position: Di atas stars (Y = 40)
- [ ] Text: "Best: 95" (default/placeholder)

---

## Step 5.4: Configure LevelButton

### 5.4.1 - Verify LevelButton Component
1. Select **LevelButton1** (parent GameObject)
2. Di **Inspector**, cari component **"Level Button (Script)"**

**Existing fields:**
```
Level Button (Script)
  Level Info
    Level Index: 1
    Scene Name: 03_Game_Processing
    
  UI References
    Padlock Overlay: ...
    Button Component: ...
    
  Audio Settings
    ...
```

### 5.4.2 - Scroll to New Fields
Karena kita sudah update script `LevelButton.cs` (Part 1), sekarang ada fields baru:

```
  Star Rating Display
    Star Icons
      Size: 0  ← Akan diisi
    Best Score Text: None
```

### 5.4.3 - Configure Star Icons Array
1. Field **"Star Icons"**, change **Size: 0** → **Size: 3**
2. Array expand:
   ```
   Star Icons
     Size: 3
     Element 0: None
     Element 1: None
     Element 2: None
   ```

3. Dari Hierarchy, expand **StarsContainer**
4. **Drag** Star1 ke **Element 0**
5. **Drag** Star2 ke **Element 1**
6. **Drag** Star3 ke **Element 2**

**Result:**
```
Star Icons
  Size: 3
  Element 0: Star1 (GameObject) ✓
  Element 1: Star2 (GameObject) ✓
  Element 2: Star3 (GameObject) ✓
```

### 5.4.4 - Link Best Score Text
1. Field **"Best Score Text"**
2. **Drag** GameObject **BestScoreText** dari Hierarchy

**Result:**
```
Best Score Text: BestScoreText (TextMeshPro) ✓
```

### 5.4.5 - Save as Prefab Variant (Optional)
Jika level buttons adalah prefab:
1. Select LevelButton1
2. Di **Inspector** atas, klik **"Overrides"** → **"Apply All"**
3. Changes disimpan ke prefab

Atau manual save per button (jika bukan prefab).

### 5.4.6 - Repeat for Other Level Buttons
Ulangi Step 5.2-5.4 untuk:
- LevelButton2
- LevelButton3
- LevelButton4
- ... dst semua level buttons

**⚠️ TIPS**: Jika buttons adalah prefab instances:
- Edit 1 button (LevelButton1)
- Apply overrides ke prefab
- Other instances akan auto-update

**Jika bukan prefab**: Harus manual edit setiap button 😅

**✅ Checklist Step 5.4:**
- [ ] LevelButton script verified (memiliki Star Rating Display fields)
- [ ] Star Icons array size 3, semua elements di-link
- [ ] Best Score Text di-link
- [ ] Changes applied (prefab atau manual)
- [ ] Repeat untuk semua level buttons

---

## 🎉 BAGIAN 5 SELESAI!

**Apa yang sudah kita capai:**
- ✅ Level selection scene terbuka
- ✅ Level buttons ditemukan & selected
- ✅ 3 star icons ditambahkan per button (inactive by default)
- ✅ Best score text ditambahkan per button
- ✅ LevelButton script configured (star icons array & text linked)
- ✅ Repeat untuk semua level buttons

**Next:** Bagian 6 - Testing Complete Flow!

---
---
---

# BAGIAN 6: TESTING
## ⏱️ Estimasi: 15 menit

Sekarang kita test apakah semua sistem bekerja dengan benar.

---

## Step 6.1: Test Star Rating

### 6.1.1 - Setup Test Environment
1. Buka scene **`03_Game_Processing.unity`** (atau game scene lain)
2. **Play** mode (tekan **Play** button ▶ atau F5)

### 6.1.2 - Play Through Level
1. Main game sampai selesai (menang)
2. Win Panel muncul

### 6.1.3 - Verify Star Display
**Expected Behavior:**
1. Win Panel muncul
2. **Stars muncul satu-per-satu** dengan animasi scale (0 → 1)
3. **Sound effect** play (jika ada audio)
4. Bintang terisi sesuai skor:
   - Skor 90-100 → 3 bintang ⭐⭐⭐
   - Skor 75-89 → 2 bintang ⭐⭐
   - Skor 50-74 → 1 bintang ⭐
5. **Delay 0.3 detik** antar bintang

### 6.1.4 - Check Console Logs
Buka **Console window** (Window → General → Console):

**Expected Logs:**
```
⭐ [WIN PANEL] Displaying 3 stars (Record: True)
⭐ [SHOW WIN PANEL] Skor: 95 → 3 Bintang
🎉 [SHOW WIN PANEL] NEW RECORD untuk Level 1!
✅ [STAR RATING] Level 1 - New Best: 3 bintang! (Previous: 0)
🏆 [STAR RATING] Level 1 - New High Score: 95! (Previous: 0)
✅ Win Panel ditampilkan!
✅ [WIN PANEL] Star animation complete!
```

### 6.1.5 - Check New Record Popup
**If first play or beat previous best:**
- **"NEW RECORD!" popup** muncul setelah stars
- Sound effect play (if audio configured)

**If not new record:**
- Popup tidak muncul (expected)

### 6.1.6 - Stop Play Mode
Tekan **Play** button lagi (stop)

**✅ Test 1 Passed If:**
- [ ] Stars muncul dengan animasi smooth
- [ ] Jumlah bintang sesuai skor (1-3)
- [ ] New Record popup muncul (jika applicable)
- [ ] Console logs show "✅ Star animation complete!"
- [ ] No errors di Console

**❌ If Failed**: Lihat [Troubleshooting](#common-problems--solutions)

---

## Step 6.2: Test Upload Score

### 6.2.1 - Check LeaderboardManager Setup
1. Buka scene **`00_MainMenu.unity`**
2. Select **LeaderboardManager** GameObject di Hierarchy
3. **Inspector** verify:
   ```
   Google Apps Script URL: [URL lengkap filled]
   Class Name: 3A
   Player Name: Player1
   Debug Mode: ☑ Checked
   ```

### 6.2.2 - Play Game Scene Again
1. Buka scene **`03_Game_Processing.unity`**
2. **Play** mode
3. Main sampai menang

### 6.2.3 - Click Upload Button
1. Win Panel muncul dengan stars
2. **Click button "📤 Upload to Leaderboard"**
3. Button text berubah: **"Uploading..."**

### 6.2.4 - Check Console Logs
**Expected Logs (Upload Success):**
```
📤 [WIN PANEL] Uploading score: 95 (3★) for Level 1
==================================================
📤 [LEADERBOARD] Uploading score...
   Player: Player1
   Class: 3A
   Level: 1
   Score: 95
   Stars: 3
📄 [LEADERBOARD] JSON Data: {"action":"upload","playerName":"Player1",...}
✅ [LEADERBOARD] Upload SUCCESS!
   Response: {"status":"success","message":"Score uploaded successfully!"}
==================================================
📤 [WIN PANEL] Upload SUCCESS
```

**Button text berubah**: **"Uploaded! ✓"**

### 6.2.5 - Verify Google Sheets
1. Switch ke browser, buka **Google Sheets** leaderboard
2. **Refresh page** (F5)
3. **New row added** dengan data:
   ```
   | Player1 | 3A | 1 | 95 | 3 | 2025-12-08 15:30:00 |
   ```

### 6.2.6 - Stop Play Mode

**✅ Test 2 Passed If:**
- [ ] Upload button clicked
- [ ] Button text: "Uploading..." → "Uploaded! ✓"
- [ ] Console logs show "✅ Upload SUCCESS!"
- [ ] Google Sheets has new row with correct data
- [ ] No errors di Console

**❌ If Upload Failed**: Check Console error message:
- **"Google Apps Script URL kosong"** → Paste URL di Inspector
- **"Timeout"** → Check internet connection
- **"Error 404"** → URL salah, re-deploy Apps Script
- **"Unknown action"** → Update Apps Script code (Part 1 Step 1.3)

---

## Step 6.3: Test Leaderboard Display

### 6.3.1 - Setup Leaderboard Button (If Not Exist)
Di scene Main Menu, buat button untuk open leaderboard:

1. **Klik kanan** Canvas → **UI** → **Button - TextMeshPro**
2. Rename: **`ButtonLeaderboard`**
3. Position & text: "🏆 Leaderboard"

4. Select button, di **Inspector** → **Button** component
5. **On Click ()** → Add event:
   - Click **+**
   - Drag **LeaderboardPanel** ke object field
   - Function: **GameObject** → **SetActive** (bool)
   - Checkbox: **☑ Check** (true)

### 6.3.2 - Play Main Menu Scene
1. Scene **`00_MainMenu.unity`** terbuka
2. **Play** mode

### 6.3.3 - Click Leaderboard Button
1. Click button **"🏆 Leaderboard"**
2. Leaderboard Panel muncul (overlay full screen)

### 6.3.4 - Verify Loading State
**Expected sequence:**
1. Panel muncul
2. **"Loading..."** text muncul (jika configured)
3. Spinner/animation (if any)

### 6.3.5 - Check Console Logs
```
🔄 [LEADERBOARD UI] Refreshing leaderboard (Level: All)
==================================================
📥 [LEADERBOARD] Downloading leaderboard...
   Class: 3A
   Level: 0
🔗 [LEADERBOARD] Request URL: https://script.google.com/.../exec?action=getLeaderboard&className=3A&level=0
📄 [LEADERBOARD] JSON Response: [{"playerName":"Player1","score":95,...},...]
✅ [LEADERBOARD] Download SUCCESS!
   Total Entries: 4
==================================================
✅ [LEADERBOARD UI] Populated 4 entries
✨ [LEADERBOARD UI] Highlighted player 'Player1' at rank 1
```

### 6.3.6 - Verify Leaderboard Display
**Panel shows:**
```
╔════════════════════════════════════╗
║   🏆 LEADERBOARD 🏆                ║
║  ┌──────────────────────────────┐  ║
║  │ 🥇 │ Player1  │  95  │ ⭐⭐⭐ │  ║ ← Highlighted (gold bg)
║  │ 🥈 │ Daffa    │  87  │ ⭐⭐  │  ║
║  │ 🥉 │ Budi     │  78  │ ⭐⭐  │  ║
║  │ 4  │ Siti     │  65  │ ⭐    │  ║
║  └──────────────────────────────┘  ║
║                                    ║
║   [🔄 Refresh]     [✖ Close]       ║
╚════════════════════════════════════╝
```

**Check:**
- **Rank 1-3**: Display 🥇🥈🥉 emoji
- **Rank 4+**: Display number (4, 5, 6, ...)
- **Current player row**: Gold background (highlighted)
- **Stars**: Display ⭐ emoji (1-3)
- **Scroll**: Can scroll if many entries

### 6.3.7 - Test Refresh Button
1. Click **"🔄 Refresh"**
2. Button text: **"Loading..."**
3. Data re-downloaded
4. List updated
5. Button text: **"Refresh"**

### 6.3.8 - Test Close Button
1. Click **"✖ Close"**
2. Leaderboard Panel hilang (inactive)
3. Back to Main Menu

### 6.3.9 - Stop Play Mode

**✅ Test 3 Passed If:**
- [ ] Leaderboard button opens panel
- [ ] Loading state shows briefly
- [ ] Player rows populated dengan data dari Google Sheets
- [ ] Top 3 have trophy emoji 🥇🥈🥉
- [ ] Current player row highlighted (gold bg)
- [ ] Refresh button works (re-download data)
- [ ] Close button works (hide panel)
- [ ] Console logs show "✅ Download SUCCESS!"
- [ ] No errors

**❌ If Failed**: Check Console error, verify Google Apps Script URL

---

## Step 6.4: Test Level Selection Stars

### 6.4.1 - Play Level Selection Scene
1. Open scene **`01_Hub_Klub.unity`**
2. **Play** mode

### 6.4.2 - Verify Stars Display
**Expected:**
- **Level 1** (yang baru dimainkan):
  - **3 stars active** ⭐⭐⭐ (visible)
  - Text: **"Best: 95"**

- **Other levels** (belum dimainkan):
  - **Stars inactive** (hidden/invisible)
  - Text: **"Not Played"** atau blank

### 6.4.3 - Check Console Logs
```
⭐ LevelButton [1]: Best Stars = 3, Best Score = 95
⭐ LevelButton [2]: Best Stars = 0, Best Score = 0
⭐ LevelButton [3]: Best Stars = 0, Best Score = 0
...
```

### 6.4.4 - Play Another Level
1. Stop Play mode
2. Open level scene (contoh Level 2)
3. Play & finish dengan score berbeda (contoh: 82)
4. Win Panel → Stars muncul (2 bintang) ⭐⭐
5. Stop Play mode

### 6.4.5 - Check Level Selection Again
1. Play scene Hub/Level Selection
2. **Level 1**: Still 3 stars ⭐⭐⭐
3. **Level 2**: Now 2 stars ⭐⭐ (NEW!)
4. Text: "Best: 82"

**✅ Test 4 Passed If:**
- [ ] Stars display correctly per level (based on best score)
- [ ] Best score text shows correct number
- [ ] Not played levels show no stars
- [ ] After playing new level, stars appear correctly
- [ ] PlayerPrefs persists across Play mode sessions

**❌ If Not Showing**: Check LevelButton Inspector (Star Icons array linked?)

---

## 🎉 BAGIAN 6 SELESAI!

**All Tests Summary:**
- ✅ Test 1: Star Rating Animation (Win Panel)
- ✅ Test 2: Upload Score to Google Sheets
- ✅ Test 3: Leaderboard Display & Refresh
- ✅ Test 4: Level Selection Stars Persistence

**If ALL tests passed**: 🎊 **CONGRATULATIONS!** Sistema berhasil! 🎊

**If ANY test failed**: Lanjut ke Bagian 7 - Troubleshooting

---
---
---

# BAGIAN 7: TROUBLESHOOTING
## 🔧 Common Problems & Solutions

Jika ada masalah saat testing, check section ini.

---

## Problem 1: Stars Tidak Muncul di Win Panel

### Symptoms:
- Win Panel muncul
- Skor & waktu tampil
- **Tapi stars tidak muncul** (blank)

### Possible Causes & Solutions:

#### Cause A: Star GameObjects Tidak Di-link
**Check:**
1. Select Win Panel di Hierarchy
2. Inspector → **Win Panel Controller** component
3. Field **"Star Objects"** → Size harus **3**
4. Element 0, 1, 2 harus ada GameObjects (tidak None)

**Fix:**
- Drag Star1, Star2, Star3 ke array elements
- Save scene

#### Cause B: Star Scale Tidak Di-set ke 0
**Check:**
1. Select Star1 di Hierarchy
2. Inspector → **Rect Transform**
3. Scale: X, Y harus **0** (bukan 1)

**Fix:**
- Set Scale X: 0, Y: 0, Z: 1
- Repeat untuk Star2, Star3

#### Cause C: Sprite Tidak Di-set
**Check:**
1. Select Star1
2. Inspector → **Image** component
3. Field **"Source Image"** → Harus ada sprite (tidak None)

**Fix:**
- Assign sprite (placeholder Knob, atau import star sprite)

#### Cause D: WinPanelController Script Tidak Attached
**Check:**
1. Select Win Panel
2. Inspector → Cari component **"Win Panel Controller (Script)"**

**Fix:**
- Add Component → WinPanelController

---

## Problem 2: Upload Button Tidak Ada Response

### Symptoms:
- Click button "Upload to Leaderboard"
- **Tidak ada reaksi** (no log, no text change)

### Possible Causes & Solutions:

#### Cause A: LeaderboardManager Tidak Ada
**Check:**
1. Open scene Main Menu
2. Hierarchy → Cari GameObject **"LeaderboardManager"**

**Fix:**
- Buat LeaderboardManager di Main Menu (Part 1 Bagian 2)
- Set **DontDestroyOnLoad** agar persist antar scene

#### Cause B: Google Apps Script URL Kosong
**Check Console:**
```
❌ [LEADERBOARD] Cannot upload - Google Apps Script URL kosong!
```

**Fix:**
1. Select LeaderboardManager
2. Inspector → Paste Web App URL ke field "Google Apps Script URL"

#### Cause C: Button OnClick Event Tidak Setup
**Check:**
1. Select ButtonUploadLeaderboard
2. Inspector → **Button** component
3. **On Click ()** → Harus ada event

**Fix:**
- Add event: WinPanelController → OnClickUploadToLeaderboard()

---

## Problem 3: Upload Error "Timeout" atau "Connection Failed"

### Symptoms:
Console shows:
```
❌ [LEADERBOARD] Upload FAILED!
   Error: Timeout
   Response Code: 0
```

### Possible Causes & Solutions:

#### Cause A: No Internet Connection
**Check:**
- Test browser akses Google.com
- Ping google.com di CMD/Terminal

**Fix:**
- Connect to internet
- Re-test upload

#### Cause B: Google Apps Script URL Salah
**Check:**
- URL harus dimulai `https://script.google.com/macros/s/`
- URL harus diakhiri `/exec`

**Fix:**
- Re-copy URL dari Apps Script deployment
- Paste ulang ke Inspector

#### Cause C: Apps Script Not Deployed Properly
**Test:**
- Paste URL di browser
- Harus muncul JSON response (bukan error HTML)

**Fix:**
- Re-deploy Apps Script (Part 1 Step 1.5)
- Deploy → New deployment → Copy new URL

---

## Problem 4: Leaderboard Panel Tidak Muncul

### Symptoms:
- Click leaderboard button
- **Panel tetap tidak muncul**

### Possible Causes & Solutions:

#### Cause A: Panel Set Active, Tapi Invisible
**Check:**
1. Hierarchy → LeaderboardPanel
2. Inspector → Checkbox di atas harus ☑ checked saat Play

**Fix:**
- Panel default inactive ☐ di Edit mode
- Tapi harus ☑ active saat button clicked

#### Cause B: Button OnClick Tidak Setup
**Check:**
1. Select button Leaderboard
2. Inspector → Button → On Click ()
3. Harus ada event: LeaderboardPanel → SetActive(true)

**Fix:**
- Add event manually
- Or call via code: `leaderboardPanel.SetActive(true);`

#### Cause C: Canvas Render Issue
**Check:**
- Panel active tapi di belakang other UI

**Fix:**
1. Select LeaderboardPanel
2. Hierarchy → Drag ke **bawah sendiri** (render last = on top)
3. Or add Canvas component dengan Sort Order tinggi

---

## Problem 5: Leaderboard Kosong (No Rows)

### Symptoms:
- Panel muncul
- Loading selesai
- **Empty State "No scores yet"**

### Console Shows:
```
📭 [LEADERBOARD UI] Leaderboard kosong (belum ada data)
```

### Possible Causes & Solutions:

#### Cause A: Google Sheets Kosong
**Check:**
- Buka Google Sheets di browser
- Pastikan ada data (minimal 3 sample rows)

**Fix:**
- Run `setupSpreadsheet` function lagi di Apps Script
- Or manual add data rows

#### Cause B: Class Name Filter Salah
**Check:**
1. LeaderboardManager → Class Name: `3A`
2. Google Sheets data → ClassName column: `3A`
3. **Harus MATCH** (case-sensitive!)

**Fix:**
- Ganti Class Name di Inspector sesuai data
- Or ganti data di Sheets

#### Cause C: Level Filter Tidak Match
**Check:**
1. LeaderboardUI → Level Filter: `1`
2. Google Sheets → Level column: `1`, `2`, `3`

**Fix:**
- Set Level Filter: `0` (all levels)
- Or filter by specific level dengan data yang ada

---

## Problem 6: Player Row Tidak Ter-highlight

### Symptoms:
- Leaderboard menampilkan data
- **Tapi tidak ada row gold** (semua putih)

### Possible Causes & Solutions:

#### Cause A: Player Name Tidak Match
**Check:**
1. LeaderboardManager → Player Name: `"Player1"`
2. Google Sheets data → PlayerName: `"Player1"`
3. **Harus MATCH EXACTLY** (case-sensitive!)

**Fix:**
- Ganti Player Name di Inspector
- Or update Sheets data

#### Cause B: Highlight Color Transparent
**Check:**
1. Select LeaderboardPanel
2. Inspector → Leaderboard UI → Highlight Color
3. Alpha harus > 0 (not fully transparent)

**Fix:**
- Set Alpha: 0.3 (30% opacity)
- Color: Gold (R 1, G 0.92, B 0.016)

---

## Problem 7: Stars Tidak Muncul di Level Selection

### Symptoms:
- Level buttons tampil
- **Tapi stars tidak visible**

### Possible Causes & Solutions:

#### Cause A: Stars Tidak Di-link di Inspector
**Check:**
1. Select LevelButton1
2. Inspector → Level Button (Script)
3. **Star Icons** array → Size: 3, elements filled?

**Fix:**
- Link Star1, Star2, Star3 ke array
- Repeat untuk all level buttons

#### Cause B: GameManager Instance Null
**Check Console:**
```
NullReferenceException: Object reference not set to instance
at LevelButton.UpdateStarDisplay()
```

**Fix:**
- Pastikan GameManager ada di scene
- Or check GameManager.Instance singleton

#### Cause C: PlayerPrefs Tidak Tersimpan
**Check:**
1. Play game → Win → Score saved?
2. Console log: "✅ [STAR RATING] Level X - New Best: Y bintang"

**Fix:**
- GameManager.ShowWinPanel() harus call SaveBestStars()
- Check code di GameManager.cs (Part 1 update)

---

## Problem 8: "Unknown action" Error saat Test Browser

### Symptoms:
Browser shows:
```json
{
  "status": "error",
  "message": "Unknown action"
}
```

### Solution:
Sudah dijelaskan di file **`FIX_ERROR_UNKNOWN_ACTION.md`**

**Quick Fix:**
1. Update Apps Script code (copy dari `GoogleAppsScript_Leaderboard.js`)
2. Re-deploy
3. Test ulang

---

## Problem 9: Console Log "Missing Component"

### Symptoms:
```
MissingReferenceException: The object of type 'WinPanelController' has been destroyed but you are still trying to access it.
```

### Possible Causes:
- Script reference hilang
- GameObject deleted tapi masih referenced

### Solution:
1. Check all Inspector fields (no missing scripts marked as ⚠️)
2. Re-assign components
3. Save scene

---

## Problem 10: Build Error "Type or namespace not found"

### Symptoms:
```
error CS0246: The type or namespace name 'LeaderboardManager' could not be found
```

### Solution:
1. Check script files ada di folder correct:
   - `Assets/_Scripts/Manager/LeaderboardManager.cs`
   - `Assets/_Scripts/UI/WinPanelController.cs`
   - `Assets/_Scripts/UI/LeaderboardUI.cs`

2. Refresh Assets: **Ctrl+R** atau **Assets → Refresh**

3. Reimport scripts: Klik kanan folder → **Reimport**

---

## 🆘 STILL NOT WORKING?

### Debug Checklist:

#### For Star Rating Issues:
- [ ] GameManager.cs updated dengan star functions?
- [ ] Win Panel stars exist & scale = 0?
- [ ] WinPanelController attached & configured?
- [ ] Console shows star calculation logs?

#### For Upload Issues:
- [ ] LeaderboardManager exists di Main Menu?
- [ ] Google Apps Script URL filled & correct?
- [ ] Debug Mode enabled (check logs)?
- [ ] Internet connection OK?
- [ ] Test URL di browser (JSON response)?

#### For Leaderboard Issues:
- [ ] LeaderboardPanel configured properly?
- [ ] Player Row Prefab exists & linked?
- [ ] Google Sheets has data?
- [ ] Class Name & Level Filter match data?

#### For Level Selection Issues:
- [ ] LevelButton.cs updated with star fields?
- [ ] Stars added to all buttons?
- [ ] Star Icons array linked?
- [ ] Best Score Text linked?

### Enable Debug Logs:
1. LeaderboardManager → Debug Mode: ☑
2. Play mode → Check Console for detailed logs
3. Every function call logged

### Contact/Community:
- Check Unity docs: `docs.unity3d.com`
- UnityWebRequest docs
- Google Apps Script reference

---

## 🎉 TUTORIAL SELESAI!

Jika semua berhasil, kamu sekarang punya:
- ✅ Star Rating System (1-3 bintang)
- ✅ Google Sheets Leaderboard (upload & download)
- ✅ Win Panel dengan star animation
- ✅ Leaderboard Panel dengan ScrollView
- ✅ Level Selection dengan star display
- ✅ PlayerPrefs persistence

**Total Development Time:** ~2-3 hours (first setup)

**Next Steps (Optional):**
1. Import proper star sprites (from AI generation)
2. Add audio SFX (star appear, upload success)
3. Add particle effects (confetti for new record)
4. Implement player name input field
5. Add class selection dropdown
6. Implement level filter in leaderboard

**🎊 CONGRATULATIONS! 🎊**

---

**Created by**: GitHub Copilot  
**Date**: 2025-12-08  
**Part**: 2 of 2 (Complete)  
**Total Words**: 18,000+  
**Total Steps**: 50+ detailed steps
