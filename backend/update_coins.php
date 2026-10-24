<?php
require 'db_connect.php';

if (isset($_POST['chilledID']) && isset($_POST['coins'])) {
    $chilledID = intval($_POST['chilledID']);
    $coins = intval($_POST['coins']);

    // Update coins for the child
    $query = "UPDATE child SET Coins = ? WHERE ChildID = ?";
    $stmt = $conn->prepare($query);
    $stmt->bind_param('ii', $coins, $chilledID);

    if ($stmt->execute()) {
        echo json_encode(['success' => true, 'message' => 'Coins updated successfully']);
    } else {
        echo json_encode(['success' => false, 'message' => 'Failed to update coins']);
    }
} else {
    echo json_encode(['success' => false, 'message' => 'Invalid input']);
}

$conn->close();
?>
