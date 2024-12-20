﻿namespace InterReact;

public sealed class AccountValue
{
    public string Account { get; }
    public string Key { get; }
    public string Value { get; } = "0";
    public string Currency { get; }
    internal AccountValue(ResponseReader r)
    {
        r.RequireMessageVersion(2);
        Key = r.ReadString();
        Value = r.ReadString();
        Currency = r.ReadString();
        Account = r.ReadString();
    }
}

public sealed class PortfolioValue
{
    public string Account { get; }
    public Contract Contract { get; }
    public decimal Position { get; }
    public double MarketPrice { get; }
    public double MarketValue { get; }
    public double AverageCost { get; }
    public double UnrealizedPnl { get; }
    public double RealizedPnl { get; }
    internal PortfolioValue(ResponseReader r)
    {
        r.RequireMessageVersion(8);
        Contract = new(r, includeExchange:false);
        Position = r.ReadDecimal();
        MarketPrice = r.ReadDouble();
        MarketValue = r.ReadDouble();
        AverageCost = r.ReadDouble();
        UnrealizedPnl = r.ReadDouble();
        RealizedPnl = r.ReadDouble();
        Account = r.ReadString();
    }
}

public sealed class AccountUpdateTime
{
    public string Time { get; }
    internal AccountUpdateTime(ResponseReader r)
    {
        r.IgnoreMessageVersion();
        Time = r.ReadString();
    }
}

public sealed class AccountUpdateEnd
{
    public string Account { get; }
    internal AccountUpdateEnd(ResponseReader r)
    {
        r.IgnoreMessageVersion();
        Account = r.ReadString();
    }
}
