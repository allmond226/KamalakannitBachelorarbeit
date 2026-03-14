using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    public string speakerName;
    public Sprite NewsImage;
    [TextArea(3, 10)]
    public string[] lines;
}
