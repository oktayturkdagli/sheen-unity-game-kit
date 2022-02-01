using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheenJoystick : MonoBehaviour
{
    [SerializeField] private RectTransform center;
    [SerializeField] private RectTransform knob;
    [SerializeField] private float range;
    [SerializeField] private bool fixedJoystick;
    [SerializeField] private bool isActive;

    private Vector2 direction;

    void Start()
    {
        ShowJoystick(false);
        isActive = false;
    }
       
    void Update()
    {
        Vector2 pos = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
        {
            ShowJoystick(true);
            isActive = true;
            knob.position = pos;
            center.position = pos;

        }
        else if (Input.GetMouseButton(0))
        {
            knob.position = pos;
            knob.position = center.position + Vector3.ClampMagnitude(knob.position - center.position, center.sizeDelta.x * range);
            if (knob.position != Input.mousePosition && !fixedJoystick)
            {
                Vector3 obv = Input.mousePosition - knob.position;
                center.position += obv;
            }
            direction = (knob.position - center.position).normalized;
        }
        else
        {
            isActive = false;
        }

        if (!isActive)
        {
            ShowJoystick(false);
            direction = Vector2.zero;
        }
    }

    void ShowJoystick(bool state)
    {
        center.gameObject.SetActive(state);
        knob.gameObject.SetActive(state);

    }

}
