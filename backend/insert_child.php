<?php
require 'db_connect.php';

// Get data from Unity (using $_POST)
if ($_SERVER["REQUEST_METHOD"] == "POST") {
    // Sanitize and validate the data
    $name = isset($_POST["name"]) ? $_POST["name"] : '';
    $age = isset($_POST["age"]) ? $_POST["age"] : '';
    $avatarID = isset($_POST["avatarID"]) ? $_POST["avatarID"] : '';
    $parentID = isset($_POST["parentID"]) ? $_POST["parentID"] : 1; // Default to 1 for now
    $coins = isset($_POST["coins"]) ? $_POST["coins"] : 0; // Default to 0 coins

    // Validate input
    if (empty($name) || empty($age) || empty($avatarID)) {
        echo json_encode(['success' => false, 'message' => 'Missing required fields!']);
        exit;
    }

    // Prepare and execute the SQL query to insert the child's data
    $stmt = $conn->prepare("INSERT INTO child (Name, Age, IconID, ParentID, Coins) VALUES (?, ?, ?, ?, ?)");
    $stmt->bind_param("siiii", $name, $age, $avatarID, $parentID, $coins);

    if ($stmt->execute()) {
        echo json_encode(['success' => true, 'message' => 'Child added successfully']);
    } else {
        echo json_encode(['success' => false, 'message' => 'Failed to add child']);
    }

    $stmt->close();
} else {
    echo json_encode(['success' => false, 'message' => 'Invalid request method.']);
}

$conn->close();
?>
