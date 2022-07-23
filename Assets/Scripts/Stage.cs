using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // serializable ekleyebilmek icin

[Serializable]
public class Level
{
    [Range(1,11)]
    public int partCount = 11;

    [Range(0, 11)]
    public int deathPartCount = 1;
}

[CreateAssetMenu(fileName = "New Stage")]
// Nesneler icin
public class Stage : ScriptableObject { 
    public Color stageBackgroundColor = Color.white;//default ayar
    public Color stageLevelPartColor = Color.white;//default ayar 
    public Color stageBallColor = Color.white;//default ayar
    public List<Level> levels = new List<Level>();
}
