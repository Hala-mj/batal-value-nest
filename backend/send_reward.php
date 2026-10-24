<?php
include 'db_connect.php';
header('Content-Type: application/json; charset=UTF-8');

// 1) Check required POST fields
if (!isset($_POST['ChilledID'], $_POST['Title'], $_POST['Reason'], $_POST['IconID'])) {
    echo json_encode([
        "success" => false,
        "message" => "Missing one or more required fields!"
    ]);
    exit();
}

// 2) Extract them
$chilledID = intval($_POST['ChilledID']);
$title     = $_POST['Title'];
$reason    = $_POST['Reason'];
$iconID    = intval($_POST['IconID']);

// 3) Prepare the INSERT
// We'll insert ValueID as NULL
$query = "
    INSERT INTO reward (ChilledID, Title, Reason, IconID, ValueID)
    VALUES (?, ?, ?, ?, NULL)
";
$stmt = $conn->prepare($query);
if (!$stmt) {
    echo json_encode([
        "success" => false,
        "message" => "SQL Prepare failed: " . $conn->error
    ]);
    exit();
}

$stmt->bind_param('issi', $chilledID, $title, $reason, $iconID);
$execOk = $stmt->execute();

if ($execOk) {
    // Return the new RewardID
    $newRewardID = $stmt->insert_id;
    echo json_encode([
        "success" => true,
        "message" => "Reward inserted successfully!",
        "RewardID" => $newRewardID
    ]);
} else {
    echo json_encode([
        "success" => false,
        "message" => "Insert error: " . $stmt->error
    ]);
}

$stmt->close();
$conn->close();
?>
