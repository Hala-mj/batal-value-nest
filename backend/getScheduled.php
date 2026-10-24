<?php
include 'db_connect.php';

header('Content-Type: application/json; charset=UTF-8');

if ($_SERVER["REQUEST_METHOD"] == "GET" && isset($_GET['childID'])) {
    $childID = intval($_GET['childID']);

    // Query to fetch scheduled tasks for the given child.
    // "ChilledID" in the scheduled table stores the child's ID.
    $query = "SELECT s.ScheduledID, s.TaskID, s.Duration, s.Deadline, t.TaskName 
              FROM scheduled s
              INNER JOIN task t ON s.TaskID = t.TaskID
              WHERE s.ChilledID = ?";
    
    if ($stmt = $conn->prepare($query)) {
        $stmt->bind_param("i", $childID);
        $stmt->execute();
        $result = $stmt->get_result();
        $scheduledTasks = array();
        
        while ($row = $result->fetch_assoc()) {
            $scheduledTasks[] = array(
                "ScheduledID" => $row['ScheduledID'],
                "TaskID"      => $row['TaskID'],
                "TaskName"    => $row['TaskName'],
                "Duration"    => intval($row['Duration']),
                "Deadline"    => $row['Deadline']
            );
        }
        
        echo json_encode(array("success" => true, "scheduledTasks" => $scheduledTasks));
        $stmt->close();
        $conn->close();
    } else {
        echo json_encode(array("success" => false, "message" => "Failed to prepare SQL query: " . $conn->error));
    }
} else {
    echo json_encode(array("success" => false, "message" => "Missing childID parameter or invalid request method."));
}
?>
