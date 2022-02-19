using UnityEngine;

public class InputControllerSO : ScriptableObject
{
    [Header("Touch")]
    [SerializeField] public bool useTouch;
    [SerializeField] [Range(0.01f, 1f)] public float tapThreshold;
    [SerializeField] public bool workOnHalfOfScreenTouch;

    [Header("Joystick")]
    [SerializeField] public bool useJoystick;
    [SerializeField] public Sprite joystickCenterSprite;
    [SerializeField] public Sprite joystickKnobSprite;
    [SerializeField] [Range(0.01f, 1f)] public float joystickOutRange;
    [SerializeField] public bool fixedJoystick;
    [SerializeField] public bool alwaysDisplayJoystick;
    [SerializeField] public bool workOnHalfOfScreenJoystick;
}