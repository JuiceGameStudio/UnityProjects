using CustomSystem.Pooling;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ScriptablePoolling : ScriptableObject
{
    public string scriptableName; // Le nom donn� au ScriptableObject

    [HideInInspector] public string Name; // Nom afficher sur la classe
    public GameObject entity; // Le gameobject de l'entit� voulant �tre pool
    public string customEntityType = "DEFAULT"; // Le nom du custom enum de l'entit� voulant �tre pool
    public int nmbOfEntity; // Le nombre d'entit� max pouvant �tre pool et cr�er au d�marrage
    public PoollingEntityType entityType = PoollingEntityType.DEFAULT; // Le type de l'entit� voulant �tre pool
}