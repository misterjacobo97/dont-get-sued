using UnityEngine;

[CreateAssetMenu(fileName = "Systems/UserInputChannel")]
public class UserInputChannelSO : ScriptableObject {
    public BoolReference freezeInput;
    public Vector2Reference moveInput;
    public Vector2Reference lastMoveDir;

    public BoolReference InteractInput;


}
