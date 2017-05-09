using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace HomeLibrary.Infrastructure.Alerts
{
	public static class AlertExtensions
	{
		const string Alerts = "_Alerts";

		public static List<Alert> GetAlerts(this ITempDataDictionary tempData)
		{
			if (!tempData.ContainsKey(Alerts))
			{
				tempData[Alerts] = string.Empty;
			}

			return DeserializeAlerts((string)tempData[Alerts]);
		}

		public static void SetAlerts(this ITempDataDictionary tempData, List<Alert> alerts)
		{
			tempData[Alerts] = SerializeAlerts(alerts);
		}

		public static string SerializeAlerts(List<Alert> tempData)
		{
    		return JsonConvert.SerializeObject(tempData);
		}
		public static List<Alert> DeserializeAlerts(string tempData)
		{
			if(tempData.Length == 0)
			{
				return new List<Alert>();
			}
			return JsonConvert.DeserializeObject<List<Alert>>(tempData);
		}

		public static ActionResult WithSuccess(this ActionResult result, string message)
		{
			return new AlertDecoratorResult(result, "alert-success", message);
		}

		public static ActionResult WithInfo(this ActionResult result, string message)
		{
			return new AlertDecoratorResult(result, "alert-info", message);
		}

		public static ActionResult WithWarning(this ActionResult result, string message)
		{
			return new AlertDecoratorResult(result, "alert-warning", message);
		}

		public static ActionResult WithError(this ActionResult result, string message)
		{
			return new AlertDecoratorResult(result, "alert-danger", message);
		}
	}
}