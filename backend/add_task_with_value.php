<?php
// Include the database connection
include('db_connect.php');

// Check for database connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

// Function to add the task with value
function addTaskWithValue($taskName, $valueName) {
    global $conn;
    
    // Insert the valueName into the batalvalues table
    $insertValueQuery = "INSERT INTO batalvalues (ValueName) VALUES ('$valueName')";
    if (mysqli_query($conn, $insertValueQuery)) {
        // Successfully inserted valueName, now get the last inserted ValueID
        $valueID = mysqli_insert_id($conn);
        
        // Optionally, you can also insert into other task-related tables if needed.
        // Insert the task into your other table (assuming there is one)
        $insertTaskQuery = "INSERT INTO task (TaskName, ValueID) VALUES ('$taskName', '$valueID')";
        if (mysqli_query($conn, $insertTaskQuery)) {
            return $valueID; // Return the ID of the inserted value
        } else {
            return "Error inserting task: " . mysqli_error($conn);
        }
    } else {
        return "Error inserting value: " . mysqli_error($conn);
    }
}

// Extract task and value name from POST request
$taskName = $_POST['taskName'];
$valueName = $_POST['valueName'];

// Add the task with value to the database
$result = addTaskWithValue($taskName, $valueName);

echo $result;

// Close the database connection
$conn->close();
?>
