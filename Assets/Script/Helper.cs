using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    public static float EaseOut(float t) => -t * t + 2 * t;

    public static float EaseIn(float t) => t * t;

    public static float EaseInOut(float t) => Mathf.Lerp(EaseIn(t), EaseOut(t), t);

    public static Vector3 Vec2toVec3(Vector2Int vector) => new Vector3(vector.x, 0, vector.y);

    public static Vector2Int Vec3toVec2(Vector3 vector) => new Vector2Int(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.z));

    public static Vector3 NormalVec2toVec3(Vector2 vector) => new Vector3(vector.x, 0, vector.y);

    public static Vector2Int RoundVec2(Vector2 vector) => new Vector2Int(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y));

    public static Vector2Int[] neigbourTiles = new Vector2Int[]
        {
            new Vector2Int(0,1),
            new Vector2Int(0,-1),
            new Vector2Int(1,0),
            new Vector2Int(-1,0),
            new Vector2Int(1,1),
            new Vector2Int(-1,-1),
            new Vector2Int(1,-1),
            new Vector2Int(-1,1)
        };

    public static Vector2Int[] farNeighbourTiles = new Vector2Int[]
        {
            new Vector2Int(0,2),
            new Vector2Int(0,-2),
            new Vector2Int(2,0),
            new Vector2Int(-2,0),
            new Vector2Int(2,2),
            new Vector2Int(-2,-2),
            new Vector2Int(2,-2),
            new Vector2Int(-2,2),
            new Vector2Int(1,2),
            new Vector2Int(-1,2),
            new Vector2Int(1,-2),
            new Vector2Int(-1,-2),
            new Vector2Int(2,1),
            new Vector2Int(2,-1),
            new Vector2Int(-2,1),
            new Vector2Int(-2,-1)
        };
}