using Tasks;
using UnityEngine;

public class BaseShelf : TaskHolder, I_Interactable {

    enum ShelfType {
        SINGLE_TASK,
        MULTI_TASK
    }

    [Header("Refs")]
    [SerializeField] private SpriteRenderer _sprite;

    [Header("Params")]
    [SerializeField] private ShelfType _shelfType = ShelfType.SINGLE_TASK;

    protected override void Start() {
        base.Start();
        PlayerInteract.Instance.OnSelectedInteractableChanged += OnSelectedInteractableChanged;

        TaskManager.Instance.AddTaskReceiver(this);
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
        Debug.Log("Interacted with");

        if (caller is TaskHolder) {
            if ((caller as TaskHolder).HasTaskObject() == false && HasTaskObject()) {
                Debug.Log("sending task to: " + caller.ToString());
                (caller as TaskHolder).SetTaskObject(GetTaskObject);
                ClearTaskObject();
            }
        }

    }
}
