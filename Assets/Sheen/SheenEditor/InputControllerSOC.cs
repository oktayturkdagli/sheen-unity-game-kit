using UnityEngine;
using Sheen.Touch;

public class InputControllerSOC : ScriptableObject
{
    [Header("Touch")]
    [SerializeField] public bool useTouch;
    [SerializeField] public float referenceDpi;
    [SerializeField] public float tapThreshold;
    [SerializeField] public float swipeThreshold;
    
    [Header("Joystick")]
    [SerializeField] public bool useJoystick;
    [SerializeField] public Texture joystickCenterTexture;
    [SerializeField] public Texture joystickKnobTexture;
    [SerializeField] [Range(0f, 10f)] public float joystickOutRange;
}