using System.Collections.Generic;

namespace Benchmark.Data;

public class Tag
{
    public string TagId { get; set; }

    public List<PostTag> Posts { get; set; }
}