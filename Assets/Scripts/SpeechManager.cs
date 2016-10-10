using HoloToolkit.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class SpeechManager : MonoBehaviour
{
    KeywordRecognizer keywordRecognizer = null;
    Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    // Use this for initialization
    void Start()
    {


        keywords.Add("Place Note", () =>
        {
            BroadcastMessage("OnPlaceNote");
            /*var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                // Call the OnDrop method on just the focused object.
                focusObject.SendMessage("OnMoveSomewhere");
            }*/
        });

        keywords.Add("Move", () =>
        {
            SequenceManager.Instance.Move();
            /*var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                // Call the OnDrop method on just the focused object.
                focusObject.SendMessage("OnMove");
            }*/
        });

        keywords.Add("Drop", () =>
        {
            SequenceManager.Instance.Drop();
        });

        keywords.Add("Remove", () =>
        {
           // SequenceManager.Instance.RemoveNode();
        });

        keywords.Add("Stop", () =>
        {

            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                // Call the OnDrop method on just the focused object.
                focusObject.SendMessage("OnStop");
            }
        });

        keywords.Add("Change", () =>
        {
            SequenceManager.Instance.Change();
        });

        keywords.Add("Clone", () =>
        {
            SequenceManager.Instance.Clone();
        });


        keywords.Add("Play", () =>
        {

            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                // Call the OnDrop method on just the focused object.
                focusObject.SendMessage("OnPlay");
            }
        });


        keywords.Add("Lock", () =>
        {

            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                // Call the OnDrop method on just the focused object.
                focusObject.SendMessage("OnLock");
            }
        });

        keywords.Add("Unlock", () =>
        {

            var focusObject = GazeManager.Instance.FocusedObject;
            if (focusObject != null)
            {
                // Call the OnDrop method on just the focused object.
                focusObject.SendMessage("OnUnlock");
            }
        });

        keywords.Add("Clear All Nodes", () =>
        {

            SequenceManager.Instance.OnRemoveAllNodes();
        });


        // Tell the KeywordRecognizer about our keywords.
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());

        // Register a callback for the KeywordRecognizer and start recognizing!
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }
}