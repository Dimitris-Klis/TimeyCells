using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class PhotoManager : MonoBehaviour
{
    public Camera PhotoCamera;
    public Canvas PhotoCanvas;
    public RectTransform Content;
    public RawImage rawImg;
    [Space]
    public TMP_Dropdown FormatDropdown;
    public CustomGridLayoutGroup Grid;
    public GameObject CornerPiecePrefab;
    
    public WeekDayObject PhotoTimePrefab_;
    public CellInfo CellPrefab;
    [Space]
    public Button AndroidShareButton;
    public Button WindowsCopyButton;

    List<WeekDayObject> PhotoTimes = new();
    List<CellInfo> PhotoCells = new();
    GameObject CornerPiece;

    Texture2D PhotoTex;

    private void Start()
    {
        // Enabling the correct buttons, depending on the platform.
        #if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
                WindowsCopyButton.gameObject.SetActive(true);
                AndroidShareButton.gameObject.SetActive(false);
        #elif UNITY_ANDROID
                WindowsCopyButton.gameObject.SetActive(false);
                AndroidShareButton.gameObject.SetActive(true);
        #else
                WindowsCopyButton.gameObject.SetActive(false);
                AndroidShareButton.gameObject.SetActive(false);
        #endif
    }
    void SetupCells()
    {
        for (int i = 0; i < PhotoTimes.Count; i++)
        {
            Destroy(PhotoTimes[i].gameObject);
        }

        for (int i = 0; i < PhotoCells.Count; i++)
        {
            Destroy(PhotoCells[i].gameObject);
        }

        PhotoTimes.Clear();
        PhotoCells.Clear();

        if (CornerPiece != null)
            Destroy(CornerPiece.gameObject);

        Grid.MultirowIndexes.Clear();

        StartCoroutine(ContinueSetup());

    }
    IEnumerator ContinueSetup()
    {
        yield return new WaitForEndOfFrame();
        CornerPiece = Instantiate(CornerPiecePrefab, Content);
        CornerPiece.transform.position = Vector3.zero;
        // Info
        if (FormatDropdown.value == 0)
        {
            Grid.Size.x = 100;
            Grid.Size.y = 100;

            Grid.Rows = DayTimeManager.instance.Grid.Rows + 1;
            Grid.Columns = DayTimeManager.instance.Grid.Columns + 1;

            int num = 1;

            int index = 1;
            for (int i = 0; i < DayTimeManager.instance.WeekDays.Count; i++)
            {
                WeekDayObject p = Instantiate(PhotoTimePrefab_, Content);
                p.WeekDayName.text = DayTimeManager.instance.WeekDays[i].DayName;
                PhotoTimes.Add(p);
                index++;
            }
            
            for (int x = 0; x < Grid.Columns - 1; x++)
            {
                WeekDayObject p = Instantiate(PhotoTimePrefab_, Content);
                PhotoTimes.Add(p);

                if (DayTimeManager.instance.TimeLabels[x].IsCustomLabel)
                {
                    p.WeekDayName.text = DayTimeManager.instance.TimeLabels[x].CustomLabelName;
                    if (DayTimeManager.instance.TimeLabels[x].CountAsIndex)
                    {
                        num++;
                    }
                }
                else
                {
                    p.WeekDayName.text = num.ToString();
                    num++;
                }
                index++;

                for (int y = 0; y < DayTimeManager.instance.Grid.ColumnsList[x].Children.Count; y++)
                {
                    CellInfo c = Instantiate(CellPrefab, Content);
                    PhotoCells.Add(c);
                    CellInfo info = DayTimeManager.instance.Grid.ColumnsList[x].Children[y].Info;

                    // Casting Cell Info into cell info data so we can set up everything.
                    c.SetupSelf(new(info));

                    c.UpdateUI();
                    
                    // TO DO: FIX MULTIROWS IN CUSTOM GRID.
                    if (DayTimeManager.instance.Grid.ColumnsList[x].IsMultirow)
                        Grid.MultirowIndexes.Add(index);
                    index++;
                }
            }
        }
        // Times
        else if (FormatDropdown.value == 1)
        {
            Grid.Size.x = 200;
            Grid.Size.y = 100;
            
            Grid.Rows = DayTimeManager.instance.Grid.Columns+1;
            Grid.Columns = DayTimeManager.instance.Grid.Rows+1;
            
            int num = 1;
            
            for (int i = 0; i < DayTimeManager.instance.TimeLabels.Count; i++)
            {
                WeekDayObject p = Instantiate(PhotoTimePrefab_, Content);
                PhotoTimes.Add(p);
                if (DayTimeManager.instance.TimeLabels[i].IsCustomLabel)
                {
                    p.WeekDayName.text = DayTimeManager.instance.TimeLabels[i].CustomLabelName;
                    if (DayTimeManager.instance.TimeLabels[i].CountAsIndex)
                    {
                        num++;
                    }
                    continue;
                }
                
                p.WeekDayName.text = num.ToString();
                num++;
            }
            for(int x = 0; x < Grid.Columns-1; x++)
            {
                WeekDayObject p = Instantiate(PhotoTimePrefab_, Content);
                
                p.WeekDayName.text = DayTimeManager.instance.WeekDays[x].DayName;
                PhotoTimes.Add(p);
                for (int y = 0; y < Grid.Rows-1; y++)
                {
                    WeekDayObject p2 = Instantiate(PhotoTimePrefab_, Content);
                    p2.transform.Find("Background (Image)").tag = "Styled/Background";
                    p2.WeekDayName.text = DayTimeManager.instance.FormatTime(DayTimeManager.instance.GetCellStartTime(y, x));
                    PhotoTimes.Add(p2);
                }
            }
        }
        SaveManager.instance.Stylizer.GetElements();
        SaveManager.instance.Stylizer.RefreshPreset();
        Grid.UpdateLayout();
        Snap();
    }

    [UnityEngine.ContextMenu("Setup!")]
    public void Setup()
    {
        SetupCells();
    }
    public void Snap()
    {
        float scaleFactor = PhotoCanvas.scaleFactor;
        int width = Mathf.CeilToInt(Content.sizeDelta.x * scaleFactor);
        int height = Mathf.CeilToInt(Content.sizeDelta.y * scaleFactor);

        PhotoCamera.gameObject.SetActive(true);

        RenderTexture rt = new(width, height, 24);
        PhotoCamera.targetTexture = rt;

        RenderTexture.active = rt;

        PhotoCamera.Render();

        // Instead of directly using RenderTexture, we save a snapshot of that renderTexture. This is more efficient.
        PhotoTex = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
        PhotoTex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        PhotoTex.Apply();

        RenderTexture.active = null;
        rawImg.texture = PhotoTex;
        rawImg.rectTransform.sizeDelta = Content.sizeDelta * 0.25f;

        PhotoCamera.targetTexture = null;
        rt.Release();
        rt = null;

        PhotoCamera.gameObject.SetActive(false);
    }


    [ContextMenu("Test Copy!")]
    public void CopyPhotoToClipboard()
    {
        #if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN

            Setup();

            // Temporarily storing the Texture2D as a png in the cache path.
            string imagePath = Path.Combine(Application.temporaryCachePath, "capture.png");
            File.WriteAllBytes(imagePath, PhotoTex.EncodeToPNG());

            // Running a windows powershell script (located in streamingassets), which takes the png and copies it to the clipboard.
            string scriptPath = Path.Combine(Application.streamingAssetsPath, "CopyToClipboard.ps1");
            string args = $"-ExecutionPolicy Bypass -File \"{scriptPath}\" -imgPath \"{imagePath}\"";

            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo()
            {
                FileName = "powershell.exe",
                Arguments = args,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            System.Diagnostics.Process.Start(psi);

        #endif
    }

    public void SharePhoto()
    {
        #if UNITY_ANDROID
            Setup();

            // Temporarily storing the Texture2D as a png in the cache path.
            string imagePath = Path.Combine(Application.temporaryCachePath, "capture.png");
            File.WriteAllBytes(imagePath, PhotoTex.EncodeToPNG());

            // Open android share sheet
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            using (AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent"))
            using (AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent"))
            {
                intent.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
                intent.Call<AndroidJavaObject>("setType", "image/png");

                // Get FileProvider URI
                using (AndroidJavaClass fileProvider = new AndroidJavaClass("androidx.core.content.FileProvider"))
                {
                    AndroidJavaObject file = new AndroidJavaObject("java.io.File", imagePath);
                    AndroidJavaObject uri = fileProvider.CallStatic<AndroidJavaObject>(
                        "getUriForFile",
                        activity,
                        activity.Call<string>("getPackageName") + ".fileprovider",
                        file
                    );
                 
                    intent.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uri);
                    intent.Call<AndroidJavaObject>("addFlags", 1 << 1); // FLAG_GRANT_READ_URI_PERMISSION

                    // Correct way to call createChooser
                    using (AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intent, "Share Image"))
                    {
                        activity.Call("startActivity", chooser);
                    }
                }
            }
        #endif
    }

}