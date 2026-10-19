using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "COTV/Quest")]
public class QuestData : ScriptableObject
{
    public string questID;
    public string questTitle;

    [TextArea(2, 5)]
    public string questDescription;
}
