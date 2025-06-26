using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class FailureNode : DecoratorNode {
        protected override void OnStart() {
        }

        protected override void OnStop() {
        }

        protected override State OnUpdate() {
            var state = child.Update();
            if (state == State.SUCCESS) {
                return State.FAILURE;
            }
            return state;
        }
    }
