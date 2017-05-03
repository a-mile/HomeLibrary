using System;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace HomeLibrary.HtmlHelpers
{
	public static class BootstrapHelpers
	{
		public static IHtmlContent BootstrapLabelFor<TModel, TProp>(
				this IHtmlHelper<TModel> helper,
				Expression<Func<TModel, TProp>> property)
		{
			return helper.LabelFor(property, new
			{
				@class = "col-md-2 control-label"
			});
		}

		public static IHtmlContent BootstrapLabel(
				this IHtmlHelper helper,
				string propertyName)
		{
			return helper.Label(propertyName,propertyName, new
			{
				@class = "col-md-2 control-label"
			});
		}

        public static IHtmlContent BootstrapValidationMessage(
				this IHtmlHelper helper,
				string propertyName)
		{
			return helper.ValidationMessage(propertyName,"", new
			{
				@class = "text-danger"
			});
		}
	}
}