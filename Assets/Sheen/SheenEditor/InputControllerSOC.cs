using UnityEngine;

public class InputControllerSOC : ScriptableObject
{
    [Header("Touch")]
    [SerializeField] public bool useTouch;
    [SerializeField] [Range(0.01f, 1f)] public float tapThreshold;
    
    [Header("Joystick")]
    [SerializeField] public bool useJoystick;
    [SerializeField] public Sprite joystickCenterSprite;
    [SerializeField] public Sprite joystickKnobSprite;
    [SerializeField] [Range(0.01f, 1f)] public float joystickOutRange;
}