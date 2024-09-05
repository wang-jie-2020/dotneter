using Yi.Framework.Ddd;
using Yi.Framework.Ddd.Application.Contracts;

namespace Yi.Framework.Rbac.Application.Contracts.Dtos.Config
{
    /// <summary>
    /// ���ò�ѯ����
    /// </summary>
    public class ConfigGetListInputVo : PagedAllResultRequestDto
    {
        /// <summary>
        /// ��������
        /// </summary>
        public string? ConfigName { get; set; }

        /// <summary>
        /// ���ü�
        /// </summary>
        public string? ConfigKey { get; set; }

    }
}
