using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeoutNode : DecoratorNode
{
    public float duration = 1.0f;
    float startTime;

    protected override void OnStart()
    {
        startTime = Time.time;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (Time.time - startTime > duration)
        {
            return State.FAILURE;
        }

        return child.Update();
    }
}
