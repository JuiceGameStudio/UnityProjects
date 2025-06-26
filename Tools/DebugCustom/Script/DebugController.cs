using UnityEngine;

namespace CustomSystem.Debug
{
    [AddComponentMenu("Custom System/Debug/Controller")]
    public class DebugController : MonoBehaviour
    {
        private DebugConsoleLogs debugConsoleLogs;
        private DebugConsoleCommands debugConsoleCommands;
        public KeyCode enabledGlobalDebug = KeyCode.F1;
        public KeyCode toggleLogConsoleKey = KeyCode.F2;
        public KeyCode toggleCommandConsoleKey = KeyCode.F3;

        public enum ConsoleType { None, Logs, Commands }
        private ConsoleType currentConsole = ConsoleType.None;

        void Awake()
        {
            debugConsoleLogs = new DebugConsoleLogs();
            debugConsoleCommands = new DebugConsoleCommands();
            Application.logMessageReceived += HandleLog;
        }

        void OnDestroy()
        {
            Application.logMessageReceived -= HandleLog;
        }

        void HandleLog(string logString, string stackTrace, LogType type)
        {
            if (currentConsole == ConsoleType.Logs)
            {
                debugConsoleLogs.HandleLog(logString, stackTrace, type);
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(toggleLogConsoleKey))
            {
                ToggleConsole(ConsoleType.Logs);
            }

            if (Input.GetKeyDown(toggleCommandConsoleKey))
            {
                ToggleConsole(ConsoleType.Commands);
            }

            if (Input.GetKeyDown(enabledGlobalDebug))
            {
                DebugSystem.EnableGlobalDebug(!DebugSystem.IsGlobalDebugEnabled());
            }
        }

        void OnGUI()
        {
            if (currentConsole == ConsoleType.Logs)
            {
                debugConsoleLogs.DrawConsole(this);
            }
            else if (currentConsole == ConsoleType.Commands)
            {
                debugConsoleCommands.DrawConsole(this);
            }
        }

        public void ToggleConsole(ConsoleType consoleType)
        {
            if (currentConsole == consoleType)
            {
                currentConsole = ConsoleType.None;
            }
            else
            {
                currentConsole = consoleType;
            }
        }

        public void SwitchConsole()
        {
            if (currentConsole == ConsoleType.Logs)
            {
                currentConsole = ConsoleType.Commands;
            }
            else if (currentConsole == ConsoleType.Commands)
            {
                currentConsole = ConsoleType.Logs;
            }
        }
    }
}

