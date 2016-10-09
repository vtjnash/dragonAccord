using UnityEngine;
using System.Collections;
using UnityEngine.VR.WSA.Persistence;
using UnityEngine.VR.WSA;

public class MusicNode : MonoBehaviour {

    public AudioSource audioSource;
    public GameObject[] particles;

    public string ObjectAnchorStoreName { get; private set; }
    private WorldAnchorStore anchorStore;

    public bool Moving { get; private set; }
    public bool Editing { get; private set; }
    public bool IsSynced { get; private set; }
    public bool AnchorLocked { get; private set; }

    private static Color[] colors =
    {
        Color.white, //0
        Color.white,
        Color.white,
        Color.white,
        Color.white,
        Color.white, //5
        Color.red,
        Color.white,
        Color.white,
        Color.white,
        Color.red, //10        
        Color.white,
        Color.white,
        Color.red,
        Color.white,
        Color.white, //15
        Color.white,
        Color.white,
        Color.white,
        Color.white,
        Color.white // 20
    };

    public int Id;

    public bool Stopped;

    public AudioSource masterSource;

    // Use this for initialization
    void Start () {
        
        OnUnlock();
    }

    public void SetAnchorID(string id)
    {
        //GameObject.Find("DebugLayer").GetComponent<TextMesh>().text = "ID= "+id;
        ObjectAnchorStoreName = id;
        WorldAnchorStore.GetAsync(AnchorStoreReady);
    }

    public void SetNodeID(int id)
    {
        Id = id;
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

        int i;
        int limit = particles.Length;
        for (i = 0; i < limit; ++i)
        {
            /*ParticleSystem ps = particles[i].GetComponent<ParticleSystem>();
            //Debug.Log("particles" + i);
            var em = ps.emission;
            em.enabled = true;

            em.type = ParticleSystemEmissionType.Time;

            em.rate = SequenceManager.Instance.bpm / 10;

            em.SetBursts(
                    new ParticleSystem.Burst[]{
                        new ParticleSystem.Burst(0.0f, 100)
                    });*/
        }
    }

    // Update is called once per frame
    void Update () {
        
        //Debug.Log(masterSource.timeSamples);
        //float freq = audioSource.clip.frequency;
        if (audioSource.isPlaying)
        {
            /**/
        }

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
        //Material outerShell = this.GetComponent<Renderer>().materials[1];
        //outerShell.SetColor(0,Color.clear);
        //outerShell.
       // Debug.Log(this.GetComponent<Renderer>().materials[1].GetColor(0));
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
        // on bar trigger
        //Debug.Log("onBeat");
        if (audioSource.isPlaying)
        {

            int i;
            int limit = particles.Length;
            for (i = 0; i < limit; ++i)
            {
                ParticleSystem ps = particles[i].GetComponent<ParticleSystem>();
                //Debug.Log("particles" + i);
                var em = ps.emission;
                em.enabled = true;
                ps.Emit(10);
            }

            
            //em.burstCount = SequenceManager.Instance.bpm / 10;
        }
    }

    public void OnBar()
    {
       // Debug.Log("onBar");
        //Debug.Log("On Beat" + this.ObjectAnchorStoreName + " "+audioSource.isPlaying);
        if (!Stopped)
        {
            if (audioSource.isPlaying)
            {
                if (GetComponent<Animation>() != null)
                {
                    Animation anim = GetComponent<Animation>();
                    anim.Play();
                }

                if (!IsSynced && audioSource.timeSamples < 1f)
                {
                    audioSource.timeSamples = masterSource.timeSamples;
                    IsSynced = true;
                }
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
        
        if (Id > 0 && Id < colors.Length && colors[Id] != Color.white)
        {
            Color baseColor = Stopped ? Color.gray : colors[Id];
            
            Renderer[] rs = GetComponentsInChildren<Renderer>();
            for (int i = 0; i < rs.Length; i++)
            {
                Renderer r = rs[i];
                if (r.material.HasProperty("_TintColor"))
                {
                    Color c = baseColor;
                    Color c0 = r.material.GetColor("_TintColor");
                    c.a = c0.a;
                    r.material.SetColor("_TintColor", c);
                }
            }
        }
    }

    public void OnSelect()
    {
        //SequenceManager.Instance.DestroyNode(Id);
    }

    public void OnStop()
    {
        Stopped = true;
        audioSource.Stop();
        IsSynced = false;
    }

    public void OnPlay()
    {
        Stopped = false;
    }

    public void OnRemove()
    {
        OnStop();
        gameObject.SetActive(false);

    }

    public void OnEditing()
    {
        Editing = true;
    }


    public void OnMove()
    {
        Moving = true;

    }

    public void OnDrop()
    {
        Moving = false;
    }

    public void OnLock()
    {
        SetAnchorLock(true);
        Editing = false;
    }

    public void OnUnlock()
    {
        SetAnchorLock(false);
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
