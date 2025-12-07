// ============================================
// GOOGLE APPS SCRIPT - ECO QUEST LEADERBOARD
// ============================================
// Deploy as: Web App
// Execute as: Me
// Who has access: Anyone

// ⚠️ TIDAK PERLU GANTI SPREADSHEET_ID LAGI!
// Script ini otomatis akses spreadsheet dimana script ini berada

// Nama sheet (default "Sheet1", sesuaikan jika beda)
var SHEET_NAME = "Sheet1";

// ============================================
// TEST ENDPOINT - Akses via Browser
// ============================================
// URL: https://script.google.com/macros/s/[YOUR_DEPLOYMENT_ID]/exec
// Response: {"status":"success","message":"Leaderboard API is running!","timestamp":"..."}

function doGet(e) {
  Logger.log("=== doGet called ===");
  Logger.log("Parameters: " + JSON.stringify(e.parameter));
  
  // Jika tidak ada parameter, return test response
  if (!e.parameter || Object.keys(e.parameter).length === 0) {
    return ContentService.createTextOutput(JSON.stringify({
      "status": "success",
      "message": "Eco Quest Leaderboard API is running!",
      "timestamp": new Date().toISOString(),
      "endpoints": {
        "test": "GET ?action=test",
        "getLeaderboard": "GET ?action=getLeaderboard&className=3A&level=1",
        "upload": "POST with JSON body {action:'upload', playerName:'...', ...}"
      },
      "version": "1.0"
    })).setMimeType(ContentService.MimeType.JSON);
  }
  
  var action = e.parameter.action;
  
  try {
    // TEST ACTION
    if (action === "test") {
      return ContentService.createTextOutput(JSON.stringify({
        "status": "success",
        "message": "Test endpoint working!",
        "receivedParams": e.parameter,
        "timestamp": new Date().toISOString()
      })).setMimeType(ContentService.MimeType.JSON);
    }
    
    // GET LEADERBOARD ACTION
    if (action === "getLeaderboard") {
      var className = e.parameter.className || "";
      var level = parseInt(e.parameter.level) || 0;
      
      Logger.log("Getting leaderboard - Class: " + className + ", Level: " + level);
      
      var leaderboard = getLeaderboardData(className, level);
      
      return ContentService.createTextOutput(JSON.stringify(leaderboard))
        .setMimeType(ContentService.MimeType.JSON);
    }
    
    // Unknown action
    return ContentService.createTextOutput(JSON.stringify({
      "status": "error",
      "message": "Unknown action: " + action + ". Valid actions: test, getLeaderboard"
    })).setMimeType(ContentService.MimeType.JSON);
    
  } catch (error) {
    Logger.log("Error in doGet: " + error.toString());
    return ContentService.createTextOutput(JSON.stringify({
      "status": "error",
      "message": error.toString()
    })).setMimeType(ContentService.MimeType.JSON);
  }
}

// ============================================
// POST ENDPOINT - Upload Score dari Unity
// ============================================
function doPost(e) {
  Logger.log("=== doPost called ===");
  Logger.log("Post data: " + e.postData.contents);
  
  try {
    // Parse JSON dari Unity
    var data = JSON.parse(e.postData.contents);
    
    Logger.log("Parsed data: " + JSON.stringify(data));
    
    var action = data.action;
    
    // UPLOAD SCORE ACTION
    if (action === "upload") {
      var playerName = data.playerName || "Unknown";
      var className = data.className || "Unknown";
      var level = parseInt(data.level) || 0;
      var score = parseInt(data.score) || 0;
      var stars = parseInt(data.stars) || 0;
      var timestamp = data.timestamp || new Date().toISOString();
      
      Logger.log("Uploading score - Player: " + playerName + ", Score: " + score);
      
      // Validasi data
      if (!playerName || !className || level === 0) {
        throw new Error("Missing required fields: playerName, className, or level");
      }
      
      // Simpan ke spreadsheet
      appendScoreToSheet(playerName, className, level, score, stars, timestamp);
      
      return ContentService.createTextOutput(JSON.stringify({
        "status": "success",
        "message": "Score uploaded successfully!",
        "data": {
          "playerName": playerName,
          "className": className,
          "level": level,
          "score": score,
          "stars": stars
        }
      })).setMimeType(ContentService.MimeType.JSON);
    }
    
    // Unknown action
    return ContentService.createTextOutput(JSON.stringify({
      "status": "error",
      "message": "Unknown action: " + action + ". Valid actions: upload"
    })).setMimeType(ContentService.MimeType.JSON);
    
  } catch (error) {
    Logger.log("Error in doPost: " + error.toString());
    return ContentService.createTextOutput(JSON.stringify({
      "status": "error",
      "message": error.toString()
    })).setMimeType(ContentService.MimeType.JSON);
  }
}

// ============================================
// HELPER FUNCTIONS
// ============================================

/**
 * Append score data ke spreadsheet
 */
function appendScoreToSheet(playerName, className, level, score, stars, timestamp) {
  try {
    var ss = SpreadsheetApp.getActiveSpreadsheet();
    var sheet = ss.getSheetByName(SHEET_NAME);
    
    if (!sheet) {
      throw new Error("Sheet not found: " + SHEET_NAME);
    }
    
    // Append row baru
    sheet.appendRow([
      playerName,
      className,
      level,
      score,
      stars,
      timestamp
    ]);
    
    Logger.log("✅ Score appended successfully!");
    
  } catch (error) {
    Logger.log("❌ Error appending score: " + error.toString());
    throw error;
  }
}

