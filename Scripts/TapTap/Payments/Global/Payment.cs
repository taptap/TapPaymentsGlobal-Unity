using System;
using TapTap.Payments.Global.bean;
using TapTap.Payments.Global.util;
using UnityEngine;

namespace TapTap.Payments.Global
{
	public static class TapPayment
	{
		private static UnityDispatcher dispatcher;
		static TapPayment(){
			dispatcher = UnityDispatcher.Instance;
		}
		private static AndroidJavaClass UnityPlayer = new AndroidJavaClass ( "com.unity3d.player.UnityPlayer" );

		private static AndroidJavaClass TapPaySDKLoader = new AndroidJavaClass ( "com.taptap.payment.shell.TapPaySDKLoader" );
		private static AndroidJavaObject currentActivity => UnityPlayer.GetStatic < AndroidJavaObject > ( "currentActivity" );

		public static string SDKVersion => TapPaySDKLoader.CallStatic < string > ( "SDKVersion" );

		private static AndroidJavaObject getSDK ()
		{
			return TapPaySDKLoader.CallStatic < AndroidJavaObject > ( "getSDK" );
		}

		/// <summary>
		/// 请求支付
		/// </summary>
		/// <param name="lang">需要国际化语言代码
		/// 如 "en"、"zh_CN" 等。
		/// </param>
		/// <param name="id">商品 ID</param>
		/// <param name="quantity">购买数量</param>
		/// <param name="extra"> 开发者自定义的信息
		/// 该参数会在支付流程结束时，作为参数传入<see cref="PayCallback"/>回调方法，当支付操作完成时。
		/// </param>
		/// <param name="callback">回调对象</param>
		public static void RequestPayFlow ( string lang, string id, int quantity, string extra, PayCallback callback )
		{
			getSDK ().Call ( "requestPayFlow", currentActivity, lang, id, quantity, extra, new PayCallbackWrapper ( callback ) );
		}

		///<summary>
		///请求支付 (不包括语言参数 lang)
		///</summary>
		///
		/// <param name="id">商品 ID</param>
		/// <param name="quantity">购买数量</param>
		/// <param name="extra"> 开发者自定义的信息
		/// 该参数会在支付流程结束时，作为参数传入<see cref="PayCallback"/>回调方法，当支付操作完成时。
		/// </param>
		/// <param name="callback">回调对象</param>
		public static void RequestPayFlow ( string id, int quantity, string extra, PayCallback callback )
		{
			RequestPayFlow ( null, id, quantity, extra, callback );
		}

		/// <summary>
		/// 打开订单列表
		/// 打开当前登陆用户的订单列表。
		/// </summary>
		/// <param name="lang">需要国际化语言代码
		/// 如 "en"、"zh_CN" 等。
		/// </param>
		public static void OpenUserOrderList ( string lang )
		{
			getSDK ().Call ( "openUserOrderList", currentActivity, lang );
		}

		/// <summary>
		/// 打开订单列表 (不包括 lang)
		/// </summary>
		public static void OpenUserOrderList ()
		{
			OpenUserOrderList ( null );
		}

		/// <summary>
		/// 查询商品信息
		/// </summary>
		/// <param name="lang">语言参数</param>
		/// <param name="ids">商品 ID 数组</param>
		/// <param name="callback">回调对象
		/// 详见 <see cref="Item"/> 和 <see cref="Callback{T}"/>
		/// </param>
		public static void QueryItems ( string lang, string [] ids, Callback < Item [] > callback )
		{
			getSDK ().Call ( "queryItems", new object [] { lang, ids, new CallbackWrapper < Item [] > ( callback ) } );
		}

		public static void QueryItems ( string [] ids, Callback < Item [] > callback )
		{
			QueryItems ( null, ids, callback );
		}

		/// <summary>
		/// 查询订单信息
		/// </summary>
		/// <param name="id">订单 ID</param>
		/// <param name="callback">查询结果的异步回调方法
		/// 详见 <see cref="Order"/> 和 <see cref="Callback{T}"/>
		/// </param>
		public static void QueryOrder ( string id, Callback < Order > callback )
		{
			getSDK ().Call ( "queryOrder", new object [] { id, new CallbackWrapper < Order > ( callback ) } );
		}

