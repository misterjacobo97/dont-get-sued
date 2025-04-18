using UnityEngine;

[CreateAssetMenu (fileName ="new AnyObjectGameEvent", menuName ="GameEvents/AnyObjectGameEvent")]
public class AnyObjectGameEventSO : GameEventSO<object> { }

[CreateAssetMenu (fileName ="new ScriptableObjectGameEvent", menuName ="GameEvents/ScriptableObjectGameEvent")]
public class ScriptableObjectGameEventSO : GameEventSO<ScriptableObject> { }

[CreateAssetMenu(menuName = "GameEvents/FloatGameEvent")]
public class FloatGameEventSO : GameEventSO<float> { }

[CreateAssetMenu(menuName = "GameEvents/IntGameEvent")]
public class IntGameEventSO : GameEventSO<int> { }


[CreateAssetMenu(menuName = "GameEvents/AudioClipGameEvent")]
public class AudioClipGameEventSO : GameEventSO<AudioClip> { }