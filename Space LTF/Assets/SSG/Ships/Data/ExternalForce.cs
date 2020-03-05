using UnityEngine;

public class ExternalForce
{
    public bool IsActive;
    private Vector3 Dir;
    private float Power;
    private float EndTime;
    private float Period;


    public ExternalForce()
    {
        IsActive = false;
    }

    public void Init(float power, float delay, Vector3 dir)
    {
        IsActive = true;
        dir.y = 0;
        Dir = Utils.NormalizeFastSelf(dir);
        Power = power;
        EndTime = Time.time + delay;
        Period = delay;
    }

    public Vector3 Update()
    {
        var delta = EndTime - Time.time;
        if (delta <= 0)
        {
            Stop();
            return Vector3.zero;
        }
        var d = delta / Period;
        var p = Power * d;

        return p * Dir * Time.deltaTime;
    }

    public void Stop()
    {

        IsActive = false;
    }

}