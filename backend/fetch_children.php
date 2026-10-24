<?php
include 'db_connect.php';
session_start();

header('Content-Type: application/json; charset=UTF-8');

if (!isset($_SESSION['ParentID'])) {
    echo json_encode(["success" => false, "message" => "Unauthorized: No active session found"]);
    exit();
}

$parentID = $_SESSION['ParentID'];

// Log ParentID for debugging
error_log("Fetching children for ParentID: " . $parentID);

// SQL query to fetch children details
$sql = "
    SELECT child.ChildID, child.Name, icons.source AS Avatar
    FROM child
    INNER JOIN icons ON child.IconID = icons.IconID
    WHERE child.ParentID = ?
";

$stmt = $conn->prepare($sql);
if (!$stmt) {
    error_log("SQL Error: " . $conn->error);
    echo json_encode(["success" => false, "message" => "Failed to prepare SQL query"]);
    exit();
}

$stmt->bind_param("i", $parentID);
$stmt->execute();
$result = $stmt->get_result();

$children = [];
while ($row = $result->fetch_assoc()) {
    if (isset($row['Name']) && isset($row['Avatar'])) {
        $children[] = [
            'ChildID' => $row['ChildID'], // Ensure ChildID is included
            'Name' => $row['Name'],
            'Avatar' => $row['Avatar']
        ];
    } else {
        error_log("Invalid row data: " . json_encode($row));
    }
}

if (!empty($children)) {
    echo json_encode(["success" => true, "children" => $children], JSON_UNESCAPED_UNICODE);
} else {
    error_log("No children found for ParentID: " . $parentID);
    echo json_encode(["success" => false, "message" => "No children found for ParentID: $parentID"]);
}

$stmt->close();
$conn->close();
?>