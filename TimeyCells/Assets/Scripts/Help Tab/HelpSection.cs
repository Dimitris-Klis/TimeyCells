using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using TMPro;
using UnityEngine.UI;

public class HelpSection : MonoBehaviour
{
    [SerializeField] TableOfContents tableOfContents;

    [SerializeField] TMP_Text H1Text;
    [SerializeField] TMP_Text H2Text;
    [SerializeField] TMP_Text H3Text;
    [SerializeField] TMP_Text NormalText;
    [Space]
    [SerializeField] Image UIImage;
    [SerializeField] GIFObject GifImage;
    [Space]
    [SerializeField] GIF[] gifs;
    [SerializeField] IMG[] images;
    [SerializeField] OBJ[] objects;

    [SerializeField] List<GameObject> SpawnedObjects = new();
    [SerializeField] HelpLayoutGroup ContentLayoutGroup; // This doesn't work! We need to implement a special layout group for the help section!
    [SerializeField] ScrollRect scrollRect; // Assign your ScrollRect
    [SerializeField] GIFHandler GIFHandler;

    [System.Serializable]
    public class HelpMedia
    {
        public string name;
    }
    
    [System.Serializable]
    class GIF : HelpMedia
    {
        public float PixelsPerUnit;
        public VideoClip GIFClip;
    }
    [System.Serializable]
    class IMG : HelpMedia
    {
        public Sprite IMGSprite;
    }
    [System.Serializable]
    class OBJ : HelpMedia
    {
        public RectTransform UIObject;
    }

