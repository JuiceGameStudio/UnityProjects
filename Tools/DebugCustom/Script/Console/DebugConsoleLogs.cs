using UnityEngine;
using System.Collections.Generic;

namespace CustomSystem.Debug
{
    public class DebugConsoleLogs
    {
        private Dictionary<string, int> logMessages = new Dictionary<string, int>();
        private List<(string, LogType, string)> logOrder = new List<(string, LogType, string)>();
        private Dictionary<string, int> originalIndices = new Dictionary<string, int>();
        private Vector2 scrollPosition;
        private int consoleHeight = 200;
        private bool collapse = false;
        private bool showLog = true;
        private bool showWarning = true;
        private bool showError = true;

        public void HandleLog(string logString, string stackTrace, LogType type)
        {
            string timeStamp = System.DateTime.Now.ToString("HH:mm:ss");

            if (collapse && logMessages.ContainsKey(logString))
            {
                logMessages[logString]++;
            }
            else
            {
                logMessages[logString] = 1;
                int newIndex = logOrder.Count;
                logOrder.Add((logString, type, timeStamp));
                originalIndices[logString] = newIndex;
            }
        }

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
            if (GUILayout.Button("Switch to Commands Console"))
            {
                controller.SwitchConsole();
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            // Spacer to push the other buttons to the right
            GUILayout.FlexibleSpace();

            // Align right buttons
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Clear Logs"))
            {
                ClearLogs();
            }

            if (GUILayout.Button(collapse ? "Disable Collapse" : "Enable Collapse"))
            {
                collapse = !collapse;
                if (collapse)
                {
                    CollapseMessages();
                }
            }
            if (GUILayout.Button(showLog ? "Hide Log" : "Show Log"))
            {
                showLog = !showLog;
            }
            if (GUILayout.Button(showWarning ? "Hide Warning" : "Show Warning"))
            {
                showWarning = !showWarning;
            }
            if (GUILayout.Button(showError ? "Hide Error" : "Show Error"))
            {
                showError = !showError;
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
            GUILayout.EndArea();

            Rect consoleRect = new Rect(0, Screen.height - consoleHeight, Screen.width, consoleHeight);
            GUILayout.BeginArea(consoleRect, GUI.skin.box);
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(Screen.width), GUILayout.Height(consoleHeight));

            foreach (var logEntry in logOrder)
            {
                string message = logEntry.Item1;
                LogType type = logEntry.Item2;
                string timeStamp = logEntry.Item3;

                if ((type == LogType.Log && !showLog) ||
                    (type == LogType.Warning && !showWarning) ||
                    (type == LogType.Error && !showError))
                {
                    continue;
                }

                int count = logMessages[message];
                if (collapse && count > 1)
                {
                    GUILayout.Label($"[{timeStamp}] {message} (x{count})");
                }
                else
                {
                    GUILayout.Label($"[{timeStamp}] {message}");
                }
            }

            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        private void CollapseMessages()
        {
            var newLogMessages = new Dictionary<string, int>();
            foreach (var logEntry in logOrder)
            {
                string message = logEntry.Item1;
                if (newLogMessages.ContainsKey(message))
                {
                    newLogMessages[message]++;
                }
                else
                {
                    newLogMessages[message] = 1;
                }
            }
            logMessages = newLogMessages;
        }

        private void ClearLogs()
        {
            logMessages.Clear();
            logOrder.Clear();
            originalIndices.Clear();
        }
    }
}
