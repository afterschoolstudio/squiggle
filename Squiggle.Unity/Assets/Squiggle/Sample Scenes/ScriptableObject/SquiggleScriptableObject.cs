using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SquiggleCode", menuName = "ScriptableObjects/SquiggleCode", order = 1)]
public class SquiggleScriptableObject : ScriptableObject
{
    [TextArea]
    public string SquiggleText;
}