		/// <summary>
		/// 验证订单
		/// 当发放完商品后调用此接口进行订单确认。
		/// </summary>
		/// <param name="id">商品 ID</param>
		/// <param name="token">商品 Token</param>
		/// <param name="callback">查询结果的异步回调方法
		/// 详见 <see cref="Order"/> 和 <see cref="Callback{T}"/>
		/// </param>
		public static void ConsumeOrder ( string id, string token, Callback < Order > callback )
		{
			getSDK ().Call ( "consumeOrder", new object [] { id, token, new CallbackWrapper < Order > ( callback ) } );
		}

		/// <summary>
		/// 验证订单
		/// 当发放完商品后调用此接口进行订单确认。
		/// </summary>
		/// <param name="order">订单对象</param>
		/// <param name="callback">查询结果的异步回调方法
		/// 详见 <see cref="Order"/> 和 <see cref="Callback{T}"/>
		/// </param>
		public static void ConsumeOrder ( Order order, Callback < Order > callback )
		{
			getSDK ().Call ( "consumeOrder", new object [] { order.id, order.token, new CallbackWrapper < Order > ( callback ) } );
		}

		/// <summary>
		/// 支付异常类
		/// </summary>
		public class Error : Exception
		{
			/// <summary>
			/// 错误代码的枚举类型
			/// </summary>
			public enum Code
			{
				/// <summary>
				/// 未知错误
				/// </summary>
				UNKNOWN = -1,

				/// <summary>
				/// 正常
				/// </summary>
				OK = 0,

				/// <summary>
				/// 传递参数非法
				/// </summary>
				ILLEGAL_ARGUMENT = 1,

				/// <summary>
				/// 网络错误
				/// </summary>
				NETWORK_ERROR = 2,

				/// <summary>
				/// 服务器出现问题
				/// </summary>
				INTERNAL_SERVER_ERROR = 3,

				/// <summary>
				/// 用户未登录
				/// </summary>
				NOT_LOGIN = 401,

				/// <summary>
				/// 请求过于频繁
				/// </summary>
				TOO_MANY_REQUEST = 1003,

				/// <summary>
				/// 商品不存在
				/// </summary>
				ITEM_NOT_FOUND = 1001,

				/// <summary>
				/// 商品信息已变更
				/// </summary>
				///
				ITEM_INFO_CHANGE = 1017,

				/// <summary>
				/// 重复购买非消耗型商品
				/// </summary>
				REPEAT_PURCHASE_NON_CONSUMABLE_GOODS = 1002,

				/// <summary>
				/// 订单不存在
				/// </summary>
				ORDER_NOT_FOUND = 1004,

				/// <summary>
				/// 创建订单失败
				/// </summary>
				ORDER_CREATE_FAILED = 1005,

				/// <summary>
				/// 订单已支付
				/// </summary>
				ORDER_PAID = 1006,
				
				/// <summary>
				/// 订单超时
				/// </summary>
				ORDER_TIMEOUT = 1008,

				/// <summary>
				/// 验证订单错误
				/// </summary>
				ORDER_VERIFY_ERROR = 1018,

				/// <summary>
				/// 地区不可更改
				/// </summary>
				REGION_NOT_EDITABLE = 1007
			}

			/// <summary>
			/// 错误代码
			/// </summary>
			public Code code;

			/// <summary>
			/// Error 的构造函数
			/// 创建一个支付错误对象，其中包含支付错误代码和信息。
			/// </summary>
			/// <param name="code">错误代码</param>
			/// <param name="message">错误信息</param>
			public Error ( Code code, string message ) : base ( message )
			{
				this.code = code;
			}

			public override string ToString ()
			{
				return "Error(" + "code=" + code + ", message=" + this.Message + ')';
			}
		}

		private class PayCallbackWrapper : AndroidJavaProxy
		{
			private PayCallback callback;

			public PayCallbackWrapper ( PayCallback callback ) : base ( "com.taptap.payment.api.ITapPayment$PayCallback" )
			{
				this.callback = callback;
			}
			
			public void onCancel ( string extra )
			{
				dispatcher.PostTask(() => callback.OnCancel?.Invoke ( extra ));
			}

			public void onCancel ( AndroidJavaObject extra )
			{
				dispatcher.PostTask(() => callback.OnCancel?.Invoke ( JavaUnityInterface.CopyFormObject < string > ( extra ) ));
			}

			public void onError ( AndroidJavaObject error, string extra )
			{
				dispatcher.PostTask(() => callback.OnError?.Invoke
				( 
				    JavaUnityInterface.CopyFormObject < Error > ( error ),
					extra
				));
				
			}

