<?php
// Database connection settings are loaded from config.php (not committed).
// See config.example.php for the expected format.
$configFile = __DIR__ . '/config.php';
if (!file_exists($configFile)) {
    die("Missing backend/config.php. Copy config.example.php to config.php and set your database credentials.");
}
$config = require $configFile;

$servername = $config['db_host'];
$username   = $config['db_user'];
$password   = $config['db_pass'];
$dbname     = $config['db_name'];

$conn = new mysqli($servername, $username, $password, $dbname);

if ($conn->connect_error) {
    die("Database connection failed: " . $conn->connect_error);
}
$conn->set_charset('utf8mb4');

// Error reporting is controlled by the "debug" flag in config.php
if (!empty($config['debug'])) {
    error_reporting(E_ALL);
    ini_set('display_errors', 1);
} else {
    error_reporting(0);
    ini_set('display_errors', 0);
}
?>
