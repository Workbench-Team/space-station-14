namespace Content.Client.SurveillanceCamera;

[RegisterComponent]
public sealed partial class ActiveSurveillanceCameraMonitorVisualsComponent : Component
{
    public float TimeLeft = 2f;

    public Action? OnFinish;
}
