namespace ESS 
{
	[SugarTable("ess.project")]
	public class EssProject 
	{
		/// <summary>
		/// 项目标识ID
		/// </summary>
		[SugarColumn(ColumnName = "id")]
		public long Id { get; set; }

		/// <summary>
		/// 区域
		/// </summary>
		[SugarColumn(ColumnName = "area")]
		public string Area { get; set; } = "NULL::character varying";

		/// <summary>
		/// 容量
		/// </summary>
		[SugarColumn(ColumnName = "capacity")]
		public decimal? Capacity { get; set; }

		/// <summary>
		/// 电芯类型
		/// </summary>
		[SugarColumn(ColumnName = "cell_type")]
		public string CellType { get; set; } = "NULL::character varying";

		/// <summary>
		/// 城市
		/// </summary>
		[SugarColumn(ColumnName = "city")]
		public string City { get; set; } = "NULL::character varying";

		/// <summary>
		/// 项目编码
		/// </summary>
		[SugarColumn(ColumnName = "code")]
		public string Code { get; set; } = "NULL::character varying";

		/// <summary>
		/// 国家
		/// </summary>
		[SugarColumn(ColumnName = "country")]
		public string Country { get; set; } = "NULL::character varying";

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
		/// 客户
		/// </summary>
		[SugarColumn(ColumnName = "customer")]
		public string Customer { get; set; } = "NULL::character varying";

		/// <summary>
		/// 调试维护信息url
		/// </summary>
		[SugarColumn(ColumnName = "debbug_url")]
		public string DebbugUrl { get; set; }

		/// <summary>
		/// 工厂ID
		/// </summary>
		[SugarColumn(ColumnName = "factory_id")]
		public string FactoryId { get; set; } = "NULL::character varying";

		/// <summary>
		/// 对接人
		/// </summary>
		[SugarColumn(ColumnName = "fae")]
		public string Fae { get; set; } = "NULL::character varying";

		/// <summary>
		/// 对接人id
		/// </summary>
		[SugarColumn(ColumnName = "fae_id")]
		public string FaeId { get; set; } = "NULL::character varying";

		/// <summary>
		/// 文件路径
		/// </summary>
		[SugarColumn(ColumnName = "file_url")]
		public string FileUrl { get; set; } = "NULL::character varying";

		/// <summary>
		/// FOB质保曲线url
		/// </summary>
		[SugarColumn(ColumnName = "fob_soh_url")]
		public string FobSohUrl { get; set; }

		/// <summary>
		/// FOB时间url
		/// </summary>
		[SugarColumn(ColumnName = "fob_url")]
		public string FobUrl { get; set; }

		/// <summary>
		/// 目录标识ID
		/// </summary>
		[SugarColumn(ColumnName = "folder_id")]
		public long? FolderId { get; set; }

		/// <summary>
		/// 成组系数url
		/// </summary>
		[SugarColumn(ColumnName = "group_url")]
		public string GroupUrl { get; set; }

		/// <summary>
		/// 纬度
		/// </summary>
		[SugarColumn(ColumnName = "latitude")]
		public decimal? Latitude { get; set; }

		/// <summary>
		/// 经度
		/// </summary>
		[SugarColumn(ColumnName = "longitude")]
		public decimal? Longitude { get; set; }

		/// <summary>
		/// 项目名称
		/// </summary>
		[SugarColumn(ColumnName = "name")]
		public string Name { get; set; } = "NULL::character varying";

		/// <summary>
		/// PACK数量
		/// </summary>
		[SugarColumn(ColumnName = "pack_num")]
		public int? PackNum { get; set; }

		/// <summary>
		/// PACK类型
		/// </summary>
		[SugarColumn(ColumnName = "pack_type")]
		public string PackType { get; set; }

		/// <summary>
		/// 项目经理
		/// </summary>
		[SugarColumn(ColumnName = "pm")]
		public string Pm { get; set; } = "NULL::character varying";

		/// <summary>
		/// pmid
		/// </summary>
		[SugarColumn(ColumnName = "pm_id")]
		public string PmId { get; set; } = "NULL::character varying";

		[SugarColumn(ColumnName = "processing")]
		public int? Processing { get; set; } = 0;

		/// <summary>
		/// 产品ID
		/// </summary>
		[SugarColumn(ColumnName = "product_id")]
		public long? ProductId { get; set; }

		/// <summary>
		/// 承诺RACK能量
		/// </summary>
		[SugarColumn(ColumnName = "rack_energy")]
		public decimal? RackEnergy { get; set; }

		/// <summary>
		/// Rack相关路径
		/// </summary>
		[SugarColumn(ColumnName = "rack_url")]
		public string RackUrl { get; set; } = "NULL::character varying";

		/// <summary>
		/// 备注
		/// </summary>
		[SugarColumn(ColumnName = "remark")]
		public string Remark { get; set; } = "NULL::character varying";

		/// <summary>
		/// 承诺RTE url
		/// </summary>
		[SugarColumn(ColumnName = "rte_url")]
		public string RteUrl { get; set; }

		/// <summary>
		/// 运营质保曲线url
		/// </summary>
		[SugarColumn(ColumnName = "run_soh_url")]
		public string RunSohUrl { get; set; }

		/// <summary>
		/// SAT时间
		/// </summary>
		[SugarColumn(ColumnName = "sat_time")]
		public DateTime? SatTime { get; set; }

		/// <summary>
		/// 隔膜类型
		/// </summary>
		[SugarColumn(ColumnName = "separator_type")]
		public string SeparatorType { get; set; } = "NULL::character varying";

		/// <summary>
		/// 电芯批次号
		/// </summary>
		[SugarColumn(ColumnName = "serial_cell")]
		public string SerialCell { get; set; } = "NULL::character varying";

		/// <summary>
		/// 模组批次号
		/// </summary>
		[SugarColumn(ColumnName = "serial_module")]
		public string SerialModule { get; set; } = "NULL::character varying";

		/// <summary>
		/// Pack批次号
		/// </summary>
		[SugarColumn(ColumnName = "serial_pack")]
		public string SerialPack { get; set; } = "NULL::character varying";

		/// <summary>
		/// Rack批次号
		/// </summary>
		[SugarColumn(ColumnName = "serial_rack")]
		public string SerialRack { get; set; } = "NULL::character varying";

		/// <summary>
		/// 数据资源服务标识ID（冷调）
		/// </summary>
		[SugarColumn(ColumnName = "server_cid")]
		public long? ServerCid { get; set; }

		/// <summary>
		/// 数据资源服务标识ID（热调）
		/// </summary>
		[SugarColumn(ColumnName = "server_hid")]
		public string ServerHid { get; set; } = "NULL::character varying";

		/// <summary>
		/// 验收标准url
		/// </summary>
		[SugarColumn(ColumnName = "standard_url")]
		public string StandardUrl { get; set; }

		/// <summary>
		/// 状态
		/// </summary>
		[SugarColumn(ColumnName = "status")]
		public string Status { get; set; } = "NULL::character varying";

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

		/// <summary>
		/// 质保相关路径
		/// </summary>
		[SugarColumn(ColumnName = "warranty_url")]
		public string WarrantyUrl { get; set; } = "NULL::character varying";

		/// <summary>
		/// 年度维护信息url
		/// </summary>
		[SugarColumn(ColumnName = "year_debbug_url")]
		public string YearDebbugUrl { get; set; }

	}
}
