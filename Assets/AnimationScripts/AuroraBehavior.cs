using UnityEngine;
using System.Collections;

public class AuroraBehavior : MonoBehaviour {
	public float heightMultiplier = 2f;
	public float decayRate = 0.96f;
	public int nsamples = 512;

	private ParticleSystem particles;
	private float timeBase;
	private ParticleSystem.Particle[] m_Particles;
	private float[] leftear, rightear;
	private float decay;

	// Use this for initialization
	void Start () {
		particles = GetComponent<ParticleSystem>();
		timeBase = Time.time;
		leftear = new float[nsamples];
		rightear = new float[nsamples];
		decay = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		AudioListener.GetOutputData(leftear, 0);
		AudioListener.GetOutputData(rightear, 1);
		float maxVolume = 0;
		for (int i = 0; i < leftear.Length; i++)
			if (leftear[i] > maxVolume)
				maxVolume = leftear[i];
		for (int i = 0; i < rightear.Length; i++)
			if (rightear[i] > maxVolume)
				maxVolume = rightear[i];

		decay = Mathf.Max(maxVolume, decay * decayRate);
		float height = decay * heightMultiplier;
		Vector3 scale = particles.transform.localScale;
		scale.z = height;
		particles.transform.localScale = scale;
		particles.gravityModifier = height;

//		if (m_Particles == null || m_Particles.Length < particles.maxParticles)
//			m_Particles = new ParticleSystem.Particle[particles.maxParticles];
//		int numParticlesAlive = particles.GetParticles(m_Particles);
//		// Change only the particles that are alive
//		for (int i = 0; i < numParticlesAlive; i++) {
//			Vector3 startSize = m_Particles[i].startSize3D;
//			startSize.y = height * heightMultiplier;
//			m_Particles[i].startSize3D = startSize;
//		}
//		// Apply the particle changes to the particle system
//		particles.SetParticles(m_Particles, numParticlesAlive);
	}
}
