using UnityEngine;

namespace PluggableAI {

    public abstract class PluggableAction : ScriptableObject {
        public abstract void Act(StateController controller);
    }
}