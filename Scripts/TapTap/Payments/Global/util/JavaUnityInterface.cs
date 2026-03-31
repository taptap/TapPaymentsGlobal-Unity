using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using TapTap.Payments.Global;
using UnityEngine.UI;

namespace TapTap.Payments.Global.util
{
	public class JavaUnityInterface
	{
		protected static object Get ( AndroidJavaObject javaObject, string name, Type type )
		{
			if ( type == typeof ( bool ) )
			{
				return javaObject.Get < bool > ( name );
			}
			else if ( type == typeof ( char ) )
			{
				return javaObject.Get < char > ( name );
			}
			else if ( type == typeof ( byte ) )
			{
				return javaObject.Get < byte > ( name );
			}
			else if ( type == typeof ( sbyte ) )
			{
				return javaObject.Get < sbyte > ( name );
			}
			else if ( type == typeof ( short ) )
			{
				return javaObject.Get < short > ( name );
			}
			else if ( type == typeof ( int ) )
			{
				return javaObject.Get < int > ( name );
			}
			else if ( type == typeof ( long ) )
			{
				return javaObject.Get < long > ( name );
			}
			else if ( type == typeof ( float ) )
			{
				return javaObject.Get < float > ( name );
			}
			else if ( type == typeof ( double ) )
			{
				return javaObject.Get < double > ( name );
			}
			else if ( type == typeof ( string ) )
			{
				return javaObject.Get < string > ( name );
			}
			else if ( type.IsArray )
			{
				if ( type == typeof ( bool [] ) )
				{
					return javaObject.Get < bool [] > ( name );
				}
				else if ( type == typeof ( char [] ) )
				{
					return javaObject.Get < char [] > ( name );
				}
				else if ( type == typeof ( byte [] ) )
				{
					return javaObject.Get < byte [] > ( name );
				}
				else if ( type == typeof ( sbyte [] ) )
				{
					return javaObject.Get < sbyte [] > ( name );
				}
				else if ( type == typeof ( short [] ) )
				{
					return javaObject.Get < short [] > ( name );
				}
				else if ( type == typeof ( int [] ) )
				{
					return javaObject.Get < int [] > ( name );
				}
				else if ( type == typeof ( long [] ) )
				{
					return javaObject.Get < long [] > ( name );
				}
				else if ( type == typeof ( float [] ) )
				{
					return javaObject.Get < float [] > ( name );
				}
				else if ( type == typeof ( double [] ) )
				{
					return javaObject.Get < double [] > ( name );
				}
				else if ( type == typeof ( string [] ) )
				{
					return javaObject.Get < string [] > ( name );
				}
				else
				{
					return javaObject.Get < AndroidJavaObject [] > ( name );
				}
			}
			else
			{
				return javaObject.Get < AndroidJavaObject > ( name );
			}
		}

		public static object CopyFormObject ( AndroidJavaObject javaObject, Type type )
		{
			if ( javaObject == null ) return null;
			
			object result = null;
			if ( type == typeof ( TapPayment.Error ) )
			{
				TapPayment.Error.Code code = ( TapPayment.Error.Code ) Enum.Parse ( typeof ( TapPayment.Error.Code ), javaObject.Get < AndroidJavaObject > ( "code" ).Call < string > ( "name" ) );
				string message = javaObject.Call < string > ( "getMessage" );
				return new TapPayment.Error ( code, message );
			}
			else if ( type == typeof ( decimal ) )
			{
				return Decimal.Parse ( javaObject.Call < string > ( "toPlainString" ), NumberStyles.Number );
			}
			else if ( type.IsEnum )
			{
				result = Enum.Parse ( type, javaObject.Call < string > ( "name" ) );
			}
			else
			{
				result = Activator.CreateInstance ( type );
				foreach ( var field in type.GetFields () )
				{
					if ( !field.IsPublic ) continue;

					var fieldObject = Get ( javaObject, field.Name, field.FieldType );
					if ( fieldObject.GetType () == typeof ( AndroidJavaObject ) )
					{
						fieldObject = CopyFormObject ( ( AndroidJavaObject ) fieldObject, field.FieldType );
					}
					else if ( fieldObject.GetType () == typeof ( AndroidJavaObject [] ) )
					{
						var elementType = field.FieldType.GetElementType ();
						var array = Array.CreateInstance ( elementType, ( ( AndroidJavaObject [] ) fieldObject ).Length );
						for ( int i = 0; i < array.Length; i++ )
						{
							array.SetValue ( CopyFormObject ( ( ( AndroidJavaObject [] ) fieldObject ) [ i ], elementType ), i );
						}

						fieldObject = array;
					}
					
					field.SetValue ( result, fieldObject );
				}

				return result;
			}

			return result;
		}

		public static T CopyFormObject < T > ( AndroidJavaObject javaObject )
		{
			return ( T ) CopyFormObject ( javaObject, typeof ( T ) );
		}

		public static T CopyFormObject < T > ( AndroidJavaObject[] javaObjects )
		{
			Type type = typeof ( T );
			if ( !type.IsArray )
			{
				throw new InvalidCastException ( "Can not copy javaObjects to not array" );
			}
			var elementType = type.GetElementType ();
			var array = Array.CreateInstance ( elementType, javaObjects.Length );
			for ( int i = 0; i < javaObjects.Length; i++ )
			{
				array.SetValue ( CopyFormObject ( javaObjects [ i ], elementType ), i );
			}

			return (T) (object) array;
		}
	}
}