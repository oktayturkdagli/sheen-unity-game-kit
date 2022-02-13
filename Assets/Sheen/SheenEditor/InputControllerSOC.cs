using UnityEngine;

public class InputControllerSOC : ScriptableObject
{
    [SerializeField] public float referenceDpi;
    [SerializeField] public float tapThreshold;
    [SerializeField] public float swipeThreshold;

    [SerializeField] public bool useJoystick;
}