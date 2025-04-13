using System;
using UnityEngine;

namespace PluggableAI {
    [Serializable]
    public class PluggableTransition {
        [TextArea]
        public string comment;
        public BaseStateSO nextState;
        public PluggableDecision decision;
    }
}