using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShownEnemyUnit : BaseUnit
{
    public void SetVisible(bool IsVisible)
    {
        meshRenderer.enabled = IsVisible;
    }
}
