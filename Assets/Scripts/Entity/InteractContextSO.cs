using R3;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName ="New InteractContext", menuName = "Entities/InteractContext")]
public class InteractContextSO : ScriptableObject {
    public SerializableReactiveProperty<Transform> selectedInteractableObject;

    private void OnEnable() {
        SceneManager.activeSceneChanged += (a, b) => {
            selectedInteractableObject = new();
        
        };
    }

    private void OnDestroy() {
        selectedInteractableObject?.Dispose();
    }
}
