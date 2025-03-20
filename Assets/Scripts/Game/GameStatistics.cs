[System.Serializable]
public class GameStatistics
{
    private float programming;
    private float art;
    private float sound;
    private float marketing;

    public event System.Action OnValuesChanged;

    public float ProgrammingValue
    {
        get => programming;
        set
        {
            programming = value;
            OnValuesChanged?.Invoke();
        }
    }

    public float ArtValue
    {
        get => art;
        set
        {
            art = value;
            OnValuesChanged?.Invoke();
        }
    }

    public float SoundValue
    {
        get => sound;
        set
        {
            sound = value;
            OnValuesChanged?.Invoke();
        }
    }

    public float MarketingValue
    {
        get => marketing;
        set
        {
            marketing = value;
            OnValuesChanged?.Invoke();
        }
    }


    public float GetMediumValue()
    {
        return (ProgrammingValue + ArtValue + SoundValue + MarketingValue) / 4;
    }

    public void ResetWhitoutEvent()
    {
        programming = 0;
        art = 0;
        sound = 0;
        marketing = 0;
    }

    public void Reset()
    {
        ResetWhitoutEvent();
        OnValuesChanged?.Invoke();
    }
}
