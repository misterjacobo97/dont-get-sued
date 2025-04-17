using R3;
using R3.Triggers;
using UnityEngine;

public class NPCSlapDetectionArea : MonoBehaviour {
    

    [SerializeField] private string _slapAreaLayerName;
    [SerializeField] private float _slapStunTime = 2.0f;

    public ReactiveProperty<bool> IsSlapped { get; private set; }
    public ReactiveProperty<Vector2> SlapDir { get; private set; }
    
    private void Awake() {
        IsSlapped = new ReactiveProperty<bool>().AddTo(this);
        SlapDir = new ReactiveProperty<Vector2>(Vector2.zero).AddTo(this);

        this.OnTriggerEnter2DAsObservable().Subscribe(collider => {
            if (collider.gameObject.layer != LayerMask.NameToLayer(_slapAreaLayerName)) return;

            SlapDir.Value = (transform.position - collider.transform.position).normalized;

            StartSlapTimer();


        }).AddTo(this);
    }


    public async void StartSlapTimer() {
        if (IsSlapped.Value == true) return;

        IsSlapped.Value = true;

        await Awaitable.WaitForSecondsAsync(_slapStunTime);

        IsSlapped.Value = false;
    }

}
