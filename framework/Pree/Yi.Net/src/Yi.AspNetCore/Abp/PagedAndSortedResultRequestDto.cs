using System;
using System.ComponentModel.DataAnnotations;

namespace Volo.Abp.Application.Dtos;

/// <summary>
/// Simply implements <see cref="IPagedAndSortedResultRequest"/>.
/// </summary>
[Serializable]
public class PagedAndSortedResultRequestDto
{
    public virtual string? Sorting { get; set; }

    [Range(0, int.MaxValue)]
    public virtual int SkipCount { get; set; }

    /// <summary>
    /// Default value: 10.
    /// </summary>
    public static int DefaultMaxResultCount { get; set; } = 10;

    /// <summary>
    /// Maximum possible value of the <see cref="MaxResultCount"/>.
    /// Default value: 1,000.
    /// </summary>
    public static int MaxMaxResultCount { get; set; } = 1000;

    /// <summary>
    /// Maximum result count should be returned.
    /// This is generally used to limit result count on paging.
    /// </summary>
    [Range(1, int.MaxValue)]
    public virtual int MaxResultCount { get; set; } = DefaultMaxResultCount;
}