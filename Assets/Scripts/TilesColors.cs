using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilesColors : MonoBehaviour
{
    public static TilesColors Instance { get; private set; }

    public Tile[] tiles = new Tile[7];

    private void Awake()
    {
        Instance = this;
    }
}
