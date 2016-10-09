using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioInfo : MonoBehaviour {
	public int nsamples = 64; //Min = 64. Max = 8192.
	public float overInterpolateLimit = 1.25f;

	private AudioSource audiosource;
	private bool donesampling;
	public struct Sample {
		public float attime;
		public float low;
		public float mid;
		public float high;
		public Sample(float attime) {
			this.attime = attime; low = 0; mid = 0; high = 0;
		}
		public Sample(float attime, float low, float mid, float high) {
			this.attime = attime; this.low = low; this.mid = mid; this.high = high;
		}
	};
	private List<Sample> samples = new List<Sample>();
	float lasttime;

	// temporary
	void Start() {
		StartLooping();
	}

	void Update() {
		Sample s = GetSample();
		BroadcastMessage("FFTUpdate", s);
	}

	void StartLooping() {
		audiosource = GetComponentInChildren<AudioSource>();
		audiosource.time = 0;
		audiosource.Play();
		donesampling = false;
		StartCoroutine(ComputeSpectrum());
	}

	Sample GetSample() {
		float attime = audiosource.time;
		int count = samples.Count;
		if (count == 0 || lasttime == 0)
			return new Sample(0f);
		if (count == 1)
			return samples[count - 1];
		int n0 = Mathf.FloorToInt(count * attime / lasttime); // guess closest
		if (n0 + 1 >= count)
			n0 = count - 2;
		while (n0 > 0 && samples[n0].attime > attime)
			n0 -= 1;
		while (n0 < count - 2 && samples[n0 + 1].attime < attime)
			n0 += 1;
		Sample sn0;
		Sample sn1;
		sn0 = samples[n0];
		sn1 = samples[n0 + 1];
		if (sn1.attime - sn0.attime < 0.01f)
			return sn0;
		float pct = (attime - sn0.attime) / (sn1.attime - sn0.attime);
		if (pct > 1f && donesampling)
			return sn0; // no data past the end
		float pctm1 = 1 - pct;
		return new Sample(
			attime,
			Mathf.Min(sn0.low * pctm1 + sn1.low * pct, Mathf.Max(sn0.low, sn1.low) * overInterpolateLimit),
			Mathf.Min(sn0.mid * pctm1 + sn1.mid * pct, Mathf.Max(sn0.mid, sn1.mid) * overInterpolateLimit),
			Mathf.Min(sn0.high * pctm1 + sn1.high * pct, Mathf.Max(sn0.high, sn1.high) * overInterpolateLimit)
		);
	}

	IEnumerator	ComputeSpectrum() {
		lasttime = 0.0f;
		samples.Clear();
		float[] spectrum = new float[nsamples];
		while (true) {
			float time = audiosource.time;
			if (time < lasttime)
				break;
			audiosource.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

			Sample s = new Sample(time);
			int third = spectrum.Length / 12;
			for (int i = 1; i < third; i++) {
				s.low += spectrum[i];
			}
			for (int i = third; i < 2 * third; i++) {
				s.mid += spectrum[i];
			}
			for (int i = 2 * third; i < 3 * third; i++) {
				s.high += spectrum[i];
			}
			samples.Add(s);
			lasttime = time;
			yield return new WaitForSeconds(0.1f);

		}
		donesampling = true;
	}
}
