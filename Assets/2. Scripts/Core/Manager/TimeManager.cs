using Scripts.Manager;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    public float currentTime = 1.0f;
    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;

    }
    public float GetTimeScale() => Time.timeScale;

    public void PauseGame()
    {
        SetTimeScale(0);
    }
    public void ResumeGame()
    {
        SetTimeScale(1);
    }
    
}