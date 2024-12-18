using UnityEngine;

public static class CMath
{
    public static Vector3 RoundVector3Data(Vector3 v)
    {
        return new(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));
    }
    public static Vector2 RoundVector2Data(Vector2 v)
    {
        return new(Mathf.Round(v.x), Mathf.Round(v.y));
    }
}