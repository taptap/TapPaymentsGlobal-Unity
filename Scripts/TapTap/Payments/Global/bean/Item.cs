namespace TapTap.Payments.Global.bean
{
	/// <summary>
	/// 商品信息类
	/// </summary>
	public class Item
	{
		/// <summary>
		/// 货币单位
		/// </summary>
		public string currency;

		/// <summary>
		/// 商品描述
		/// </summary>
		public string description;

		/// <summary>
		/// 商品 ID
		/// </summary>
		public string id;

		/// <summary>
		/// 商品语言
		/// </summary>
		public string languageId;

		/// <summary>
		/// 商品名称
		/// </summary>
		public string name;

		/// <summary>
		/// 商品价格
		/// </summary>
		public decimal price;

		/// <summary>
		/// 商品地区
		/// </summary>
		public string regionId;

		/// <summary>
		/// 商品类型
		/// </summary>
		public int type;

		public override string ToString ()
		{
			return $"{nameof ( type )}: {type}, {nameof ( id )}: {id}, {nameof ( name )}: {name}, {nameof ( description )}: {description}, {nameof ( price )}: {price}, {nameof ( currency )}: {currency}, {nameof ( regionId )}: {regionId}, {nameof ( languageId )}: {languageId}";
		}
	}
}