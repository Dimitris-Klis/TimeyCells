using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class HelpSection : MonoBehaviour
{
    [SerializeField] TextAsset testtext;
    
    [SerializeField] TableOfContents tableOfContents;

    [SerializeField] TMP_Text H1Text;
    [SerializeField] TMP_Text H2Text;
    [SerializeField] TMP_Text H3Text;
    [SerializeField] TMP_Text NormalText;
    [Space]
    [SerializeField] GIF[] gifs;
    [SerializeField] IMG[] images;
    [SerializeField] OBJ[] objects;

    string ConvertedText;

    [SerializeField] List<GameObject> SpawnedObjects;
    [SerializeField] CenterAndFit ContentLayoutGroup; // This doesn't work! We need to implement a special layout group for the help section!

    [System.Serializable]
    public class HelpMedia
    {
        public string name;
    }

    [System.Serializable]
    class GIF : HelpMedia
    {
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
        public GameObject UIObject;
    }
    public GameObject[] sprs;

    [ContextMenu("Setup Test")]
    void Setup()
    {
        ConvertedText = "";
        tableOfContents.headers.Clear();
        bool startedItalics = false;
        bool startedBold = false;
        bool header = false;
        string[] Lines = testtext.text.Split("\n");

        for(int i =0; i < SpawnedObjects.Count; i++)
        {
            if (!Application.isPlaying)
                DestroyImmediate(SpawnedObjects[i].gameObject);
            else
                Destroy(SpawnedObjects[i].gameObject);
        }
        SpawnedObjects.Clear();

        for (int i = 0; i < Lines.Length; i++)
        {
            string currLine = Lines[i];
            currLine = currLine.Replace("<br/>", "\n");
            currLine = currLine.Replace("<br>", "\n");
            if (currLine.StartsWith("# "))
            {
                header = true;

                currLine = currLine.Remove(0, 2);

                tableOfContents.headers.Add(new(currLine, 0));

                TMP_Text t = Instantiate(H1Text, ContentLayoutGroup.transform);
                SpawnedObjects.Add(t.gameObject);

                t.text = currLine;
            }
            else if (Lines[i].StartsWith("## "))
            {
                header = true;
                currLine = currLine.Remove(0, 3);

                tableOfContents.headers.Add(new(currLine, 1));

                TMP_Text t = Instantiate(H2Text, ContentLayoutGroup.transform);
                SpawnedObjects.Add(t.gameObject);
                
                t.text = currLine;
            }
            else if (Lines[i].StartsWith("### "))
            {
                header = true;
                currLine = currLine.Remove(0, 4);
                tableOfContents.headers.Add(new(currLine, 2));

                TMP_Text t = Instantiate(H3Text, ContentLayoutGroup.transform);
                SpawnedObjects.Add(t.gameObject);

                t.text = currLine;
            }
            
            

            for(int z=0; z < currLine.Length; z++)
            {
                if (Lines[i][z] == '_')
                {
                    if(z-1 >= 0 && Lines[i][z-1] == '\\')
                    {
                        ConvertedText += '_';
                        continue;
                    }
                    if (startedItalics)
                    {
                        ConvertedText += "<\\i>";
                        startedItalics = false;
                    }
                    else
                    {
                        ConvertedText += "<i>";
                        startedItalics = true;
                    }
                    
                    continue;
                }
                if (Lines[i][z] == '*' && z + 1 < Lines[i].Length && Lines[i][z + 1] == '*')
                {
                    if (z - 1 >= 0 && Lines[i][z - 1] == '\\')
                    {
                        ConvertedText += '*';
                        continue;
                    }
                    if (startedBold)
                    {
                        ConvertedText += "<\\b>";
                        startedBold = false;
                    }
                    else
                    {
                        ConvertedText += "<b>";
                        startedBold = true;
                    }

                    continue;
                }
                ConvertedText += currLine[z];
            }
            if(header) ConvertedText += $"</size></b>\n";
        }
        ContentLayoutGroup.UpdateLayoutLate();
        tableOfContents.Setup();
    }
}
