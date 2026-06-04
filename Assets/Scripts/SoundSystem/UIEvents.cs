using System;

public static class UIEvents
{
    public static Action<SoundType> OnPlayUISound;

    public static void RaisePlaySound(SoundType type)
        => OnPlayUISound?.Invoke(type);
}