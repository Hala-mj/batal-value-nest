<?php
header('Content-Type: application/json');
include('db_connect.php');

if (!isset($_POST["ChildID"])) {
    echo json_encode(["success" => false, "message" => "ChildID missing in request"]);
    exit();
}

$childID = intval($_POST["ChildID"]);

$query = "
    SELECT dailytasks.TaskID, task.TaskName, dailytasks.TaskState AS IsCompleted
    FROM dailytasks
    LEFT JOIN scheduled ON dailytasks.ScheduleID = scheduled.ScheduledID
    LEFT JOIN task ON scheduled.TaskID = task.TaskID
    WHERE scheduled.ChilledID = ?
";

$stmt = $conn->prepare($query);
$stmt->bind_param("i", $childID);
$stmt->execute();
$result = $stmt->get_result();

$tasks = [];
while ($row = $result->fetch_assoc()) {
    $tasks[] = [
        "TaskID" => (int)$row["TaskID"],
        "TaskName" => $row["TaskName"],
        "IsCompleted" => $row["IsCompleted"]
    ];
}

$stmt->close();
$conn->close();

echo json_encode(["success" => true, "tasks" => $tasks, "message" => "Tasks retrieved successfully"]);
?>
