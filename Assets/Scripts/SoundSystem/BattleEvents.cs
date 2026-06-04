using System;
using UnityEngine;

public static class BattleEvents
{
    public static Action<SoundType, Vector3> OnPlayBattleSound3D;

    public static void RaisePlaySound3D(SoundType type, Vector3 position)
        => OnPlayBattleSound3D?.Invoke(type, position);
}