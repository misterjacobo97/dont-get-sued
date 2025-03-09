using Tasks;
using UnityEngine;

public class Bin : TaskHolder, I_Interactable {
    [Header("Refs")]
    [SerializeField] private SpriteRenderer _sprite;

    protected override void Start() {
        base.Start();
        PlayerInteract.Instance.OnSelectedInteractableChanged += OnSelectedInteractableChanged;
    }

    private void OnSelectedInteractableChanged(object sender, PlayerInteract.OnSelectedInteractableChangedEventArgs e) {
        if (e.selectedInteractable == (I_Interactable)this) {
            SetSelected();
        }
        else {
            SetUnselected();
        }
    }

    public void SetSelected() {
        _sprite.color = Color.white;
    }

    public void SetUnselected() {
        _sprite.color = Color.red;
    }

    public void Interact(object caller) {
        if (caller is TaskHolder) {
            if ((caller as TaskHolder).HasTaskObject() && HasTaskObject() == false) {
                Debug.Log("receiving task from: " + caller.ToString());
                SetTaskObject((caller as TaskHolder).GetTaskObject);
                (caller as TaskHolder).ClearTaskObject();

                CompleteTask();

            }
        }

    }

}
