using Yi.AspNetCore.Data.Seeding;
using Yi.System.Entities;

namespace Yi.System.DataSeeds;

public class PostDataSeed : IDataSeedContributor, ITransientDependency
{
    private readonly ISqlSugarRepository<PostEntity> _repository;

    public PostDataSeed(ISqlSugarRepository<PostEntity> repository)
    {
        _repository = repository;
    }

    public async Task SeedAsync()
    {
        if (!await _repository.IsAnyAsync(x => true)) await _repository.InsertRangeAsync(GetSeedData());
    }

    public List<PostEntity> GetSeedData()
    {
        var entites = new List<PostEntity>();

        var Post1 = new PostEntity
        {
            PostName = "董事长",
            PostCode = "ceo",
            OrderNum = 100,
            IsDeleted = false
        };
        entites.Add(Post1);

        var Post2 = new PostEntity
        {
            PostName = "项目经理",
            PostCode = "se",
            OrderNum = 100,
            IsDeleted = false
        };
        entites.Add(Post2);

        var Post3 = new PostEntity
        {
            PostName = "人力资源",
            PostCode = "hr",
            OrderNum = 100,
            IsDeleted = false
        };
        entites.Add(Post3);

        var Post4 = new PostEntity
        {
            PostName = "普通员工",
            PostCode = "user",
            OrderNum = 100,
            IsDeleted = false
        };

        entites.Add(Post4);
        return entites;
    }
}