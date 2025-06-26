using UnityEngine;

public abstract class Node : ScriptableObject
{
    public enum State
    {
        RUNNING,
        FAILURE,
        SUCCESS
    }

    [HideInInspector] public State state = State.RUNNING;
    [HideInInspector] public bool started = false;
    [HideInInspector] public string guid;
    [HideInInspector] public Vector2 position;
    [HideInInspector] public Blackboard blackboard;
    [TextArea] public string description;

    protected abstract void OnStart();
    protected abstract void OnStop();
    protected abstract State OnUpdate();

    public State Update()
    {
        if (!started)
        {
            OnStart();
            started = true;
        }

        state = OnUpdate();

        if (state == State.FAILURE || state == State.SUCCESS)
        {
            OnStop();
            started = false;
        }

        return state;
    }

    public virtual Node Clone()
    {
        return Instantiate(this);
    }

    public void Abort()
    {
        BehaviourTree.Traverse(this, (node) =>
        {
            node.started = false;
            node.state = State.RUNNING;
            node.OnStop();
        });
    }
}
