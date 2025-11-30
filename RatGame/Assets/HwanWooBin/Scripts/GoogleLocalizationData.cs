using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class GoogleLocalizationData : ScriptableObject
{
    [Header("파일 이름")]
    public string fileName;
    [TextArea]
    public string jsonData;

    [TextArea(5, 10)]
    public string previewData;

    public Dictionary<int, string> data = new Dictionary<int, string>();
    public ItemBase[] items;


    [ContextMenu("DownLoad Data")]
    public async Task DownloadData()
    {
        if (string.IsNullOrEmpty(jsonData))
        {
            Debug.LogError("데이터가 없는디요???");
            return;
        }

        try
        {
            using (HttpClient client = new HttpClient())
            {
                string csvData = await client.GetStringAsync(jsonData);

                if (string.IsNullOrEmpty(csvData))
                {
                    Debug.LogError("데이터가 없는디요???");
                    return;
                }

                string resourcesPath = Path.Combine(Application.dataPath, "Resources");
                if (!Directory.Exists(resourcesPath))
                {
                    Directory.CreateDirectory(resourcesPath);
                }

                string filePath = Path.Combine(resourcesPath, fileName + ".csv");
                File.WriteAllText(filePath, csvData);

                previewData = File.ReadAllText(filePath);
                AssetDatabase.Refresh();
            }

        }
        catch (System.Exception e)
        {
            Debug.LogError($"데이터 다운로드 실패: {e.Message}");
        }
    }

    [ContextMenu("Load Data")]
    public void LoadData()
    {
        string[] lines = previewData.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            string[] attribute = lines[i].Split(',');
            data[i - 1] = lines[i];


        }

        for (int i = 0; i < items.Length; i++)
        {
            string[] itemdata = data[i].Split(',');
            SetData(i, itemdata);
        }

    }

    public virtual void SetData(int i, string[] itemdata)
    {

        items[i].itemName = itemdata[0];
        //items[i].itemImage = itemdata[1];
        items[i].itemType = (ItemType)System.Enum.Parse(typeof(ItemType), itemdata[2].Trim());
        items[i].Explanation = itemdata[3];
    }

}
