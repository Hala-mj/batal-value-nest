<?php
header('Content-Type: application/json');

// Include the database connection file
include('db_connect.php');

// Ensure the connection is successful
if (!$conn) {
    echo json_encode(['success' => false, 'message' => 'Failed to connect to the database']);
    exit();
}

// Retrieve the child ID from the POST request
$childID = isset($_POST['childID']) ? intval($_POST['childID']) : 0;

if ($childID > 0) {
    // Prepare the SQL query
    $query = "SELECT Name, Age, Coins, IconID FROM child WHERE ChildID = ?";
    $stmt = $conn->prepare($query);

    if ($stmt) {
        $stmt->bind_param("i", $childID);
        $stmt->execute();
        $result = $stmt->get_result();

        if ($result->num_rows > 0) {
            $child = $result->fetch_assoc();
            echo json_encode([
                'success' => true,
                'child' => [
                    'Name' => $child['Name'],
                    'Age' => $child['Age'],
                    'Coins' => $child['Coins'],
                    'IconPath' => "/BVN/images/avatars/" . $child['IconID'] . ".png"
                ]
            ]);
        } else {
            echo json_encode(['success' => false, 'message' => 'Child not found']);
        }

        $stmt->close();
    } else {
        echo json_encode(['success' => false, 'message' => 'Failed to prepare the SQL statement']);
    }
} else {
    echo json_encode(['success' => false, 'message' => 'Invalid child ID']);
}

// Close the database connection
$conn->close();
?>
