<?php
include 'db_connect.php';
session_start();

header('Content-Type: application/json; charset=UTF-8');

if (!isset($_SESSION['ParentID'])) {
    echo json_encode(["success" => false, "message" => "Unauthorized: No active session found"]);
    exit();
}

$parentID = $_SESSION['ParentID'];

// Log ParentID for debugging
error_log("Fetching user info for ParentID: " . $parentID);
error_log("SQL Query Executed: SELECT Fname, Lname, Email, PhoneNo, Classification FROM users WHERE ParentID = " . $parentID);

// SQL query to fetch the logged-in parent's info
$sql = "
    SELECT Fname, Lname, Email, PhoneNo, Classification
    FROM users
    WHERE ParentID = ?
";

$stmt = $conn->prepare($sql);
if (!$stmt) {
    error_log("SQL Error: " . $conn->error);
    echo json_encode(["success" => false, "message" => "Failed to prepare SQL query"]);
    exit();
}

$stmt->bind_param("i", $parentID);
$stmt->execute();
$result = $stmt->get_result();

$user = $result->fetch_assoc();

if ($user) {
    error_log("Fetched user data: " . json_encode($user)); // Log user data for debugging
    echo json_encode(["success" => true, "user" => $user], JSON_UNESCAPED_UNICODE);
} else {
    error_log("No user found for ParentID: " . $parentID);
    echo json_encode(["success" => false, "message" => "No user found for ParentID: $parentID"]);
}

$stmt->close();
$conn->close();
?>
