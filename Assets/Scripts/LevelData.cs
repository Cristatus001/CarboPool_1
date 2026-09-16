using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "CarboPool/LevelData")]
public class LevelData : ScriptableObject
{
    [Header("Level Info")]
    public string levelName;
    public string isomerismType;
    public string targetMolecule;
    public string targetFormula;

    [Header("Required Fragments")]
    public int ch3Required;
    public int ch2Required;
    public int chRequired;

    [Header("Timer")]
    public float timeLimit = 180f;
}