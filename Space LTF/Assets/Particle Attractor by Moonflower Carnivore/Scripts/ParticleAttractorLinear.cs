using System.Collections;
using UnityEngine;
[RequireComponent(typeof(ParticleSystem))]
public class ParticleAttractorLinear : MonoBehaviour {
	ParticleSystem ps;
	ParticleSystem.Particle[] m_Particles;
	public Transform target;
    private Vector3 _posTarget;
    private bool _useGO;
    public float pRotation = 0f;

	public float speed = 5f;

	int numParticlesAlive;

	void Awake ()
	{
	    _useGO = (target != null);
		ps = GetComponent<ParticleSystem>();
	    speed = ps.main.startSpeed.constant;

	}

	void Update () {
		m_Particles = new ParticleSystem.Particle[ps.main.maxParticles];
		numParticlesAlive = ps.GetParticles(m_Particles);
	    Vector3 trg = _useGO ? target.position : _posTarget;
        float step = speed * Time.deltaTime;
//	    var dir = trg - ps.transform.position;

        for (int i = 0; i < numParticlesAlive; i++)
		{
		    var p = m_Particles[i];
//            p.rotation =
            p.position = Vector3.LerpUnclamped(transform.position, trg, step);
//            m_Particles[i].rotation = pRotation;
		}
		ps.SetParticles(m_Particles, numParticlesAlive);
    }

    void OnDrawGizmos()
    {
        Vector3 v = _useGO ? target.position : _posTarget;
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position,v);
    }

    public void SetTarget(Vector3 v)
    {
        transform.LookAt(v);
        _posTarget = v;
        _useGO = false;
    }
}
