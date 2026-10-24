<?php
include 'db_connect.php'; // your DB connection
header('Content-Type: application/json; charset=UTF-8');

// Make sure we have all required POST parameters
if (!isset($_POST['ChildID'], $_POST['Title'], $_POST['TextContent'], $_POST['DOR'])) {
    echo json_encode([
        "success" => false,
        "message" => "Missing one or more required fields!"
    ]);
    exit();
}

// Extract data
$childID = intval($_POST['ChildID']);
$title = $_POST['Title'];
$textContent = $_POST['TextContent'];
$dor = $_POST['DOR']; // e.g. "2023-10-12" or "2025-01-01"

// Prepare the INSERT (using ChildID, not ChilledID)
$stmt = $conn->prepare("
    INSERT INTO messages (ChildID, Title, TextContent, DOR) 
    VALUES (?, ?, ?, ?)
");
if (!$stmt) {
    echo json_encode([
        "success" => false, 
        "message" => "Prepare failed: " . $conn->error
    ]);
    exit();
}

// Bind and execute
$stmt->bind_param("isss", $childID, $title, $textContent, $dor);
$success = $stmt->execute();

if ($success) {
    // Insert succeeded
    $newID = $stmt->insert_id; // The new message's ID
    echo json_encode([
        "success" => true, 
        "message" => "Message inserted successfully",
        "MessageID" => $newID
    ]);
} else {
    // Insertion error
    echo json_encode([
        "success" => false,
        "message" => "Insert failed: " . $stmt->error
    ]);
}

$stmt->close();
$conn->close();
?>
