using UnityEngine;

[CreateAssetMenu(menuName = "Kingdom/Tree")]
public class TreeData : ScriptableObject
{
    public string treeName;
    public Sprite sprite; // optional if using a prefab
    public Color displayColor = Color.green;
}