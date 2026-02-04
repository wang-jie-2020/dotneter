namespace ESS 
{
	[SugarTable("ess.project_table")]
	public class EssProjectTable 
	{
		/// <summary>
		/// 项目表关系标识ID
		/// </summary>
		[SugarColumn(ColumnName = "id")]
		public long Id { get; set; }

		/// <summary>
		/// 基地
		/// </summary>
		[SugarColumn(ColumnName = "base")]
		public string Base { get; set; } = "NULL::character varying";

		/// <summary>
		/// 分类Id
		/// </summary>
		[SugarColumn(ColumnName = "category_id")]
		public long? CategoryId { get; set; }

		/// <summary>
		/// 创建者
		/// </summary>
		[SugarColumn(ColumnName = "create_by")]
		public long? CreateBy { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		[SugarColumn(ColumnName = "create_time")]
		public DateTime? CreateTime { get; set; }

		/// <summary>
		/// 定制字段用,拼接
		/// </summary>
		[SugarColumn(ColumnName = "cust_columns")]
		public string CustColumns { get; set; } = "NULL::character varying";

		/// <summary>
		/// 字典类型 1电芯号2模组号3PACK号4涂布号5碾压号6预分切号7分切号8烘烤号9匀浆号10裸电芯号11RACK号
		/// </summary>
		[SugarColumn(ColumnName = "dict")]
		public short? Dict { get; set; }

		/// <summary>
		/// 目录标识ID
		/// </summary>
		[SugarColumn(ColumnName = "folder_id")]
		public long? FolderId { get; set; }

		/// <summary>
		/// 路径
		/// </summary>
		[SugarColumn(ColumnName = "path")]
		public string Path { get; set; } = "NULL::character varying";

		/// <summary>
		/// 项目标识ID
		/// </summary>
		[SugarColumn(ColumnName = "project_id")]
		public long ProjectId { get; set; }

		/// <summary>
		/// 表名称
		/// </summary>
		[SugarColumn(ColumnName = "table_name")]
		public string TableName { get; set; } = "NULL::character varying";

		/// <summary>
		/// 模板标识Id
		/// </summary>
		[SugarColumn(ColumnName = "template_id")]
		public long? TemplateId { get; set; }

		/// <summary>
		/// 更新者
		/// </summary>
		[SugarColumn(ColumnName = "update_by")]
		public long? UpdateBy { get; set; }

		/// <summary>
		/// 更新时间
		/// </summary>
		[SugarColumn(ColumnName = "update_time")]
		public DateTime? UpdateTime { get; set; }

	}
}
