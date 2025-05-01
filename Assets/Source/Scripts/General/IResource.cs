using System;

public interface IResource
{
    int Amount { get; }
    int Maximum { get; }

    event Action ResourcesAmountChanged;
}