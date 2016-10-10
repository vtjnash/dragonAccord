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

    public GameObject activeSelection;

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



    public void Change()
    {
        var focusObject = GazeManager.Instance.FocusedObject;
        if (focusObject != null && focusObject.CompareTag("MusicNote"))
        {

            int noteId = order;// (int)Random.Range(0, (prefabs.Count - 1));
            order++;
            if (order >= prefabs.Count)
            {
                order = 0;
            }
            GameObject prefabNote = prefabs[noteId];
            MusicNode musicNode = (focusObject.GetComponent<MusicNode>() as MusicNode);
            musicNode.DestroyClipMarker();
            musicNode.SetClipMarker(prefabNote);
            musicNode.SetAudioClip(audioClips[noteId]);
            musicNode.FlagSync();

        }
    }

    public void Clone()
    {
        var focusObject = GazeManager.Instance.FocusedObject;
        if (focusObject != null && focusObject.CompareTag("MusicNote"))
        {

            MusicNode originalMusicNode = (focusObject.GetComponent<MusicNode>() as MusicNode);
            GameObject note = Instantiate(focusObject, Cursor.transform.position, Cursor.transform.rotation) as GameObject;
            //note.transform.localScale = originalMusicNode.transform.localScale;
            note.transform.parent = sequence.transform;
            MusicNode musicNode = (note.GetComponent<MusicNode>() as MusicNode);
            musicNode.OnMove();
            musicNode.SetClipMarker(originalMusicNode.clipPrefab);
            //musicNode.clipMarker.transform.localScale = originalMusicNode.clipMarker.transform.localScale;
            musicNode.SetNodeID(musicNodes.Count);
            musicNode.SetAnchorID("node" + musicNodes.Count.ToString());
            musicNode.SetAudioClip(originalMusicNode.audioSource.clip);
            musicNodes.Add(note);
            musicNode.SetMasterAudioSync(masterAudio);
            BeatCounter.Instance.observersList.Add(note);
        }
    }

    public void Move()
    {
        if (activeSelection)
        {

        }
        else
        {
            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                focusObject.SendMessage("OnMove");
            }
            activeSelection = focusObject;
        }
    }

    public void Drop()
    {
        if (!activeSelection)
        {
            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                focusObject.SendMessage("OnDrop");
            }
        }
        else
        {
            activeSelection.SendMessage("OnDrop");
            activeSelection = null;
        }
    }


    public void RemoveNode(int nodeID)
    {
        BeatCounter.Instance.observersList.RemoveAt(nodeID);
    }

    public void RemoveFocusedNode()
    {
        var focusObject = GazeManager.Instance.FocusedObject;
        if (focusObject != null && focusObject.GetComponent<MusicNode>() != null)
        {
            MusicNode musicNode = focusObject.GetComponent<MusicNode>();
            
           // BeatCounter.Instance.observersList.Remove(musicNode.GetAnchorID);
           // musicNode.RemoveNode();
        }

    }

    public void OnRemoveAllNodes()
    {
        int i;
        int limit = musicNodes.Count;
        for (i=0;i<limit;++i)
        {
            MusicNode musicNode = musicNodes[i].GetComponent<MusicNode>();
            musicNode.RemoveNode();
        }

        BeatCounter.Instance.observersList = new List<GameObject>();
        /*limit = BeatCounter.Instance.observersList.Count;
        for (i = 0; i < limit; ++i)
        {
            BeatCounter.Instance.observersList[i] = null;
        }*/
    }

    public void OnPlaceNote()
    {
        
        int randNote = (int)Random.Range(0, (prefabs.Count - 1));
        order++;
        if (order >= prefabs.Count)
        {
            order = 0;
        }
        
        GameObject note = Instantiate(NotePrefab, Cursor.transform.position, Cursor.transform.rotation) as GameObject;
        
        note.transform.parent = sequence.transform;
        MusicNode musicNode = (note.GetComponent<MusicNode>() as MusicNode);
        GameObject prefabNote = prefabs[randNote];
        musicNode.SetClipMarker(prefabNote);
        // musicNode.GetComponent<AudioSource>
        musicNode.SetNodeID(randNote);
        musicNode.SetAnchorID("node"+ musicNodes.Count.ToString());
        musicNode.SetAudioClip(audioClips[randNote]);
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
