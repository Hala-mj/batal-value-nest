<?php
require 'db_connect.php'; // Use the database connection script

if ($_SERVER["REQUEST_METHOD"] == "POST") {
    $taskID = isset($_POST['taskID']) ? intval($_POST['taskID']) : null;
    $duration = isset($_POST['duration']) ? intval($_POST['duration']) : null;
    $deadline = isset($_POST['deadline']) ? $_POST['deadline'] : null;

    if ($taskID === null || $duration === null || empty($deadline)) {
        die("Error: Missing taskID, duration, or deadline.");
    }

    error_log("Received TaskID: $taskID");
    error_log("Received Duration: $duration");
    error_log("Received Deadline: $deadline");

    $stmt = $conn->prepare("INSERT INTO scheduled (TaskID, Duration, Deadline) VALUES (?, ?, ?)");
    if (!$stmt) {
        die("Error preparing statement: " . $conn->error);
    }

    $stmt->bind_param("iis", $taskID, $duration, $deadline);

    if ($stmt->execute()) {
        echo "Schedule saved successfully.";
    } else {
        echo "Error: " . $stmt->error;
    }

    $stmt->close();
    $conn->close();
} else {
    echo "Error: Invalid request method. Received method: " . $_SERVER["REQUEST_METHOD"];
}
?>
