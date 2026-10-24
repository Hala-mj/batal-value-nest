<?php
// Include database connection
include 'db_connect.php';

header("Content-Type: application/json; charset=UTF-8");  // Ensure UTF-8 encoding

// Query to fetch all values
$query = "SELECT ValueID, ValueName FROM batalvalues";

if ($result = $conn->query($query)) {
    $values = [];
    // Fetch all values and store in an array
    while ($row = $result->fetch_assoc()) {
        // Ensure ValueID is an integer
        $row['ValueID'] = (int) $row['ValueID'];
        $values[] = $row;
    }

    // Return the values as a JSON response wrapped in an object with the "values" key
    echo json_encode(["values" => $values], JSON_UNESCAPED_UNICODE);  // Ensure Arabic characters are not escaped
} else {
    echo json_encode(["error" => "Failed to fetch values."]);
}

// Close the database connection
$conn->close();
?>
