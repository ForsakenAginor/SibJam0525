using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public class Bar : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Wallet wallet;

    


    [SerializeField] private Image bar;
    [SerializeField] private float delta;
    private float barValue;
    private float inputValue;
    private float currentValue;

    [SerializeField] private BarType barType;
    private float healthValue;
    private float moneyValue;

    // Update is called once per frame
    private void Update()
    {
        BarUpdate();
        
    }
    private void Start()
    {
        if (playerHealth != null)
        {
            var healthValue = GetHealthValue(playerHealth);
        }
        if (wallet != null)
        {
            var moneyValue = GetMoneyValue(wallet);
        }
    }
    private void InputUpdater()
    {
        if (barType == BarType.Health)
        {

            inputValue = healthValue;
        }
        else if(barType == BarType.Energy)
        {
            //inputValue = energy;
        }
        else if(barType == BarType.Monet)
        {
            inputValue = moneyValue;
        }
    }
    private void BarUpdate()
    {
        currentValue = inputValue / 100.0f;
        if (currentValue > barValue)
            barValue += delta;
        if (currentValue < barValue)
            barValue -= delta;
        if (currentValue < delta)
            barValue = currentValue;
        bar.fillAmount = barValue;

    }
    public enum BarType
    {
        Health, Energy, Monet
    }
    private Resource GetHealthValue(PlayerHealth playerHealth)
    {
       
        Type playerHealthType = typeof(PlayerHealth); 
        FieldInfo fieldInfo = playerHealthType.GetField("_health", BindingFlags.NonPublic | BindingFlags.Instance);
        if (fieldInfo != null)
        {
            Resource health = (Resource)fieldInfo.GetValue(playerHealth);
            return health;
        }

        return null;
    }
    private Resource GetMoneyValue(Wallet wallet)
    {
        // Получаем тип класса PlayerHealth
        Type playerWalletType = typeof(Wallet);

        // Получаем поле _health с помощью рефлексии
        FieldInfo fieldInfo = playerWalletType.GetField("_money", BindingFlags.NonPublic | BindingFlags.Instance);

        if (fieldInfo != null)
        {
            // Получаем значение поля
            Resource money = (Resource)fieldInfo.GetValue(wallet);
            return money;
        }

        return null;
    }
}
