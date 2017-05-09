using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace HomeLibrary.Infrastructure.Alerts
{
	public class AlertDecoratorResult : ActionResult
	{
		public ActionResult InnerResult { get; set; }
		public string AlertClass { get; set; }
		public string Message { get; set; }

		public AlertDecoratorResult(ActionResult innerResult, string alertClass, string message)
		{
			InnerResult = innerResult;
			AlertClass = alertClass;
			Message = message;
		}

		public override async Task ExecuteResultAsync(ActionContext context)
		{
			ITempDataDictionaryFactory factory = context.HttpContext.RequestServices.GetService(typeof(ITempDataDictionaryFactory)) as ITempDataDictionaryFactory;
   			ITempDataDictionary tempData = factory.GetTempData(context.HttpContext);

    		var alerts = tempData.GetAlerts();
    		alerts.Add(new Alert(AlertClass, Message));
			tempData.SetAlerts(alerts);

    		await InnerResult.ExecuteResultAsync(context);
		}
	}
}