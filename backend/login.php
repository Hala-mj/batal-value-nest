<?php
include 'db_connect.php';
session_start();

header('Content-Type: application/json; charset=UTF-8');

if ($_SERVER["REQUEST_METHOD"] == "POST") {
    $email = trim($_POST['email']);
    $password = trim($_POST['password']);

    if (empty($email) || empty($password)) {
        echo json_encode(["success" => false, "message" => "Email and password are required"]);
        exit();
    }

    // Fetch user data
    $stmt = $conn->prepare("SELECT ParentID, Password FROM users WHERE Email = ?");
    $stmt->bind_param("s", $email);
    $stmt->execute();
    $result = $stmt->get_result();

    if ($result->num_rows == 0) {
        echo json_encode(["success" => false, "message" => "Invalid email or password"]);
        exit();
    }

    $user = $result->fetch_assoc();

    // Verify password
    if (!password_verify($password, $user['Password'])) {
        echo json_encode(["success" => false, "message" => "Invalid email or password"]);
        exit();
    }

    // Set session
    session_unset();
    session_destroy();
    session_start();
    $_SESSION['ParentID'] = $user['ParentID'];
    $_SESSION['email'] = $email;

    // Log for debugging
    error_log("Login successful. Email: $email, ParentID: " . $user['ParentID']);

    echo json_encode(["success" => true, "ParentID" => $user['ParentID'], "message" => "Login successful"]);
}

$conn->close();
?>
