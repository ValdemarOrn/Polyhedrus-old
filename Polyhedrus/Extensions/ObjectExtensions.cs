using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Polyhedrus
{
	public static class ObjectExtensions
	{
		/// <summary>
		/// Evaluates an expression with the object as a parameter.
		/// If the expression can be successfully evaluated, it returns the result.
		/// If an exception occurs, e.g. NullReferenceException or other, 
		/// it returns a default value.
		/// </summary>
		/// <typeparam name="TArg">Type of the argument object</typeparam>
		/// <typeparam name="TRes">Type of result value</typeparam>
		/// <param name="arg">Input argument</param>
		/// <param name="expression">Expression to evaluate</param>
		/// <param name="defaultValue">Default value in case the expression cannot be evaluated</param>
		/// <returns></returns>
		public static TRes Eval<TArg, TRes>(this TArg arg, Func<TArg, TRes> expression,
			TRes defaultValue = default(TRes))
		{
			try
			{
				if (((object)arg) == null)
				{
					return defaultValue;
				}
				return expression(arg);
			}
			catch (Exception)
			{
				return defaultValue;
			}
		}

		/// <summary>
		/// Evaluates an expression with the object as a parameter.
		/// If the expression can be successfully evaluated, it returns the result.
		/// If an exception occurs, e.g. NullReferenceException or other, 
		/// it returns a Nullable&lt;&gt; default value.
		/// </summary>
		/// <typeparam name="TArg">Type of the argument object</typeparam>
		/// <typeparam name="TRes">Type of the Nullable&lt;&gt; result value</typeparam>
		/// <param name="arg">Input argument</param>
		/// <param name="expression">Expression to evaluate</param>
		/// <param name="defaultValue">Default value in case the expression cannot be evaluated</param>
		/// <returns></returns>
		public static Nullable<TRes> Eval<TArg, TRes>(this TArg arg, Func<TArg, TRes> expression,
			Nullable<TRes> defaultValue) where TRes : struct
		{
			try
			{
				if (((object)arg) == null)
				{
					return defaultValue;
				}
				return expression(arg);
			}
			catch (Exception)
			{
				return defaultValue;
			}
		}
	}
}
