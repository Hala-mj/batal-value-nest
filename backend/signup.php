<?php
include 'db_connect.php';
session_start();

header('Content-Type: application/json; charset=UTF-8');

if ($_SERVER["REQUEST_METHOD"] == "POST") {
    $fname = trim($_POST['fname']);
    $lname = trim($_POST['lname']);
    $phone = trim($_POST['phone']);
    $email = trim($_POST['email']);
    $password = trim($_POST['password']);
    $classification = trim($_POST['Classification']);

    if (empty($fname) || empty($lname) || empty($phone) || empty($email) || empty($password) || empty($classification)) {
        echo json_encode(["success" => false, "message" => "Missing required fields"]);
        exit();
    }

    // Check if the email already exists
    $stmt = $conn->prepare("SELECT ParentID FROM users WHERE Email = ?");
    $stmt->bind_param("s", $email);
    $stmt->execute();
    $result = $stmt->get_result();

    if ($result->num_rows > 0) {
        echo json_encode(["success" => false, "message" => "Email already exists"]);
        exit();
    }

    // Hash the password and insert the new user
    $hashedPassword = password_hash($password, PASSWORD_DEFAULT);
    $stmt = $conn->prepare("INSERT INTO users (Fname, Lname, PhoneNo, Email, Password, Classification) VALUES (?, ?, ?, ?, ?, ?)");
    $stmt->bind_param("ssssss", $fname, $lname, $phone, $email, $hashedPassword, $classification);

    if ($stmt->execute()) {
        $parentId = $stmt->insert_id;

        // Insert a default child for the new parent
        $childStmt = $conn->prepare("INSERT INTO child (Name, ParentID, Age, IconID, Coins) VALUES (?, ?, ?, ?, ?)");
        $defaultChildName = "Default Child";
        $defaultAge = 6;
        $defaultIconID = 1;
        $defaultCoins = 100;
        $childStmt->bind_param("siiii", $defaultChildName, $parentId, $defaultAge, $defaultIconID, $defaultCoins);
        $childStmt->execute();
        $childStmt->close();

        // Set session for the new user
        session_unset();
        session_destroy();
        session_start();
        $_SESSION['ParentID'] = $parentId;
        $_SESSION['email'] = $email;

        // Log ParentID for debugging
        error_log("Signup successful. New ParentID: " . $parentId);

        echo json_encode(["success" => true, "ParentID" => $parentId, "message" => "Account created successfully"]);
    } else {
        error_log("Database Error: " . $stmt->error);
        echo json_encode(["success" => false, "message" => "Database error"]);
    }

    $stmt->close();
}
$conn->close();
?>
