using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    public static bool gameStarted = false;
    public static int currentLevelIndex = 0;
    public static int currentScore = 0;
    public static MoleculeData currentMolecule = null;
    public static MoleculeData savedMolecule = null; // Fix 1

    public static List<Ball.FragmentType> pottedBallsInOrder =
        new List<Ball.FragmentType>();

    public static Dictionary<Ball.FragmentType, GameObject> fragmentToPrefab =
        new Dictionary<Ball.FragmentType, GameObject>();

    public static string moleculeName = "";
    public static string moleculeFormula = "";
    public static string isomerismType = "";

    public static void Clear()
    {
        pottedBallsInOrder.Clear();
        fragmentToPrefab.Clear();
    }

    public static void ResetAll() // Fix 2
    {
        gameStarted = false;
        currentLevelIndex = 0;
        currentScore = 0;
        currentMolecule = null;
        savedMolecule = null;
        moleculeName = "";
        moleculeFormula = "";
        isomerismType = "";
        Clear();
    }
}