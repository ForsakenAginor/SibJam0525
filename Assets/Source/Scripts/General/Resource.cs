using System;

public class Resource : IResource
{
    private readonly int _maximum;
    private int _amount;

    public Resource(int amount)
    {
        _amount = amount >= 0 ? amount : throw new ArgumentOutOfRangeException(nameof(amount));
        _maximum = amount;
    }

    public Resource(int amount, int maximum)
    {
        _amount = amount >= 0 ? amount : throw new ArgumentOutOfRangeException(nameof(amount));
        _maximum = maximum >= 0  && maximum >= amount ? maximum : throw new ArgumentOutOfRangeException(nameof(maximum));
    }

    public event Action ResourcesAmountChanged;
    public event Action ResourceOver;

    public int Amount => _amount;

    public int Maximum => _maximum;

    public void Add(int amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount));

        int temp = _amount + amount;
        _amount = _maximum <= temp ? _maximum : temp;
        ResourcesAmountChanged?.Invoke();
    }

    public void Spent(int amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount));

        _amount -= amount;
        ResourcesAmountChanged?.Invoke();

        if (_amount <= 0)
            ResourceOver?.Invoke();
    }
}