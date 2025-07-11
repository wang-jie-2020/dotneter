using Yi.Framework.Utils;

namespace Yi.System.Domains;

public class Vue3Router : TreeHelper.ITreeModel<Vue3Router>
{
    public string Name { get; set; } = string.Empty;

    public string Path { get; set; } = string.Empty;

    public bool Hidden { get; set; }

    public string Redirect { get; set; } = string.Empty;

    public string Component { get; set; } = string.Empty;

    public bool AlwaysShow { get; set; }

    public Meta Meta { get; set; } = new();

    public Guid Id { get; set; }

    public Guid ParentId { get; set; }

    public int OrderNum { get; set; }

    public List<Vue3Router> Children { get; set; }
}

public class Meta
{
    public string Title { get; set; } = string.Empty;

    public string Icon { get; set; } = string.Empty;

    public bool NoCache { get; set; }

    public string link { get; set; } = string.Empty;
}