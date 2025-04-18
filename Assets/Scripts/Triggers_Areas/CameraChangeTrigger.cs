using Unity.Cinemachine;
using UnityEngine;
using R3;
using R3.Triggers;

public class CameraAreaStayTrigger : MonoBehaviour {
    [SerializeField] private CinemachineCamera _newCam;
    [SerializeField] private string _playerLayerName;

    private void Awake() {
        this.OnTriggerEnter2DAsObservable().Subscribe(collider => {
            if (collider.gameObject.layer == LayerMask.NameToLayer(_playerLayerName)) {
                _newCam.enabled = true;
            }
        }).AddTo(this);

        this.OnTriggerExit2DAsObservable().Subscribe(collider => {
            if (collider.gameObject.layer == LayerMask.NameToLayer(_playerLayerName)) {
                _newCam.enabled = false;
            }
        }).AddTo(this);
    }
}
