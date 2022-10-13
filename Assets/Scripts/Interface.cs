using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interface : MonoBehaviour
{
    public static Interface Instance { get; private set; }

    public static EventHandler<Vector3> OnRightMouseButtonDown;
    public static EventHandler OnKeyDownN;

    private void Awake()
    {
        // test only one
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            OnRightMouseButtonDown?.Invoke(this, DestinationFromClick());
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            OnKeyDownN?.Invoke(this, EventArgs.Empty);
        }
    }


    Vector3 DestinationFromClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, float.PositiveInfinity))
        {
            return hitInfo.point;
        }
        else
        {
            Debug.LogError("No valid destination");
            return HQ.NULL_VECT;
        }
    }
}
