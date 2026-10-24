<?php
session_start();

// Destroy all session data
session_unset();
session_destroy();

header('Content-Type: application/json; charset=UTF-8');

// Respond to the client
echo json_encode(["success" => true, "message" => "User logged out successfully"]);
exit();
?>