    GIF FindGIF(string name)
    {
        for (int i = 0; i < gifs.Length; i++)
        {
            if (gifs[i].name == name) return gifs[i];
        }
        Debug.LogWarning($"Error: No GIF with name: '{name}' exists");
        return null;
    }
    IMG FindIMG(string name)
    {
        for (int i = 0; i < images.Length; i++)
        {
            if (images[i].name == name) return images[i];
        }
        Debug.LogWarning($"Error: No IMG with name: '{name}' exists");
        return null;
    }
    OBJ FindOBJ(string name)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].name == name) return objects[i];
        }
        Debug.LogWarning($"Error: No OBJ with name: '{name}' exists");
        return null;
    }
    

    public void ScrollToTarget(RectTransform target)
    {
        Canvas.ForceUpdateCanvases(); // Make sure layout is updated

        // Convert the target position to the ScrollRect's content space
        Vector2 contentPos = scrollRect.content.anchoredPosition;
        Vector2 targetLocalPos = (Vector2)scrollRect.content.InverseTransformPoint(scrollRect.viewport.position) -
                                 (Vector2)scrollRect.content.InverseTransformPoint(target.position);

        // Update content position
        scrollRect.content.anchoredPosition = contentPos + Vector2.up * (targetLocalPos.y- target.sizeDelta.y/2);
    }

    [ContextMenu("Setup Test")]
    public void Setup()
    {
        tableOfContents.headers.Clear();
        bool startedItalics = false;
        bool startedBold = false;
        bool startedMark = false;
        string docs = LocalizationSystem.instance.GetText(this.gameObject.name, "HELP_SECTION");
        string[] Lines = docs.Split("\n");

        for(int i =0; i < SpawnedObjects.Count; i++)
        {
            if (!Application.isPlaying)
                DestroyImmediate(SpawnedObjects[i].gameObject);
            else
                Destroy(SpawnedObjects[i].gameObject);
        }
        SpawnedObjects.Clear();
        ContentLayoutGroup.children.Clear();
        GIFHandler.GIFObjects.Clear();
        GIFHandler.ActiveGIFs.Clear();
        for (int i = 0; i < Lines.Length; i++)
        {
            string currLine = Lines[i];
            TMP_Text t = null;
            currLine = currLine.Replace("\r", "");
            currLine = currLine.Replace("<br/>", "\n<size=0%>a</size>");
            currLine = currLine.Replace("<br>", "\n<size=0%>a</size>");
            currLine = currLine.Replace("!{THEMECOUNT}", SaveManager.instance.Stylizer.CountBuiltInThemes().ToString());

            if (currLine.StartsWith("# "))
            {

                currLine = currLine.Remove(0, 2);

                t = Instantiate(H1Text, ContentLayoutGroup.transform);
                tableOfContents.headers.Add(new(currLine.Replace(":", ""), 0, t.rectTransform));
            }
            else if (Lines[i].StartsWith("## "))
            {
                currLine = currLine.Remove(0, 3);

                t = Instantiate(H2Text, ContentLayoutGroup.transform);
                tableOfContents.headers.Add(new(currLine.Replace(":", ""), 1, t.rectTransform));
            }
            else if (Lines[i].StartsWith("### "))
            {
                currLine = currLine.Remove(0, 4);
                

                t = Instantiate(H3Text, ContentLayoutGroup.transform);
                tableOfContents.headers.Add(new(currLine.Replace(":", ""), 2, t.rectTransform) );
            }
            else if (Lines[i].StartsWith("!IMG["))
            {
                string name = "";
                for (int j = 5; j < currLine.Length; j++)
                {
                    if (currLine[j] == ']') break;
                    name += currLine[j];
                }

                Image newimg = Instantiate(UIImage, ContentLayoutGroup.transform);
                Sprite sp = FindIMG(name).IMGSprite;
                newimg.sprite = sp;
                newimg.rectTransform.sizeDelta = new Vector2(sp.texture.width, sp.texture.height) / (sp.pixelsPerUnit / 100);
                ContentLayoutGroup.children.Add(newimg.rectTransform);
                SpawnedObjects.Add(newimg.gameObject);
            }
            else if (currLine.StartsWith("!GIF["))
            {
                string name = "";
                for (int j = 5; j < currLine.Length; j++)
                {
                    if (currLine[j] == ']') break;
                    name += currLine[j];
                }

                // TO DO: INSTEAD OF RAW IMAGE, INSTANTIATE A GIF SCRIPT THAT LOADS THE VIDEO WHEN THE IMAGE IS IN VIEW.
                GIFObject GifObj = Instantiate(GifImage, ContentLayoutGroup.transform);
                GIF gif = FindGIF(name);
                GifObj.Clip = gif.GIFClip;
                GifObj.RawSelf.rectTransform.sizeDelta = new Vector2(gif.GIFClip.width, gif.GIFClip.height) / (gif.PixelsPerUnit / 100);
                GIFHandler.GIFObjects.Add(GifObj);
                ContentLayoutGroup.children.Add(GifObj.RawSelf.rectTransform);
                SpawnedObjects.Add(GifObj.gameObject);
            }
            else if (Lines[i].StartsWith("!OBJ["))
            {
                string name = "";
                for (int j = 5; j < currLine.Length; j++)
                {
                    if (currLine[j] == ']') break;
                    name += currLine[j];
                }
                RectTransform r = Instantiate(FindOBJ(name).UIObject, ContentLayoutGroup.transform);
                ContentLayoutGroup.children.Add(r);
                SpawnedObjects.Add(r.gameObject);
            }
            else
            {
                t = Instantiate(NormalText, ContentLayoutGroup.transform);
            }
            string finaltext = "";

            for(int z=0; z < currLine.Length; z++)
            {
                if (currLine[z]=='-' && z==0)
                {
                    finaltext += "\u2022";
                    continue;
                }
                if (currLine[z] == '_')
                {
                    if(z-1 >= 0 && currLine[z-1] == '\\')
                    {
                        finaltext.Remove(finaltext.Length-2);
                        finaltext += '_';
                        continue;
                    }
                    if (startedItalics)
                    {
                        finaltext += "</i>";
                        startedItalics = false;
                    }
                    else
                    {
                        finaltext += "<i>";
                        startedItalics = true;
                    }
                    
                    continue;
                }
                if (currLine[z] == '*' && z + 1 < currLine.Length && currLine[z + 1] == '*')
                {
                    if (z - 1 >= 0 && currLine[z - 1] == '\\')
                    {
                        finaltext.Remove(finaltext.Length - 2);
                        finaltext += '*';
                        continue;
                    }
                    if (startedBold)
                    {
                        finaltext += "</b>";
                        startedBold = false;
                    }
                    else
                    {
                        finaltext += "<b>";
                        startedBold = true;
                    }
                    z++;
                    continue;
                }
                if (currLine[z] == '`')
                {
                    if (z - 1 >= 0 && currLine[z - 1] == '\\')
                    {
                        finaltext.Remove(finaltext.Length - 2);
                        finaltext += '`';
                        continue;
                    }
                    if (startedMark)
                    {
                        finaltext += "</mark>";
                        startedMark = false;
                    }
                    else
                    {
                        finaltext += "<mark=#ffffff80>";
                        startedMark = true;
                    }
                    continue;
                }
                finaltext += currLine[z];
            }
            if (t == null) continue;
            t.text = finaltext;
            ContentLayoutGroup.children.Add(t.rectTransform);
            SpawnedObjects.Add(t.gameObject);
        }
        ContentLayoutGroup.UpdateLayout();
        tableOfContents.Setup();
        SaveManager.instance.Stylizer.GetElements();
        SaveManager.instance.Stylizer.ApplyCurrentTheme();
    }
}