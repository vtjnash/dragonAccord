using UnityEngine;
using System.Collections;
using UnityEngine.VR.WSA.Persistence;
using UnityEngine.VR.WSA;

public class MusicNode : MonoBehaviour {

    public AudioSource audioSource;
    public ParticleSystem[] particles;

    public string ObjectAnchorStoreName { get; private set; }
    private WorldAnchorStore anchorStore;

    public bool Moving { get; private set; }
    public bool AnchorLocked { get; private set; }

    public AudioSource masterSource;

    // Use this for initialization
    void Start () {
        

    }

    public void SetAnchorID(string id)
    {
        //GameObject.Find("DebugLayer").GetComponent<TextMesh>().text = "ID= "+id;
        ObjectAnchorStoreName = id;
        WorldAnchorStore.GetAsync(AnchorStoreReady);
    }

    public string GetAnchorID()
    {
        return ObjectAnchorStoreName;
    }

    public void SetAudioClip(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Stop();
        

    }

    public void SetMasterAudioSync(AudioSource masterAudio)
    {
        masterSource = masterAudio;
    }

    // Update is called once per frame
    void Update () {
        
        //Debug.Log(masterSource.timeSamples);
        /*float freq = audioClip.clip.frequency;
        if (audioClip.isPlaying)
        {
            int i;
            int limit = particles.Length;
            for (i=0;i<limit;++i)
            {
                ParticleSystem ps = particles[i].GetComponent<ParticleSystem>();
                var em = ps.emission;
                em.enabled = true;

                em.type = ParticleSystemEmissionType.Time;
                
                em.rate = SequenceManager.Instance.bpm;
            }
        }*/

        if (Moving)
        {
            gameObject.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2;
        }
    }

    void AnchorStoreReady(WorldAnchorStore store)
    {
        anchorStore = store;

        string[] ids = anchorStore.GetAllIds();
        for (int i = 0; i < ids.Length; i++)
        {
            if (ids[i] == ObjectAnchorStoreName)
            {
                WorldAnchor wa = anchorStore.Load(ids[i], gameObject);
                break;
            }
        }

    }


    public void SetAnchorLock(bool value)
    {
        AnchorLocked = value;
       // this.transform.Find("Lock").gameObject.SetActive(!value);
        if (anchorStore == null)
        {
            //GameObject.Find("DebugLayer").GetComponent<TextMesh>().text += " NO ANCHOR STORE";
            return;
        }
        if (AnchorLocked)
        {
            //transform.Find("Billboard").gameObject.transform.position.Set(0.0f, 1.0f, 0.0f);

            WorldAnchor attachingAnchor = gameObject.AddComponent<WorldAnchor>();
            if (attachingAnchor.isLocated)
            {
                bool saved = anchorStore.Save(ObjectAnchorStoreName, attachingAnchor);
            }
            else
            {
                attachingAnchor.OnTrackingChanged += AttachingAnchor_OnTrackingChanged;
            }
            //GameObject.Find("DebugLayer").GetComponent<TextMesh>().text += "anchor=" + ObjectAnchorStoreName;
        }
        else
        {
            WorldAnchor anchor = gameObject.GetComponent<WorldAnchor>();
            if (anchor != null)
            {
                DestroyImmediate(anchor);
            }

            string[] ids = anchorStore.GetAllIds();
            for (int i = 0; i < ids.Length; i++)
            {
                if (ids[i] == ObjectAnchorStoreName)
                {
                    bool deleter = anchorStore.Delete(ids[i]);
                    break;
                }
            }
        }
    }


    public void OnBeat()
    {
        
        //Debug.Log("On Beat" + this.ObjectAnchorStoreName + " "+audioSource.isPlaying);
        if (audioSource.isPlaying)
        {

            //audioSource.Stop();
            //audioSource.Play();

            //TODO: sync clips with % 3 if beyond threshold then reset to previous nearest rounded divisible 
            // ask jameson to repeat what he said about mod rounding it sounds nice.
            audioSource.timeSamples = masterSource.timeSamples;
        }
        else
        {
           // audioSource.timeSamples = masterSource.timeSamples;
           // Debug.Log(audioSource.timeSamples + "  " + masterSource.timeSamples);
            audioSource.PlayScheduled(0);
            audioSource.timeSamples = masterSource.timeSamples;
            // 

        }
        

    }

    public void OnSelect()
    {

    }

    public void OnMove()
    {

    }

    public void OnDrop()
    {

    }

    public void OnLock()
    {

    }

    public void OnUnlock()
    {

    }


    private void AttachingAnchor_OnTrackingChanged(WorldAnchor self, bool located)
    {
        if (located)
        {
            bool saved = anchorStore.Save(ObjectAnchorStoreName, self);
            self.OnTrackingChanged -= AttachingAnchor_OnTrackingChanged;
        }
    }
}
