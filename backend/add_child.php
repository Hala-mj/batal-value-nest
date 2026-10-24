<?php
session_start();
include 'db_connect.php';

if ($_SERVER["REQUEST_METHOD"] == "POST") {
    $name = isset($_POST["name"]) ? trim($_POST["name"]) : '';
    $age = isset($_POST["age"]) ? intval($_POST["age"]) : 0;
    $avatarID = isset($_POST["avatarID"]) ? intval($_POST["avatarID"]) : 0;
    $parentID = isset($_POST["parentID"]) ? intval($_POST["parentID"]) : 0;

    if (empty($name) || $age <= 0 || $avatarID <= 0 || $parentID <= 0) {
        echo json_encode(['success' => false, 'message' => 'Missing or invalid required fields!']);
        exit();
    }

    // Insert into existing database schema
    $stmt = $conn->prepare("INSERT INTO child (Name, Age, IconID, ParentID, Coins) VALUES (?, ?, ?, ?, ?)");
    $coins = 0; // Default value for coins
    $stmt->bind_param("siiii", $name, $age, $avatarID, $parentID, $coins);

    if ($stmt->execute()) {
        echo json_encode(['success' => true, 'message' => 'Child added successfully']);
    } else {
        error_log("Database Error: " . $stmt->error);
        echo json_encode(['success' => false, 'message' => 'Failed to add child']);
    }

    $stmt->close();
}
$conn->close();
?>
