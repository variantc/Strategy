using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : BaseUnit
{
    protected override void Awake()
    {
        base.Awake();
        SetVisible(false);
    }
    public void SetVisible(bool IsVisible)
    {
        meshRenderer.enabled = IsVisible;
    }
}
