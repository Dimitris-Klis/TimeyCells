using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TabHandler : MonoBehaviour
{
    [System.Serializable]
    public class Tab
    {
        public Button TabButton;
        public GameObject TabObject;
    }
    public Tab[] tabs;

    public void SelectTab(int index)
    {
        if (index < 0 || index >= tabs.Length) return;
        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].TabObject.SetActive(index == i);
            tabs[i].TabButton.interactable = index != i;
        }
    }
    private void Start()
    {
        SelectTab(0);
        for (int i = 0; i < tabs.Length; i++)
        {
            tabs[i].TabButton.onClick.RemoveAllListeners();

            int tab = i;
            tabs[i].TabButton.onClick.AddListener(delegate { SelectTab(tab); });
        }
    }
}