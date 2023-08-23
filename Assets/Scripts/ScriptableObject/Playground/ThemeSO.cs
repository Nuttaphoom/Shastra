using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ThemeSO", menuName = "ScriptableObject/Theme")]
public class ThemeSO : ScriptableObject
{
    public new string name;
    [Header("Model and position")]
    public GameObject[] model;
    public Vector3[] position;

    public GameObject tabs;
    public GameObject castleModel_P;
    public GameObject castleModel_E;
    public GameObject board;
    public SpriteRenderer playerChar;
    public SpriteRenderer enemyChar;
}