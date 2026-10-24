<?php
// Include the database connection
include('db_connect.php');

if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

// Function to save the schedule
function saveSchedule($taskID, $duration, $deadline) {
    global $conn;
    
    // Check if taskID exists in the 'task' table
    $taskCheckQuery = "SELECT * FROM task WHERE TaskID = '$taskID'";
    $result = mysqli_query($conn, $taskCheckQuery);

    if (mysqli_num_rows($result) > 0) {
        // TaskID exists, proceed to insert the schedule
        $query = "INSERT INTO scheduled (taskID, duration, deadline) VALUES ('$taskID', '$duration', '$deadline')";
        
        // Execute the query
        if (mysqli_query($conn, $query)) {
            return true;
        } else {
            return false;  // Query failed
        }
    } else {
        return false;  // TaskID does not exist
    }
}

// Extract data from the request
$taskID = $_POST['taskID'];
$duration = $_POST['duration'];
$deadline = $_POST['deadline'];

// Save the schedule
if (saveSchedule($taskID, $duration, $deadline)) {
    echo "Schedule saved successfully";
} else {
    echo "Error: Could not save schedule or invalid TaskID";
}

// Close the database connection
$conn->close();
?>