			public void onError ( AndroidJavaObject error, AndroidJavaObject extra )
			{
				dispatcher.PostTask(() => callback.OnError?.Invoke
				(
					JavaUnityInterface.CopyFormObject < Error > ( error ),
					JavaUnityInterface.CopyFormObject < string > ( extra )
				));
			}
			
			public void onFinish ( AndroidJavaObject order, string extra )
			{
				dispatcher.PostTask(() => callback.OnFinish?.Invoke
				( 
				    JavaUnityInterface.CopyFormObject < Order > ( order ),
				    extra
				));
			}

			public void onFinish ( AndroidJavaObject order, AndroidJavaObject extra )
			{
				dispatcher.PostTask(() => callback.OnFinish?.Invoke
				(
					JavaUnityInterface.CopyFormObject < Order > ( order ),
					JavaUnityInterface.CopyFormObject < string > ( extra )
				));
			}
		}

		/// <summary>
		/// PayCallback 用作支付请求函数的异步回调通知，相关函数为：
		/// <ul>
		///     <li><see cref="RequestPayFlow(string,int,string,PayCallback)"/></li>
		///     <li><see cref="RequestPayFlow(string,string,int,string,PayCallback)"/></li>
		/// </ul>
		/// </summary>
		public class PayCallback
		{
			/// <summary>
			/// 取消支付
			/// 当用户取消支付操作时的异步回调。
			/// </summary>
			/// <param name="extra"> 开发者自定义的信息
			/// </param>
			public delegate void CancelType ( string extra );

			/// <summary>
			/// 支付错误
			/// 当支付过程中出现错误时的异步回调。
			/// </summary>
			/// <param name="error"> 错误信息
			/// 支付中发生错误时回调的错误信息，详见<see cref="Error"/>。
			/// </param>
			/// <param name="extra"> 开发者自定义的信息
			///</param>
			public delegate void ErrorType ( Error error, string extra );


			/// <summary>
			/// 支付流程结束
			/// 当支付流程完成时的异步回调。
			/// 注意：建议请求服务器以确定订单状态，并不要依赖此回调。此回调不能保证被调用时订单一定支付成功。
			/// </summary>
			/// <param name="order">订单信息
			/// 支付流程结束时回调的订单对象，详见<see cref="Order"/>。
			/// </param>
			/// <param name="extra">开发者自定义的信息</param>
			public delegate void FinishType ( Order order, string extra );

			public CancelType OnCancel { get; set; }
			public ErrorType OnError { get; set; }
			public FinishType OnFinish { get; set; }
		}

		private class CallbackWrapper < T > : AndroidJavaProxy
		{
			private Callback < T > callback;

			public CallbackWrapper ( Callback < T > callback ) : base ( "com.taptap.payment.api.ITapPayment$Callback" )
			{
				this.callback = callback;
			}

			public void onError ( AndroidJavaObject error )
			{
				dispatcher.PostTask(() => callback.OnError (JavaUnityInterface.CopyFormObject < Error > ( error )));
			}

			public void onFinish ( AndroidJavaObject result )
			{
				dispatcher.PostTask(() => callback.OnFinish (JavaUnityInterface.CopyFormObject < T > ( result )));
			}

			public void onFinish ( AndroidJavaObject [] result )
			{
				dispatcher.PostTask(() => callback.OnFinish (JavaUnityInterface.CopyFormObject < T > ( result )));
			}
		}

		/// <summary>
		/// Callback 接口用于相关函数的异步回调结果，支持的函数有：
		/// <ul>
		///     <li><see cref="QueryItems(string,string[],Callback{Item[]})"/></li>
		///     <li><see cref="QueryItems(string[],Callback{Item[]})"/></li>
		///     <li><see cref="QueryOrder"/></li>
		///		<li><see cref="ConsumeOrder(string,string,Callback{Order})"/></li>
		///		<li><see cref="ConsumeOrder(Order,Callback{Order})"/></li>
		/// </ul>
		/// </summary>
		/// <typeparam name="T">回调结果类型的泛型参数</typeparam>
		public class Callback < T >
		{
			/// <summary>
			/// 请求错误
			/// </summary>
			/// <param name="error">错误
			/// 发生错误时错误信息，详见 <see cref="Error"/>。
			/// </param>
			public delegate void ErrorType ( Error error );

			/// <summary>
			/// 请求结果
			/// </summary>
			/// <param name="result">结果</param>
			public delegate void FinishType ( T result );

			public ErrorType OnError { get; set; }
			public FinishType OnFinish { get; set; }
		}
	}
}
