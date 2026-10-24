<?php
include 'db_connect.php';
header('Content-Type: application/json; charset=UTF-8');

// 1. Check if ChildID is provided
if (!isset($_GET['ChildID'])) {
    echo json_encode(["success" => false, "message" => "ChildID is missing"]);
    exit();
}

$childID = intval($_GET['ChildID']);

// 2. Query the messages table
//    No date filtering, returns up to 8 messages by ascending MessageID
$query = "
    SELECT MessageID, Title, TextContent
    FROM messages
    WHERE ChildID = ?
    ORDER BY MessageID ASC
    LIMIT 8
";

$stmt = $conn->prepare($query);
$stmt->bind_param('i', $childID);
$stmt->execute();
$result = $stmt->get_result();

$messages = [];
while ($row = $result->fetch_assoc()) {
    $messages[] = $row;
}

// 3. Return JSON
if (count($messages) > 0) {
    echo json_encode(["success" => true, "messages" => $messages], JSON_UNESCAPED_UNICODE);
} else {
    echo json_encode(["success" => false, "message" => "No messages found"]);
}

$stmt->close();
$conn->close();
?>
