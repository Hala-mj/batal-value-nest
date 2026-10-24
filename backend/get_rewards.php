<?php
// Include the database connection
require_once 'db_connect.php';

header('Content-Type: application/json');

// Get the child_id from the request
$childId = isset($_GET['child_id']) ? intval($_GET['child_id']) : 0;

// Validate the child_id
if ($childId <= 0) {
    echo json_encode(["error" => "Invalid child ID."]);
    exit;
}

// Query to fetch rewards for the given child ID
$sql = "SELECT title, description, icon_path FROM rewards WHERE child_id = ?";
$stmt = $conn->prepare($sql);

if ($stmt === false) {
    echo json_encode(["error" => "Failed to prepare the statement: " . $conn->error]);
    exit;
}

$stmt->bind_param("i", $childId);
$stmt->execute();
$result = $stmt->get_result();

$rewards = [];
while ($row = $result->fetch_assoc()) {
    $rewards[] = $row;
}

// Close the statement and connection
$stmt->close();
$conn->close();

// Return the results as JSON
echo json_encode($rewards);
?>
