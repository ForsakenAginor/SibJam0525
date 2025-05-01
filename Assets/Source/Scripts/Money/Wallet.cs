using System;
using System.Collections.Generic;

public class Wallet
{
    private readonly List<Pickapable> _loot;
    private readonly Resource _money;

    public Wallet(List<Pickapable> loot, Resource money)
    {
        _loot = loot != null ? loot : throw new ArgumentNullException(nameof(loot));
        _money = money != null ? money : throw new ArgumentNullException(nameof(money));

        foreach (var item in _loot)
        {
            item.Pickuped += OnPickupedLoot;
        }
    }

    ~Wallet()
    {
        foreach (var item in _loot)
        {
            item.Pickuped -= OnPickupedLoot;
        }
    }

    private void OnPickupedLoot(Pickapable loot)
    {
        _loot.Remove(loot);
        loot.Pickuped -= OnPickupedLoot;
        _money.Add(loot.Value);
    }
}
