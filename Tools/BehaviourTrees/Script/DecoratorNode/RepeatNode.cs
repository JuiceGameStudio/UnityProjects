using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatNode : DecoratorNode
{
    public bool restartOnSuccess = true;
    public bool restartOnFailure = false;

    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        switch (child.Update())
        {
            case State.RUNNING:
                break;
            case State.FAILURE:
                if (restartOnFailure)
                {
                    return State.RUNNING;
                }
                else
                {
                    return State.FAILURE;
                }
            case State.SUCCESS:
                if (restartOnSuccess)
                {
                    return State.RUNNING;
                }
                else
                {
                    return State.SUCCESS;
                }
        }
        return State.RUNNING;
    }
}
