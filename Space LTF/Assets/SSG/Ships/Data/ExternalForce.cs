using UnityEngine;

public class ExternalForce
{
    public bool IsActive;
    private Vector3 Dir;
    private float Power;
    private float EndTime;
    private float Period;
    private float _coef = 1f;


    public ExternalForce(float coef = 1f)
    {
        _coef = coef;
        IsActive = false;
    }

    public void Init(float power, float delay, Vector3 dir)
    {
        IsActive = true;
        dir.y = 0;
        Dir = Utils.NormalizeFastSelf(dir);
        Power = power * _coef;
        Period = delay * _coef;
        EndTime = Time.time + Period;
        // Debug.LogError($"Init ext force power:{power} delay:{delay} dir:{dir}");
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
        var upd = p * Dir * Time.deltaTime;
       // Debug.LogError($"Update ext force p:{p}");

        return upd;
    }

    public void Stop()
    {

        IsActive = false;
    }

}