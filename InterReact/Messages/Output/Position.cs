﻿namespace InterReact;

public sealed class Position
{
    public string Account { get; } = "";
    public Contract Contract { get; }
    public double Quantity { get; }
    public double AverageCost { get; }

    internal Position(ResponseReader r)
    {
        r.RequireMessageVersion(3);
        Account = r.ReadString();
        Contract = new(r);
        Quantity = r.ReadDouble();
        AverageCost = r.ReadDouble();
    }
}

public sealed class PositionEnd
{
    internal PositionEnd(ResponseReader r) => r.IgnoreMessageVersion();
}
