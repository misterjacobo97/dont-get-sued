using UnityEngine;

[CreateAssetMenu (fileName ="GameStatsSO", menuName ="Systems/GameStatsSO")]
public class GameStatsSO : ScriptableObject {
    public BoolReference suedStatus;
    public FloatReference gameTimeLeft;
    public FloatReference customerSatisfaction;
    public FloatReference managementSatisfaction;
    public FloatReference levelScore;

}
