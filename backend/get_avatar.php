<?php
require 'db_connect.php';

if (isset($_GET['avatarID'])) {
    $avatarID = intval($_GET['avatarID']);
    $query = "SELECT Source FROM icons WHERE IconID = ?";
    $stmt = $conn->prepare($query);
    $stmt->bind_param('i', $avatarID);
    $stmt->execute();
    $stmt->bind_result($source);
    $stmt->fetch();

    if ($source) {
        echo json_encode(['success' => true, 'source' => $source]);
    } else {
        echo json_encode(['success' => false, 'message' => 'Avatar not found']);
    }
} else {
    echo json_encode(['success' => false, 'message' => 'No avatar selected']);
}

$conn->close();
?>
