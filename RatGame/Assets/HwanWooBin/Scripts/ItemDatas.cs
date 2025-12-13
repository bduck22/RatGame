using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

[DefaultExecutionOrder(-100)]
public class ItemDatas : MonoBehaviour
{
    public static ItemDatas instance;

    public ItemBase[] items;

    public int PotionIndex;

    public static readonly Regex CSV_SPLIT_REGEX = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");

    [TextArea]
    public string sheetURL;

    public string downloadPath;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        downloadPath = Path.Combine(Application.persistentDataPath, "LocalItemData.csv");
    }

    private static readonly byte[] Utf8Bom = new byte[] { 0xEF, 0xBB, 0xBF };

    public async Task<string> DownloadHashAsync(string url)//서버파일 해시 받기
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            var asyncOperation = www.SendWebRequest();

            while (!asyncOperation.isDone)
            {
                await Task.Yield();
            }

            if (www.result == UnityWebRequest.Result.Success)
            {
                return www.downloadHandler.text.Trim();
            }
            else
            {
                Debug.LogError($"해시 다운로드 실패 ({url}): {www.error}");
                return null;
            }
        }
    }

    public string GetLocalFileHash(string filePath)//로컬파일 해시 추출
    {
        try
        {
            byte[] fileBytes = File.ReadAllBytes(filePath);

            int startIndex = 0;
            if (fileBytes.Length >= 3 && fileBytes[0] == Utf8Bom[0] &&
                fileBytes[1] == Utf8Bom[1] && fileBytes[2] == Utf8Bom[2])
            {
                startIndex = 3;
            }

            string rawContent = Encoding.UTF8.GetString(fileBytes, startIndex, fileBytes.Length - startIndex);

            string unifiedContent = rawContent.Replace("\r", "");

            string finalContent = unifiedContent.Replace("\"", "");

            byte[] hashInputBytes = Encoding.UTF8.GetBytes(finalContent);

            //Debug.Log($"[Unity] 최종 해시 입력 바이트 길이: {hashInputBytes.Length}");

            using (MD5 md5 = MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(hashInputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[Unity] 로컬 해시 계산 오류: {e.Message}");
            return null;
        }
    }

    [ContextMenu("Check Data")]
    public async void CheckUpdate()//최신버전인지 확인
    {
        if (string.IsNullOrEmpty(sheetURL))
        {
            Debug.LogError("URL을 넣어주세요.");
            return;
        }

        string downloadHash = await DownloadHashAsync(sheetURL + "&range=A1:A1");

        string localHash = null;
        if (File.Exists(downloadPath))
        {
            localHash = GetLocalFileHash(downloadPath);
        }


        //Debug.Log(downloadHash + "\n\n\n\n\n\n\n\n" + localHash);
        

        if (localHash == null || localHash != downloadHash)
        {
            Debug.LogWarning("업데이트 필요 업데이트 진행합니다.");

            DownSheet();
        }
        else
        {
            Debug.Log("최신 데이터입니다.");
            LoadData();
        }
    }


    [ContextMenu("Download Local Data")]
    public async void DownSheet()//시트 다운받기
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                string csvData = await client.GetStringAsync(sheetURL + "&range=A2:M");
                if (string.IsNullOrEmpty(csvData))
                {
                    Debug.LogError("시트가 비어있음");
                    return;
                }

                File.WriteAllText(downloadPath, csvData);

#if UNITY_EDITOR
                Debug.Log($"다운로드 완료 '{downloadPath}'");

#endif
                LoadData();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"데이터 다운로드 실패: {e.Message}");
        }
    }

    [ContextMenu("Loading Data")]
    public void LoadData()//데이터 적용하기
    {
        string loadpath = Path.Combine(Application.persistentDataPath, "LocalItemData.csv");

        if (File.Exists(loadpath))
        {
            using (StreamReader reader = new StreamReader(loadpath))
            {
                string[] line = reader.ReadToEnd().Split('\n');

                int length = line.Length;

                for (int i = 0; i < length; i++)
                {
                    if (items[i] != null)
                    {
                        ItemBase item = ReadData.analyData(line[i]);

                        if (PotionIndex == -1&&item.itemType==ItemType.Potion)
                        {
                            PotionIndex = i;
                        }

                        items[i].SetValue(item);
                    }
                }

                AssetDatabase.Refresh();
            }
        }
        else
        {
            Debug.LogError("파일이 존재하지 않습니다.");
        }
    }
}

public enum Herbs
{
    인삼,
    당귀,
    구기자,
    오미자,
    감초,
    생강,
    작약,
    황기,
    대추,
    천궁,
    하수오,
    약초덩어리
}

public enum Level
{
    미상,
    일반,
    희귀,
    전설,
    X
}

public static class ReadData
{
    public static ItemBase analyData(string data)
    {

        string[] datas = ItemDatas.CSV_SPLIT_REGEX.Split(data);

        ItemType type;

        Enum.TryParse<ItemType>(datas[1], out type);

        if (type == ItemType.Potion) //약
        {
            return new PotionData().InitData(datas);
        }
        else // 허브
        {
            return new HerbData().InitData(datas);
        }
    }
}