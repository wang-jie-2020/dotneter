namespace ESS 
{
	[SugarTable("ess.project_user")]
	public class EssProjectUser 
	{
		/// <summary>
		/// 标识ID
		/// </summary>
		[SugarColumn(ColumnName = "id")]
		public long Id { get; set; }

		/// <summary>
		/// 用户昵称
		/// </summary>
		[SugarColumn(ColumnName = "nickname")]
		public string Nickname { get; set; } = "NULL::character varying";

		/// <summary>
		/// 关联编号
		/// </summary>
		[SugarColumn(ColumnName = "num")]
		public string Num { get; set; } = "NULL::character varying";

		/// <summary>
		/// 项目ID
		/// </summary>
		[SugarColumn(ColumnName = "projectid")]
		public long Projectid { get; set; }

		/// <summary>
		/// 项目名称
		/// </summary>
		[SugarColumn(ColumnName = "projectname")]
		public string Projectname { get; set; } = "NULL::character varying";

		/// <summary>
		/// 角色状态
		/// </summary>
		[SugarColumn(ColumnName = "rolestatus")]
		public int Rolestatus { get; set; }

		/// <summary>
		/// 用户ID
		/// </summary>
		[SugarColumn(ColumnName = "userid")]
		public long Userid { get; set; }

		/// <summary>
		/// 用户名
		/// </summary>
		[SugarColumn(ColumnName = "username")]
		public string Username { get; set; } = "NULL::character varying";

	}
}
