public struct TimeCoef
{
    public float Time;
    public float Value;
    public float TimeFromPrev;
    public float ValueFromPrev;

    public TimeCoef(float time, float timeToPrev, float value, float valueToPrev)
    {
        Time = time;
        Value = value;
        TimeFromPrev = timeToPrev;
        ValueFromPrev = valueToPrev;
    }
}





