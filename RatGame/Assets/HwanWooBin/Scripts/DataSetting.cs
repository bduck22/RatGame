using System.Collections.Generic;
using UnityEngine;

public class DataSetting : MonoBehaviour
{
    [Header("Google Sheets Localization Data")]
    public GoogleLocalizationData HerbDatas;
    public GoogleLocalizationData PotionDatas;

    public HerbData[] ItemsClass;

    private void Start()
    {
        SettingHerbData();
    }
    public void SettingHerbData()
    {
        string[] datas = HerbDatas.previewData.Split('\n');





    }
}
