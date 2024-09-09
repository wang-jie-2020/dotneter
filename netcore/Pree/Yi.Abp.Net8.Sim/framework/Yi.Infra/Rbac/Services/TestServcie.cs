using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;
using Yi.Framework.SqlSugarCore.Abstractions;
using Yi.Infra.Rbac.Entities;
using Yi.Infra.Rbac.Options;

namespace Yi.Infra.Rbac.Services;

/// <summary>
///     测试文档控制器
/// </summary>
public class TestServcie : ApplicationService
{
    private readonly IRepository<StudentEntity> _repository;
    private readonly ISqlSugarRepository<StudentEntity> _sqlsugarRepository;
    private readonly IUnitOfWorkManager _unitOfWork;

    public TestServcie(IRepository<StudentEntity> repository, IUnitOfWorkManager unitOfWork,
        ISqlSugarRepository<StudentEntity> sqlsugarRepository, IRepository<StudentEntity, Guid> repository2)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _sqlsugarRepository = sqlsugarRepository;
    }

    public IOptions<JwtOptions> options { get; set; }

    /// <summary>
    ///     你好，多线程
    /// </summary>
    /// <returns></returns>
    public async Task<string> GetTaskTest()
    {
        var tasks = Enumerable.Range(0, 2).Select(x =>
        {
            return Task.Run(async () =>
            {
                using (var uow = _unitOfWork.Begin(true))
                {
                    //  await _repository.GetListAsync();
                    await _sqlsugarRepository._DbQueryable.ToListAsync();
                    await uow.CompleteAsync();
                }
            });
        }).ToList();

        await Task.WhenAll(tasks);
        return "你哈";
    }

    [Authorize]
    public async Task<List<StudentEntity>> GetTest()
    {
        //using (var uow = _unitOfWork.Begin(true))
        //{
        var data = await _repository.GetListAsync();
        var data2 = await _repository.GetListAsync();
        //await uow.CompleteAsync();
        return data;

        //}
    }

    //[UnitOfWork]
    public async Task<StudentEntity> PostTest()
    {
        //using (var uow = _unitOfWork.Begin())
        //{
        var stu = new StudentEntity { Name = $"{DateTime.Now.ToString()}你好" };

        var data = await _repository.InsertAsync(stu);
        //await uow.CompleteAsync();
        return data;
        //}
    }

    public async Task<StudentEntity> PostError()
    {
        throw new ApplicationException();
    }

    public async Task<StudentEntity> PostUserError()
    {
        throw new UserFriendlyException("直接爆炸");
    }

    public string Login()
    {
        var data = options.Value;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(data.SecurityKey));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>
        {
            new("name", "admin")
        };
        var token = new JwtSecurityToken(
            data.Issuer,
            data.Audience,
            claims,
            expires: DateTime.Now.AddSeconds(60 * 60 * 2),
            notBefore: DateTime.Now,
            signingCredentials: creds);
        var returnToken = new JwtSecurityTokenHandler().WriteToken(token);

        return returnToken;
    }
}