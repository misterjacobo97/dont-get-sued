using System;
using R3;
using UnityEngine;

[CreateAssetMenu (fileName ="GameStatsSO", menuName ="Systems/GameStatsSO")]
public class GameStatsSO : ScriptableObject {

    public FloatReference gameTimeLeft;
    public FloatReference customerSatisfaction;
    public FloatReference managementSatisfaction;

    public BoolReference suedStatus;

    // public IDisposable[] disposables;

    // private void OnEnable (){
    //     if (UnityEditor.EditorApplication.isPlaying == false) return;

    //     disposables = new IDisposable[]{};

    //     managementSatisfaction.GetReactiveValue?.AsObservable().Subscribe(num =>{
    //         if (num <= 0) suedStatus.SetReactiveValue(true);
    //     }).AddTo(disposables);

    //     customerSatisfaction.GetReactiveValue?.AsObservable().Subscribe(num =>{
    //         if (num <= 0) suedStatus.SetReactiveValue(true);
    //     }).AddTo(disposables);

        

    // }

    // private void OnDisable (){
    //     Disposable.Dispose(disposables);
    // }
}
