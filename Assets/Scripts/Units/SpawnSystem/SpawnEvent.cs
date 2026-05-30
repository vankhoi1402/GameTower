using System;
using UnityEngine;

public class SpawnEvent : MonoBehaviour
{
    public static Action<UnitData> OnRequestAddUnit;
    public static Action<UnitData> OnRequestRemoveUnit;

    public static Action OnStartBattle;
}
