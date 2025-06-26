using UnityEngine;
using System.Collections.Generic;

namespace CustomSystem.Debug
{
    public class DebugConsoleCommands
    {
        private string commandInput = "";
        private int consoleHeight = 200;
        private Vector2 scrollPosition;
        private List<string> commandHistory = new List<string>();

        public void DrawConsole(DebugController controller)
        {
            GUILayout.BeginArea(new Rect(0, Screen.height - consoleHeight - 30, Screen.width, 30), GUI.skin.box);
            GUILayout.BeginHorizontal();

            // Align left buttons
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Close Console"))
            {
                controller.ToggleConsole(DebugController.ConsoleType.None);
            }
            if (GUILayout.Button("Switch to Logs Console"))
            {
                controller.SwitchConsole();
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            // Spacer to push the other buttons to the right
            GUILayout.FlexibleSpace();

            GUILayout.EndHorizontal();
            GUILayout.EndArea();

            // Main console area
            Rect consoleRect = new Rect(0, Screen.height - consoleHeight, Screen.width, consoleHeight - 30); // Adjusted height to leave space for input field
            GUILayout.BeginArea(consoleRect, GUI.skin.box);
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(Screen.width), GUILayout.Height(consoleHeight - 30));

            // Display the command history
            foreach (string command in commandHistory)
            {
                GUILayout.Label(command);
            }

            GUILayout.EndScrollView();
            GUILayout.EndArea();

            // Command input area
            GUILayout.BeginArea(new Rect(0, Screen.height - 30, Screen.width, 30), GUI.skin.box);
            GUILayout.BeginHorizontal();
            commandInput = GUILayout.TextField(commandInput, GUILayout.ExpandWidth(true));

            // Execute command when 'Enter' is pressed
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
            {
                ExecuteCommand(commandInput);
                commandInput = "";
            }

            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private void ExecuteCommand(string command)
        {
            if (!string.IsNullOrEmpty(command))
            {
                // Add command to history
                commandHistory.Add(command);

                // Scroll to the bottom to show the latest command
                scrollPosition.y = Mathf.Infinity;

                // Here you can add logic to actually execute the command
            }
        }
    }
}
