<?php
date_default_timezone_set('Asia/Kolkata');
if ($_SERVER['REQUEST_METHOD'] == 'POST') {
    $data = json_decode(file_get_contents('php://input'), true);

    if (isset($data['image'])) {
        $base64String = $data['image'];
        $imageData = base64_decode($base64String);

        // Get the current date for folder creation (Year, Month, Day)
        $currentYear = date('Y');    // Year (e.g., 2025)
        $currentMonth = date('m');   // Month (e.g., 01)
        $currentDay = date('d');     // Day (e.g., 19)

        // Create the directory path for Year/Month/Day
        $directoryPath = 'upload/' . $currentYear . '/' . $currentMonth . '/' . $currentDay;

        // Check if the directory exists, if not, create it
        if (!file_exists($directoryPath)) {
            mkdir($directoryPath, 0777, true);  // Create directory with recursive permissions
        }

        // Save the screenshot with a unique name (timestamp-based)
        $filePath = $directoryPath . '/screenshot_' . date('H-i-s',time()) . '.png';
        file_put_contents($filePath, $imageData);

        // Return success response with the file path
        echo json_encode(['status' => 'success', 'path' => $filePath]);
    } else {
        echo json_encode(['status' => 'error', 'message' => 'No image provided.']);
    }
} else {
    echo json_encode(['status' => 'error', 'message' => 'Invalid request method.']);
}
