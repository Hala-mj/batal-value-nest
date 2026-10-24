<?php
// Database connection
include('db_connect.php');

if ($_SERVER["REQUEST_METHOD"] == "POST") {
    // Get task data from Unity
    $taskName = $_POST['taskName'];
    $valueID = $_POST['valueID']; // Assuming this is passed as part of the form

    // Prepare SQL query
    $sql = "INSERT INTO Task (TaskName, ValueID) VALUES (?, ?)";
    if ($stmt = $conn->prepare($sql)) {
        $stmt->bind_param("si", $taskName, $valueID);
        $stmt->execute();
        echo "Task added successfully!";
        $stmt->close();
    } else {
        echo "Error: " . $conn->error;
    }

    $conn->close();
}
?>
