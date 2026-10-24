<?php
header('Content-Type: application/json');
include('db_connect.php');

if (!isset($_POST["TaskID"]) || !isset($_POST["State"])) {
    echo json_encode(["success" => false, "message" => "TaskID or State missing in request"]);
    exit();
}

$taskID = intval($_POST["TaskID"]);
$state = $_POST["State"];

$query = "UPDATE dailytasks SET TaskState = ? WHERE TaskID = ?";
$stmt = $conn->prepare($query);
$stmt->bind_param("si", $state, $taskID);

if ($stmt->execute()) {
    echo json_encode(["success" => true, "message" => "Task state updated successfully"]);
} else {
    echo json_encode(["success" => false, "message" => "Failed to update task state"]);
}

$stmt->close();
$conn->close();
?>
