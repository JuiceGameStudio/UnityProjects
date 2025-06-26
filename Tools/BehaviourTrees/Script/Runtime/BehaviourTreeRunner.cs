using UnityEngine;
using UnityEditor;

public class BehaviourTreeRunner : MonoBehaviour
{
    public BehaviourTree tree;

    void Start()
    {
        tree = tree.Clone();
        tree.BindBlackboard();
    }

    void Update()
    {
        tree.Update();
    }

    [ContextMenu("Open Behaviour Tree Editor")]
    void OpenBehaviourTree()
    {
        EditorApplication.ExecuteMenuItem("Window/BehaviourTreeEditor");
    }
}
