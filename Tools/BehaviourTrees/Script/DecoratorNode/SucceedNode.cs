using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SucceedNode : DecoratorNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        var state = child.Update();
        if (state == State.FAILURE)
        {
            return State.SUCCESS;
        }
        return state;
    }
}

