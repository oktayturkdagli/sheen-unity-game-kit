using UnityEngine;
using System.Collections.Generic;

public class LevelControllerSO : ScriptableObject
{
    [Header("Level Controller")]
    [SerializeField] public string direction;
    [SerializeField] [Range(0, 3)] public int dropDown1Selection;
    [SerializeField] [Range(0, 1)] public int dropDown2Selection;
    [SerializeField] [Range(0, 1)] public int dropDown3Selection;
    [SerializeField] [Range(0, 1)] public int dropDown4Selection;
    [SerializeField] public List<GameObject> startSegment;
    [SerializeField] public List<GameObject> midSegment;
    [SerializeField] public List<GameObject> finalSegment;
    [SerializeField] [Range(0, 100)] public int startSegmentUnit;
    [SerializeField] [Range(0, 100)] public int midSegmentUnit;
    [SerializeField] [Range(0, 100)] public int finalSegmentUnit;
}