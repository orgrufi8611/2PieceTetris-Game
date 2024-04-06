using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum Shapes
{
    I,O,T,L,J,S,Z,count
}

public  class ShapesData
{
    public static float[] RotateMatrix = new float[4]
    {
        Mathf.Cos(Mathf.PI/2),
        -Mathf.Sin(Mathf.PI/2),
        Mathf.Sin(Mathf.PI/2),
        Mathf.Cos(Mathf.PI/2),
    };
    
    public Vector2Int[] cells = new Vector2Int[4];
    public Shapes shape;
    public static ShapesData[] AvailableShapes = new ShapesData[2]
    {
        //new ShapeI(),
        new ShapeO(),
        new ShapeT(),
        //new ShapeL(),
        //new ShapeJ(),
        //new ShapeS(),
        //new ShapeZ(),
    };

    public Tile tile {  get; protected set; }

    public Board board;

    public virtual Vector2Int[] Rotate(int dir) { return cells; }

}

public class ShapeO : ShapesData
{
    public ShapeO()
    {
        tile = TilesColors.Instance.tiles[0];
        shape = Shapes.O;
        cells[0] = new Vector2Int(0, 0);
        cells[1] = new Vector2Int(0, 1);
        cells[2] = new Vector2Int(1, 0);
        cells[3] = new Vector2Int(1, 1);
    }
}


public class ShapeI : ShapesData
{
    public ShapeI()
    {
        tile = TilesColors.Instance.tiles[1];
        shape = Shapes.I;
        cells[0] = new Vector2Int(0, -1);
        cells[1] = new Vector2Int(0, 0);
        cells[2] = new Vector2Int(0, 1);
        cells[3] = new Vector2Int(0, 2);
    }
}


public class ShapeL : ShapesData
{
    public ShapeL()
    {
        tile = TilesColors.Instance.tiles[2];
        shape = Shapes.L;
        cells[0] = new Vector2Int(1, -1);
        cells[1] = new Vector2Int(0, -1);
        cells[2] = new Vector2Int(0, 0);
        cells[3] = new Vector2Int(0, 1);
    }
}


public class ShapeJ : ShapesData
{
    public ShapeJ()
    {
        tile = TilesColors.Instance.tiles[3];
        shape = Shapes.O;
        cells[0] = new Vector2Int(0, -1);
        cells[1] = new Vector2Int(0, 0);
        cells[2] = new Vector2Int(0, 1);
        cells[3] = new Vector2Int(1, 1);
    }
}


public class ShapeT : ShapesData
{
    public ShapeT()
    {
        tile = TilesColors.Instance.tiles[4];
        shape = Shapes.T;
        cells[0] = new Vector2Int(0, -1);
        cells[1] = new Vector2Int(0, 0);
        cells[2] = new Vector2Int(1, 0);
        cells[3] = new Vector2Int(0, 1);
    }
    public override Vector2Int[] Rotate(int dir)
    {
        Vector2Int[] newCells = new Vector2Int[cells.Length];
        for (int i = 0; i < cells.Length; i++)
        {
            int x = Mathf.RoundToInt(cells[i].x * RotateMatrix[0] * dir + cells[i].y * RotateMatrix[1] * dir);
            int y = Mathf.RoundToInt(cells[i].x * RotateMatrix[2] * dir + cells[i].y * RotateMatrix[3] * dir);
            newCells[i] = new Vector2Int(x, y);
        }
        return newCells;
    }
}

public class ShapeS : ShapesData
{
    public ShapeS()
    {
        tile = TilesColors.Instance.tiles[5];
        shape = Shapes.S;
        cells[0] = new Vector2Int(0, -1);
        cells[1] = new Vector2Int(0, 0);
        cells[2] = new Vector2Int(1, 0);
        cells[3] = new Vector2Int(1, 1);
    }
}


public class ShapeZ : ShapesData
{
    public ShapeZ()
    {
        tile = TilesColors.Instance.tiles[6];
        shape = Shapes.Z;
        cells[0] = new Vector2Int(1, -1);
        cells[1] = new Vector2Int(1, 0);
        cells[2] = new Vector2Int(0, 0);
        cells[3] = new Vector2Int(0, 1);
        
    }
}

