﻿namespace Benchmark.Data;

public class Theme
{
    public int Id { get; set; }

    public int ColorSchemeId { get; set; }

    public ColorScheme ColorScheme { get; set; }
}