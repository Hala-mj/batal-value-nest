<?php
require 'db_connect.php';

if (isset($_POST['userID']) && isset($_POST['avatarID'])) {
    $userID = intval($_POST['userID']);
    $avatarID = intval($_POST['avatarID']);

    $query = "UPDATE Users SET AvatarID = ? WHERE ID = ?";
    $stmt = $conn->prepare($query);
    $stmt->bind_param('ii', $avatarID, $userID);

    if ($stmt->execute()) {
        echo json_encode(['success' => true, 'message' => 'Avatar updated successfully']);
    } else {
        echo json_encode(['success' => false, 'message' => 'Failed to update avatar']);
    }
} else {
    echo json_encode(['success' => false, 'message' => 'Invalid input']);
}

$conn->close();
?>
