using Yi.AspNetCore.Data.Seeding;
using Yi.System.Entities;
using Yi.System.Entities.ValueObjects;

namespace Yi.Admin.DataSeeds;

public class UserDataSeed : IDataSeedContributor, ITransientDependency
{
    private readonly ISqlSugarRepository<UserEntity> _repository;

    public UserDataSeed(ISqlSugarRepository<UserEntity> repository)
    {
        _repository = repository;
    }

    public async Task SeedAsync()
    {
        if (!await _repository.IsAnyAsync(x => true))
        {
            var entities = new List<UserEntity>();
            var user1 = new UserEntity
            {
                Id = Guid.Parse("661DBEB5-79F6-42C6-A295-A13ADA6D505D"),
                Name = "大橙子",
                UserName = "cc",
                Nick = "橙子",
                EncryPassword = new EncryptPasswordValueObject("123456"),
                Email = "454313500@qq.com",
                Phone = 13800000000,
                Sex = SexEnum.Male,
                Address = "深圳",
                Age = 20,
                Introduction = "还有谁？",
                OrderNum = 999,
                Remark = "描述是什么呢？",
                State = true
            };
            user1.BuildPassword();
            entities.Add(user1);

            var user2 = new UserEntity
            {
                Name = "大测试",
                UserName = "test",
                Nick = "测试",
                EncryPassword = new EncryptPasswordValueObject("123456"),
                Email = "454313500@qq.com",
                Phone = 15900000000,
                Sex = SexEnum.Woman,
                Address = "深圳",
                Age = 18,
                Introduction = "还有我！",
                OrderNum = 1,
                Remark = "我没有描述！",
                State = true
            };
            user2.BuildPassword();
            entities.Add(user2);

            var user3 = new UserEntity
            {
                Name = "游客",
                UserName = "guest",
                Nick = "测试",
                EncryPassword = new EncryptPasswordValueObject("123456"),
                Email = "454313500@qq.com",
                Phone = 15900000000,
                Sex = SexEnum.Woman,
                Address = "深圳",
                Age = 18,
                Introduction = "临时游客",
                OrderNum = 1,
                Remark = "懒得创账号",
                State = true
            };
            user3.BuildPassword();
            entities.Add(user3);

            await _repository.InsertRangeAsync(entities);
        }
    }
}