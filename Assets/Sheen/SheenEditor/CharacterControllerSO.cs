using UnityEngine;

public class CharacterControllerSO : ScriptableObject
{
    [Header("Character")]
    [SerializeField] public bool gender;
    [SerializeField] public int hair;
    [SerializeField] public int eyebrows;
    [SerializeField] public int chest;
    [SerializeField] public int arms;
    [SerializeField] public int legs;
    [SerializeField] public int feet;
}