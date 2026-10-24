<?php
// Include database connection
include 'db_connect.php';

header("Content-Type: application/json; charset=UTF-8");

// Check if childID is passed as a POST parameter
if (isset($_POST['childID'])) {
    $childID = $_POST['childID'];

    // Query to fetch tasks for the specified child
    $query = "SELECT dt.TaskID, t.TaskName, dt.TaskState 
              FROM dailytasks dt
              INNER JOIN task t ON dt.TaskID = t.TaskID
              WHERE dt.ChildID = ?";

    // Prepare the statement
    if ($stmt = $conn->prepare($query)) {
        $stmt->bind_param("i", $childID);
        $stmt->execute();

        $result = $stmt->get_result();
        $tasks = array();

        // Fetch all tasks and add them to the array
        while ($row = $result->fetch_assoc()) {
            $tasks[] = array(
                "id" => $row['TaskID'],
                "description" => $row['TaskName'],
                "isDone" => ($row['TaskState'] == 'Option1') // Assuming 'Option1' represents a completed state
            );
        }

        // Output the tasks as JSON
        echo json_encode($tasks);
    } else {
        echo json_encode(array("error" => "Failed to prepare the query."));
    }
} else {
    echo json_encode(array("error" => "Missing childID parameter."));
}
?>
