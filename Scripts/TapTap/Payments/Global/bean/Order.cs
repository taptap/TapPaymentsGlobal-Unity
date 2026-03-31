namespace TapTap.Payments.Global.bean
{
	/// <summary>
    /// 订单类
    /// </summary>
    public class Order
    {
		/// <summary>
    	/// 订单支付状态
    	/// </summary>
    	public enum State
    	{
    		/// <summary>
    		/// 未知状态
    		/// </summary>
    		UNKNOWN = 0,
    
    		/// <summary>
    		/// 支付中
    		/// </summary>
    		PAYMENT_PENDING = 2,
    
    		/// <summary>
    		/// 已支付
    		/// </summary>
    		PAID = 3,
			
			/// <summary>
			/// 订单已完成支付，道具已发放
			/// </summary>
			COMPLETED = 4,
    
    		/// <summary>
    		/// 支付超时
    		/// </summary>
    		PAYMENT_TIMEOUT = 5,

			/// <summary>
    		/// 退款中
    		/// </summary>
    		REFUNDING = 20,
    
    		/// <summary>
    		/// 退款成功
    		/// </summary>
    		REFUNDED = 21,
    
    		/// <summary>
    		/// 退款失败
    		/// </summary>
    		REFUND_FAILED = 22,
    
    		/// <summary>
    		/// 退款被驳回
    		/// </summary>
			REFUND_REJECTED = 23
    	}

		/// <summary>
    	/// 支付渠道
    	/// </summary>
    	public string channel;

		/// <summary>
    	/// 客户端 ID
    	/// </summary>
    	public string clientId;

		/// <summary>
		/// 货币单位
		/// </summary>
		public string currency;

		/// <summary>
		/// 开发者自定义的信息
		/// </summary>
		public string extra;

		/// <summary>
    	/// 渠道费率
    	/// </summary>
    	public decimal fee;

		/// <summary>
    	/// 订单 ID
    	/// </summary>
    	public string id;

		/// <summary>
		/// 商品 ID
		/// </summary>
		public string itemId;

		/// <summary>
		/// 商品价格
		/// </summary>
		public decimal price;

		/// <summary>
		/// 购买的商品数量
		/// </summary>
		public int quantity;

		/// <summary>
    	/// 用户地区 ID
    	/// </summary>
    	public string regionId;

		/// <summary>
    	/// 订单状态枚举
    	/// </summary>
    	public State state;

		/// <summary>
		/// 税费
		/// </summary>
		public decimal tax;

		/// <summary>
    	/// 订单 Token
    	/// </summary>
    	public string token;

		/// <summary>
    	/// 用户 OpenID
    	/// </summary>
    	public string userId;

		public override string ToString ()
		{
			return $"{nameof ( itemId )}: {itemId}, {nameof ( price )}: {price}, {nameof ( tax )}: {tax}, {nameof ( currency )}: {currency}, {nameof ( quantity )}: {quantity}, {nameof ( extra )}: {extra}, {nameof ( id )}: {id}, {nameof ( token )}: {token}, {nameof ( state )}: {state}, {nameof ( channel )}: {channel}, {nameof ( fee )}: {fee}, {nameof ( clientId )}: {clientId}, {nameof ( userId )}: {userId}, {nameof ( regionId )}: {regionId}";
		}
	}
}
