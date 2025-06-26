using UnityEngine;
using CustomSystem.Debug;

public class ExampleDebug : MonoBehaviour, IDebugSystem
{
    public bool localDebugEnabled { get; set; }

    public bool globaldebug = false;

    void Update()
    {
        DebugSystem.Log("Test", Color.blue);
        DebugSystem.EnableGlobalDebug(globaldebug);
    }
}
