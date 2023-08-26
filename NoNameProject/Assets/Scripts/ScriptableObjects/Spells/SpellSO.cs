using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SpellSO : ScriptableObject
{
    public string spellName;
    public int damage;
    public List<KeyCode> keyCodes;
    public List<Sprite> arrowSprites;
}
