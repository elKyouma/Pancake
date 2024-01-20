
using UnityEngine;

public class RaycastHelper
{
    public static bool IsInSight(Vector2 pos, Vector2 view)
    {
        int layerMask = 1 << 6; // Obstacles layer
        RaycastHit2D hitWall = Physics2D.Linecast(view, pos, layerMask);
        Debug.Log($"{pos} {view} {!hitWall}");
        return !hitWall;
    }

    public static bool IsInBulletSight(Vector2 pos, Vector2 view, float bulletRadius)
    {
        // bulletRadius /= 2;
        return IsInSight(pos, view + Vector2.down * bulletRadius)
        && IsInSight(pos, view + Vector2.up * bulletRadius)
        && IsInSight(pos, view + Vector2.left * bulletRadius)
        && IsInSight(pos, view + Vector2.right * bulletRadius);
    }
}