﻿namespace Assistant.Contracts.Entities;

public class User
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    IList<Coordinate> Coordinates { get; set; } = new List<Coordinate>();
}

