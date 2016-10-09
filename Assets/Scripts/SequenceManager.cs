using UnityEngine;
using System.Collections;
using HoloToolkit.Unity;
using System.Collections.Generic;

public class SequenceManager : Singleton<SequenceManager> {

    public float playbackTime { get; private set; }
    public GameObject Cursor;
    public GameObject NotePrefab;

    public List<GameObject> prefabs;
    public List<AudioClip> audioClips;
    public List<GameObject> musicNodes;

    private int order;

    public GameObject sequence;

    public AudioSource masterAudio;


    public float bpm { get; set;}

	// Use this for initialization
	void Start () {
        playbackTime = 0f;
        bpm = 80f;
        order = 0;

    }
	
	// Update is called once per frame
	void Update () {
        playbackTime = Time.time;

        CheckSequence();
	}

    public void OnBeatMasterSync()
    {
        masterAudio.Stop();
        masterAudio.Play();
        
    }

    public void DestroyNode(int id)
    {
       // musicNodes[id]
        //BeatCounter.Instance.observersList[id]
    }

    public void OnPlaceNote()
    {
        //clipValue = 
        
        int noteID = (int)Random.Range(0, (prefabs.Count - 1));
        order++;
        if (order >= prefabs.Count)
        {
            order = 0;
        }
        Debug.Log(noteID);
        GameObject prefabNote = prefabs[noteID];
        GameObject note = Instantiate(prefabNote, Cursor.transform.position, Cursor.transform.rotation) as GameObject;

        note.transform.parent = sequence.transform;
        MusicNode musicNode = (note.GetComponent<MusicNode>() as MusicNode);
        // musicNode.GetComponent<AudioSource>
        musicNode.SetNodeID(noteID);
        musicNode.SetAnchorID("node"+ musicNodes.Count.ToString());
        musicNode.SetAudioClip(audioClips[noteID]);
        musicNodes.Add(note);
        musicNode.SetMasterAudioSync(masterAudio);
        BeatCounter.Instance.observersList.Add(note);
    }

    private void CheckSequence()
    {
        //Debug.Log((Mathf.Round(playbackTime * 100) / 100));
       /* if ((Mathf.Round(playbackTime*100)/100) % (bpm%60) == 0f)
        {
            //Debug.Log("Play");
            sequence.BroadcastMessage("OnPlay");
        }*/
    }
}
