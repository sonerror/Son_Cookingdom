using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AnhPD.FoodStand;
using DG.Tweening;
using sonnv;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
// using UnityEngine.AddressableAssets;
using UnityEngine.Events;
// using UnityEngine.ResourceManagement.AsyncOperations;
using Utilities;
using Random = UnityEngine.Random;

namespace AnhPD.FoodStall
{
  public class CustomerLine : MonoBehaviour
  {
    #region Properties
    [SerializeField] private Transform orderRoot;
    [SerializeField] private OrderTimer timer;
    [SerializeField] private EmojiControl emoji;
    [SerializeField] private CustomerDataArray customerData;
    [SerializeField] private FSCharacterConfigSO _customerConfig;
    [SerializeField] private Transform _customerContainer;
    [SerializeField] private TextMeshProUGUI _numberTex;
    public FoodData data;

    [SerializeField] private bool isUseTimer;

    [SerializeField] private float moveDistance = 5f;
    [SerializeField] private MoveDirection appearDirection = MoveDirection.Up, disappearDirection = MoveDirection.Down;
    [SerializeField] private float appearDuration = 0.5f;
    [SerializeField] private float waitTime = 30f;
    public UnityEvent showOrder, showOrderComplete, hideOrder, onDoneStep, onComplete;
    public Transform targetPos;

    private bool _isChangingCustomer;
    private int _count = -1;
    private Dictionary<int, Customer> _customerDict = new Dictionary<int, Customer>();
    public enum MoveDirection
    {
      Up,
      Down,
      Left,
      Right
    }
    private readonly Dictionary<MoveDirection, Vector2> _moveVectors =
        new Dictionary<MoveDirection, Vector2>()
        {
                { MoveDirection.Up, Vector2.up },
                { MoveDirection.Down, Vector2.down },
                { MoveDirection.Left, Vector2.left },
                { MoveDirection.Right, Vector2.right },
        };

    private int _currentCustomerIndex = -1;
    private int _customerCount;

    #endregion

    private void Awake()
    {
      timer.isUsed = isUseTimer;
      if (!isUseTimer) return;
      timer.OnTimeout = () =>
      {
        _customerDict[_currentCustomerIndex].ChangeState(Customer.State.Unhappy);
      };
      timer.OnTimeOutComplete = () => NextCustomer();
    }

    #region customer

    [Button]
    public void NextCustomer(float delay = 0f)
    {
      if (_currentCustomerIndex >= 0)
      {
        HideCurrentCustomer(() => ShowRandomCustomer(), delay);
      }
      else ShowRandomCustomer(delay);
    }
    private async void ShowRandomCustomer(float delay = 0)
    {
      try
      {
        await Task.Delay((int)delay * 1000);
        _count++;
        if (_count >= customerData.customers.Count) return;
        CustomerData cusData = customerData.customers[_count];
        waitTime = cusData.waitTime / 1000f;
        data.SetOrderType((OrderType)cusData.type);

        data.RandomOrder();

        // RandomCustomer();

        if (!_customerDict.ContainsKey(_currentCustomerIndex))
          await LoadCustomer(_currentCustomerIndex);
        Customer customer = _customerDict[_currentCustomerIndex];

        Debug.Log($"Random Customer Index: {_currentCustomerIndex}");

        Vector2 vector = _moveVectors[appearDirection];
        customer.gameObject.SetActive(true);
        customer.LocalMoveInstant(-vector * moveDistance);

        customer.LocalMove(vector * moveDistance, appearDuration, () =>
        {
          ShowOrder();
          customer.ChangeState(Customer.State.Talk);
        });
      }
      catch (Exception e)
      {
        Debug.Log(e);
      }
    }

    private void HideCurrentCustomer(Action callback = null, float delay = 0)
    {
      if (_currentCustomerIndex < 0) return;
      HideOrder();
      Customer customer = _customerDict[_currentCustomerIndex];
      Vector2 vector = _moveVectors[disappearDirection];
      customer.LocalMove(vector * moveDistance, appearDuration, () =>
      {
        callback?.Invoke();
        customer.gameObject.SetActive(false);
      }, .3f + delay);
    }

    public void SetCustomerData(CustomerDataArray customerDataArray)
    {
      customerData = customerDataArray;
      _numberTex.text = _customerCount + " / " + customerData.customers.Count;
    }

    private async Task LoadCustomer(int id)
    {
      //if (_opLoadCustomer.IsValid())
      //{
      //    Addressables.Release(_opLoadCustomer);
      //    _opLoadCustomer = default;
      //}
      // AssetReference assetRef = _customerConfig.GetAssetReferenceById(id);
      // if (assetRef.RuntimeKeyIsValid())
      // {
      //   _opLoadCustomer = Addressables.LoadAssetAsync<GameObject>(assetRef);
      //   _usedOps.Add(_opLoadCustomer);
      //   await _opLoadCustomer.Task;
      //   GameObject prefab = _opLoadCustomer.Result;
      //   Customer customer = Instantiate(prefab, _customerContainer).GetComponent<Customer>();
      //   _customerDict.Add(id, customer);
      // }
      // else Debug.LogError("Customer id " + id + " not found");
    }
    #endregion

    #region order

    private void ShowOrder()
    {
      timer.Init(waitTime);

      showOrder?.Invoke();
      orderRoot.DOScale(1, .2f).SetEase(Ease.OutBack).OnComplete(() =>
      {
        showOrderComplete?.Invoke();
        timer.StartWaiting();
      });
    }
    private void HideOrder()
    {
      hideOrder?.Invoke();
      orderRoot.DOScale(0, .2f).SetEase(Ease.InBack);
    }

    public void CompareOrder(FoodData foodData)
    {
      if (_isChangingCustomer) return;
      bool isCorrect = data.Compare(foodData);

      if (isCorrect && timer.IsSatisfied)
      {
        OnCorrect();
      }
      else
      {
        OnIncorrect();
      }
    }

    private void OnCorrect()
    {
      emoji.ShowPositive();
      timer.StopWaiting();

      _customerCount++;
      _numberTex.text = _customerCount + " / " + customerData.customers.Count;

      HideOrder();

      _isChangingCustomer = true;
      _customerDict[_currentCustomerIndex].ChangeState(Customer.State.Happy, next);

      void next()
      {
        _isChangingCustomer = false;
        if (_customerCount >= customerData.customers.Count)
        {
          HideCurrentCustomer();
          onComplete?.Invoke();
        }
        else
        {
          onDoneStep?.Invoke();
          NextCustomer();
        }
      }
    }

    private void OnIncorrect()
    {
      emoji.ShowNegative();
      timer.PlusTime(15f);
      _customerDict[_currentCustomerIndex].ChangeState(Customer.State.Unhappy);
    }
    #endregion

#if UNITY_EDITOR
    public async Task ShowTest()
    {
      await LoadCustomer(0);
      _customerDict[0].LocalMoveInstant(Vector2.zero);
      orderRoot.localScale = Vector3.one;
    }

    public void HideTest()
    {
      _customerDict[0]?.gameObject.SetActive(false);
      orderRoot.localScale = Vector3.zero;
    }
#endif
  }

}
