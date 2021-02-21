using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] Material groundMat;
    [SerializeField] Material edgeMat;
    [SerializeField] Material nonCollectableMat;
    [SerializeField] Material transitionWallMat;

    [SerializeField] Material holeMat;

    [SerializeField] Color groundColor;
    [SerializeField] Color edgeColor;
    [SerializeField] Color transitionWallColor;

    [SerializeField] Color nonCollectableColor;

    private void Start()
    {
        ApplyLevelColors();
    }
    private void ApplyLevelColors()
    {
        groundMat.color = groundColor;
        edgeMat.color = edgeColor;
        nonCollectableMat.color = nonCollectableColor;
        transitionWallMat.color = transitionWallColor;

        holeMat.SetColor("EdgeColor", groundColor);
    }

    private void OnValidate()
    {
        ApplyLevelColors();
    }
}
