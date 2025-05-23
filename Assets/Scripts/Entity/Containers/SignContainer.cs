using System.Collections.Generic;
using R3;
using TMPro;
using UnityEngine;

public class SignContainer : MonoBehaviour, I_Interactable {
    [Header("Refs")]
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private TextMeshProUGUI _maxText;
    [SerializeField] private TextMeshProUGUI _currentText;
    [SerializeField] private List<Transform> _targets;
    private I_Interactable interactableRef;

    [Header("context objects")]
    [SerializeField] private InteractContextSO _playerContext;

    [Header("Params")]
    [SerializeField] private HoldableItem _signPrefab;
    [SerializeField] private int _maxSignCount = 3;

    private int _currentSignsCount = 3;
    private int CurrentSignCount {
        get { return _currentSignsCount; }
        set {
            if (_currentSignsCount >= 3 && value > 0) _currentSignsCount = 3;
            else if (_currentSignsCount <= 0 && value < 0) _currentSignsCount = 0;
            // has to set variable like "currentSignCount = 1 or -1"
            else _currentSignsCount += value;
        }
    }
    private void Awake() {
        interactableRef = GetComponent<I_Interactable>();
    }

    private void Start() {
        //PlayerInteract.Instance.OnSelectedInteractableChanged += OnSelectedInteractableChanged;
        UpdateUI();

        _playerContext.selectedInteractableObject.AsObservable().Subscribe((item) => {
            if (item != null && item.GetComponent<I_Interactable>() == interactableRef) {
                SetSelected();
            }
            else SetUnselected();
        }).AddTo(this);
    }

    //private void OnSelectedInteractableChanged(object sender, PlayerInteract.OnSelectedInteractableChangedEventArgs e) {
    //    if (e.selectedInteractable == (I_Interactable)this) {
    //        SetSelected();
    //    }
    //    else {
    //        SetUnselected();
    //    }
    //}

    public void SetSelected() {
        _sprite.color = Color.white;
    }

    public void SetUnselected() {
        _sprite.color = Color.red;
    }

    public void Interact(Object caller) {
        if (caller is not PlayerInteract) {
            return;
        }

        if ((caller as PlayerInteract).HasItemHolder() == false) {
            return;
        }

        I_ItemHolder holder = (caller as PlayerInteract).GetItemHolder();

        if (holder.HasItem() && CurrentSignCount < _maxSignCount) {
            // if holder has sign, destroy and add 1 to counter
            Destroy(holder.GetHeldItem().gameObject);
            CurrentSignCount = 1;
            UpdateUI();
        }

        else if (holder.HasItem() == false && CurrentSignCount > 0) {
            CurrentSignCount = -1;
            UpdateUI();

            GameObject.Instantiate(_signPrefab.transform).GetComponent<HoldableItem>().ChangeParent(holder);
        }
    }

    private void UpdateUI() {
        _maxText.text = _maxSignCount.ToString();
        _currentText.text = CurrentSignCount.ToString();

        for (int i = 0; i < _targets.Count; i++) {

            if (i + 1 <= _currentSignsCount) {
                _targets[i].GetComponent<SpriteRenderer>().enabled = true;
            }
            else _targets[i].GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}
