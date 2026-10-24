<?php
require 'db_connect.php';

if (isset($_GET['chilledID'])) {
    $chilledID = intval($_GET['chilledID']);

    // Fetch child data based on ChilledID
    $query = "SELECT Name, Age, IconID, ParentID, Coins FROM child WHERE ChildID = ?";
    $stmt = $conn->prepare($query);
    $stmt->bind_param('i', $chilledID);
    $stmt->execute();
    $stmt->bind_result($name, $age, $iconID, $parentID, $coins);
    $stmt->fetch();

    // Return child data if found
    if ($name) {
        echo json_encode([
            'success' => true,
            'data' => [
                'Name' => $name,
                'Age' => $age,
                'IconID' => $iconID,
                'ParentID' => $parentID,
                'Coins' => $coins
            ]
        ]);
    } else {
        echo json_encode(['success' => false, 'message' => 'Child not found']);
    }
} else {
    echo json_encode(['success' => false, 'message' => 'No child selected']);
}

$conn->close();
?>
