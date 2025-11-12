using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using AnhPD.FoodStall;
using Newtonsoft.Json;
// using Newtonsoft.Json.Linq;
using UnityEditor;

namespace AnhPD.FoodStand
{
  public class JsonDataHolder : MonoBehaviour
  {
    [SerializeField] private TextAsset jsonFile;

    [Title("Data")]
    public FoodStallData data;

    private Dictionary<string, IngredientData> _ingredientDataDict = new Dictionary<string, IngredientData>();

    #region Save & Load

    public void LoadJsonByString(string jsonString)
    {
      data = JsonUtility.FromJson<FoodStallData>(jsonString);
      data.Init();
      ConvertData();
    }

    [VerticalGroup("button")]
    [Button]
    public void LoadJson()
    {
      if (!jsonFile)
      {
        Debug.LogWarning("Chưa gán file JSON!");
        return;
      }

      try
      {
        data = JsonConvert.DeserializeObject<FoodStallData>(jsonFile.text);
        data.Init();

        Debug.Log("Load JSON thành công!");
        ConvertData();
      }
      catch (System.Exception e)
      {
        Debug.LogError($"Lỗi load JSON: {e.Message}\n{e.StackTrace}");
      }
    }
    #endregion

    private void ConvertData()
    {
      _ingredientDataDict = new Dictionary<string, IngredientData>(StringComparer.OrdinalIgnoreCase);

      void AddList(List<IngredientData> list)
      {
        if (list == null) return;
        foreach (var ing in list)
        {
          if (string.IsNullOrEmpty(ing.name)) continue;
          if (!_ingredientDataDict.TryAdd(ing.name, ing))
          {
            Debug.LogWarning($"Duplicate IngredientData name: {ing.name}");
          }
        }
      }

      AddList(data.toppings.veg);
      AddList(data.toppings.meat);
      AddList(data.toppings.@base);
      AddList(data.toppings.spice);
      AddList(data.toppings.decor);
    }

    public IngredientData GetIngredientData(string nameId)
    {
      if (_ingredientDataDict.Count < 1) ConvertData();
      return _ingredientDataDict[nameId];
    }
  }

  [Serializable, HideLabel]
  public class FoodStallData
  {
    public float cooldownSpeed = 1f;
    public float customerWaitSpeed = 1f;

    [JsonProperty("topping")]
    public List<Dictionary<string, List<IngredientData>>> ToppingRaw;

    public ToppingData toppings = new ToppingData();

    public List<CustomerDataArray> customers = new List<CustomerDataArray>();

    public void Init()
    {
      toppings = new ToppingData();

      if (ToppingRaw != null)
      {
        foreach (var dict in ToppingRaw)
        {
          foreach (var kvp in dict)
          {
            switch (kvp.Key)
            {
              case "veg": toppings.veg = kvp.Value; break;
              case "meat": toppings.meat = kvp.Value; break;
              case "base": toppings.@base = kvp.Value; break;
              case "spice": toppings.spice = kvp.Value; break;
              case "decor": toppings.decor = kvp.Value; break;
            }
          }
        }
      }

      toppings.Init(); // đảm bảo null-safe
    }
  }
  [Serializable]
  public class ToppingData
  {
    public List<IngredientData> veg;
    public List<IngredientData> meat;
    public List<IngredientData> @base;
    public List<IngredientData> spice;
    public List<IngredientData> decor;

    public void Init()
    {
      veg ??= new List<IngredientData>();
      meat ??= new List<IngredientData>();
      @base ??= new List<IngredientData>();
      spice ??= new List<IngredientData>();
      decor ??= new List<IngredientData>();
    }
  }
  [Serializable]
  public class IngredientData
  {
    public string name;
    public float cost;
    public float cd;
  }

  [Serializable]
  public class CustomerData
  {
    public float waitTime = 60000;
    public OrderType type;
  }

  [Serializable]
  public class CustomerDataArray
  {
    public List<CustomerData> customers = new List<CustomerData>();
  }
}