<?php
// get_child_details.php
include 'db_connect.php';
header('Content-Type: application/json; charset=UTF-8');

// Make sure we have ChildID in the query string
if (!isset($_GET['ChildID'])) {
    echo json_encode(["success" => false, "message" => "Missing ChildID"]);
    exit();
}

$childID = intval($_GET['ChildID']);

// Example query�add more fields as needed
$sql = "
    SELECT c.ChildID, c.Name, c.Age, c.Coins, icons.source AS Avatar
    FROM child c
    LEFT JOIN icons ON c.IconID = icons.IconID
    WHERE c.ChildID = ?
";

$stmt = $conn->prepare($sql);
$stmt->bind_param('i', $childID);
$stmt->execute();
$result = $stmt->get_result();

if ($result->num_rows > 0) {
    $child = $result->fetch_assoc(); // single record
    echo json_encode([
        "success" => true,
        "child" => $child
    ], JSON_UNESCAPED_UNICODE);
} else {
    echo json_encode([
        "success" => false,
        "message" => "Child not found"
    ]);
}

$stmt->close();
$conn->close();
?>