/**
 * Get leaderboard data dengan filter
 */
function getLeaderboardData(className, level) {
  try {
    var ss = SpreadsheetApp.getActiveSpreadsheet();
    var sheet = ss.getSheetByName(SHEET_NAME);
    
    if (!sheet) {
      throw new Error("Sheet not found: " + SHEET_NAME);
    }
    
    // Get all data (skip header row)
    var data = sheet.getDataRange().getValues();
    var headers = data[0]; // Row 1 = headers
    var rows = data.slice(1); // Row 2+ = data
    
    Logger.log("Total rows: " + rows.length);
    
    // Filter data
    var filtered = [];
    
    for (var i = 0; i < rows.length; i++) {
      var row = rows[i];
      
      // Skip empty rows
      if (!row[0]) continue;
      
      var rowPlayerName = row[0];
      var rowClassName = row[1];
      var rowLevel = parseInt(row[2]);
      var rowScore = parseInt(row[3]);
      var rowStars = parseInt(row[4]);
      var rowTimestamp = row[5];
      
      // Filter by class
      if (className && rowClassName !== className) {
        continue;
      }
      
      // Filter by level (0 = all levels)
      if (level !== 0 && rowLevel !== level) {
        continue;
      }
      
      // Add to filtered array
      filtered.push({
        "playerName": rowPlayerName,
        "className": rowClassName,
        "level": rowLevel,
        "score": rowScore,
        "stars": rowStars,
        "timestamp": rowTimestamp
      });
    }
    
    // Sort by score (descending)
    filtered.sort(function(a, b) {
      return b.score - a.score;
    });
    
    // Assign ranks
    for (var j = 0; j < filtered.length; j++) {
      filtered[j].rank = j + 1;
    }
    
    Logger.log("✅ Filtered rows: " + filtered.length);
    
    return filtered;
    
  } catch (error) {
    Logger.log("❌ Error getting leaderboard: " + error.toString());
    throw error;
  }
}

// ============================================
// SETUP FUNCTION - Run Once untuk Create Sheet
// ============================================
/**
 * Function ini dijalankan SEKALI untuk setup spreadsheet
 * Cara: Buka Apps Script Editor → Select "setupSpreadsheet" → Click Run ▶
 */
function setupSpreadsheet() {
  try {
    var ss = SpreadsheetApp.getActiveSpreadsheet();
    var sheet = ss.getSheetByName(SHEET_NAME);
    
    // Jika sheet belum ada, create new
    if (!sheet) {
      sheet = ss.insertSheet(SHEET_NAME);
      Logger.log("✅ Created new sheet: " + SHEET_NAME);
    }
    
    // Clear existing data (hati-hati! Comment baris ini jika sudah ada data)
    // sheet.clear();
    
    // Set headers
    sheet.getRange(1, 1, 1, 6).setValues([[
      "PlayerName",
      "ClassName",
      "Level",
      "Score",
      "Stars",
      "Timestamp"
    ]]);
    
    // Format headers (bold, background color)
    var headerRange = sheet.getRange(1, 1, 1, 6);
    headerRange.setFontWeight("bold");
    headerRange.setBackground("#4CAF50");
    headerRange.setFontColor("#FFFFFF");
    
    // Set column widths
    sheet.setColumnWidth(1, 150); // PlayerName
    sheet.setColumnWidth(2, 100); // ClassName
    sheet.setColumnWidth(3, 80);  // Level
    sheet.setColumnWidth(4, 80);  // Score
    sheet.setColumnWidth(5, 80);  // Stars
    sheet.setColumnWidth(6, 180); // Timestamp
    
    // Freeze header row
    sheet.setFrozenRows(1);
    
    // Add sample data (optional, comment jika tidak mau)
    sheet.appendRow(["Daffa", "3A", 1, 95, 3, new Date().toISOString()]);
    sheet.appendRow(["Budi", "3A", 1, 87, 2, new Date().toISOString()]);
    sheet.appendRow(["Siti", "3A", 1, 78, 2, new Date().toISOString()]);
    
    Logger.log("✅ Spreadsheet setup complete!");
    Logger.log("Sheet Name: " + SHEET_NAME);
    Logger.log("Spreadsheet URL: " + ss.getUrl());
    
    // Return success message
    Browser.msgBox(
      "Setup Complete!",
      "Spreadsheet berhasil di-setup dengan headers dan sample data.\\n\\n" +
      "Next step: Deploy as Web App\\n" +
      "1. Click Deploy → New deployment\\n" +
      "2. Type: Web app\\n" +
      "3. Execute as: Me\\n" +
      "4. Who has access: Anyone\\n" +
      "5. Click Deploy\\n" +
      "6. Copy Web App URL",
      Browser.Buttons.OK
    );
    
  } catch (error) {
    Logger.log("❌ Error in setup: " + error.toString());
    Browser.msgBox("Error", error.toString(), Browser.Buttons.OK);
  }
}
