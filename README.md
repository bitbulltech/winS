# winS

**Windows Desktop Screenshot Capture Utility (C#)**

A lightweight Windows utility that captures system screenshots at regular intervals and sends them to a remote server via HTTP POST. Ideal for remote monitoring or automation scenarios.

---

##  Setup Instructions

### 1. Clone and Configure the Project

* Open the `.sln` file in **Visual Studio** (not VS Code).
* Open `Program.cs` and edit the following:

  * **Line 49**: Update the URL where screenshots will be sent. Example:

    ```csharp
    string serverUrl = "http://your-server-address/index.php";
    ```
  * Optional: Line number 49, Adjust the interval time in seconds (default: 300 seconds = 5 minutes).

---

##  Build the Application

* Build the project in **Release** mode.
* Locate the generated `.exe` file in the `bin/Release` folder.

---

##  Run at System Startup (Recommended)

### **Option 2: Register as a Scheduled Task (More Reliable)**

This ensures the utility runs silently in the background on every system boot.

#### Steps:

1. Press `Win + S`, search for **Task Scheduler**, and open it.
2. Click **Create Task** (not basic task) in the right pane.
3. **General tab**:

   * Name: `ScreenshotAgent`
   * Select **Run whether user is logged on or not**
   * Check **Run with highest privileges**
4. **Triggers tab**:

   * Click **New**
   * Set trigger: **At startup** or **At log on**
5. **Actions tab**:

   * Click **New**
   * Set **Action**: *Start a program*
   * Browse to the compiled `.exe` file
6. Click **OK** and enter admin credentials if prompted.

Now your screenshot utility will automatically run at boot, even if no user logs in.

---

## Notes

* Requires **.NET Framework** (usually pre-installed on Windows systems).
* Ensure your server can accept and process HTTP POST requests with image data.

Do you need professional support to setup solution or more modifications and add-on features?
Contact me by email contact@bitbulltech.com or whatsapp +91 98725 47685

