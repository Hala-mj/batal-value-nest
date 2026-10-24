<?php
// Include the database connection
include('db_connect.php');

if ($_SERVER["REQUEST_METHOD"] == "POST") {
    // Get the ValueName from the Unity form
    $valueName = $_POST['valueName'];

    // Prepare the SQL query
    $sql = "INSERT INTO batalvalues (ValueName) VALUES (?)";

    if ($stmt = $conn->prepare($sql)) {
        $stmt->bind_param("s", $valueName); // Bind the ValueName as a string
        if ($stmt->execute()) {
            echo "Value added successfully!";
        } else {
            echo "Error: " . $stmt->error;
        }
        $stmt->close();
    } else {
        echo "Error: " . $conn->error;
    }

    $conn->close();
}
?>
