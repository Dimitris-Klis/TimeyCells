using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;

public class GIFHandler : MonoBehaviour
{
    public RectTransform viewport;
    public VideoPlayer[] videoPlayers;
    public List<GIFObject> GIFObjects;

    UnityEvent[] onVideoPrepareEvents = new UnityEvent[3];

    private void Start()
    {
        for (int i = 0; i < videoPlayers.Length; i++)
        {
            int temp = i;
            onVideoPrepareEvents[i] = new();
            videoPlayers[i].prepareCompleted += delegate { onVideoPrepareEvents[temp].Invoke(); };
        }
    }

    public Dictionary<GIFObject, VideoPlayer> ActiveGIFs = new();

    public bool VisibleGIF(RectTransform child)
    {
        Vector3 childLocalPos = viewport.InverseTransformPoint(child.position);

        // The viewport pivot is on the top, therefore top is 0 and bottom is -viewportheight.
        float viewportTOP = 0f;
        float viewportBOTTOM = -viewport.rect.height;

        float childTOP = childLocalPos.y + (child.rect.height * (1 - child.pivot.y)); // account for child's pivot Y
        float childBOTTOM = childLocalPos.y - (child.rect.height * child.pivot.y);

        return (childBOTTOM < viewportTOP) && (childTOP > viewportBOTTOM);
    }

    // Update is called once per frame
    void Update()
    {
        List<GIFObject> visibleGIFS = new();
        for (int i = 0; i < GIFObjects.Count; i++)
        {
            if (VisibleGIF(GIFObjects[i].RawSelf.rectTransform))
            {
                visibleGIFS.Add(GIFObjects[i]);
            }
        }
        
        // Using a hashset is apparently more performant.
        HashSet<GIFObject> visibleSet = new HashSet<GIFObject>(visibleGIFS);


        List<GIFObject> noLongerVisibleGIFs = new();
        foreach(KeyValuePair<GIFObject, VideoPlayer> kvp in new Dictionary<GIFObject, VideoPlayer>(ActiveGIFs))
        {
            if (!visibleSet.Contains(kvp.Key))
            {
                kvp.Value.Stop();
                kvp.Key.RawSelf.color = Color.black;
                kvp.Key.RawSelf.texture = null;
                kvp.Value.targetTexture.Release();
                Destroy(kvp.Value.targetTexture);
                noLongerVisibleGIFs.Add(kvp.Key);
            }
        }
        for (int i = 0; i < noLongerVisibleGIFs.Count; i++)
        {
            ActiveGIFs.Remove(noLongerVisibleGIFs[i]);
        }

        for (int i = 0; i < visibleGIFS.Count; i++)
        {
            if (!ActiveGIFs.ContainsKey(visibleGIFS[i]))
            {
                AssignVideoToGIF(visibleGIFS[i]);
            }
        }
    }
    void AssignVideoToGIF(GIFObject gif)
    {
        // Get empty videoPlayer
        VideoPlayer emptyPlayer = null;
        UnityEvent onPrepare = null;
        for (int i = 0; i < videoPlayers.Length; i++)
        {
            if (!ActiveGIFs.ContainsValue(videoPlayers[i]))
            {
                emptyPlayer = videoPlayers[i];
                onPrepare = onVideoPrepareEvents[i];
                break;
            }
        }
        
        if (emptyPlayer == null) return;
        ActiveGIFs[gif] = emptyPlayer;

        
        

        RenderTexture rt = new RenderTexture((int)gif.Clip.width, (int)gif.Clip.height, 0);
        rt.Create();


        emptyPlayer.targetTexture = rt;

        emptyPlayer.clip = gif.Clip;

        

        VideoPlayer vpTemp = emptyPlayer;
        GIFObject gifTemp = gif;
        RenderTexture rtTemp = rt;

        gif.RawSelf.color = Color.black;

        onPrepare.RemoveAllListeners();
        onPrepare.AddListener
        (
            delegate
            {
                vpTemp.Play();
                gifTemp.RawSelf.texture = rtTemp;
                gifTemp.RawSelf.color = Color.white;
            }
        );

        emptyPlayer.Prepare();
    }
}