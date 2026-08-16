using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;
using Vax.Service.Interface;
using Vax.Service.Resources;

namespace Vax.Service.Implmentation
{
	public class LocalizationService : ILocalizationService
	{
		private readonly IStringLocalizer<SharedResource> _localizer;

		public LocalizationService(IStringLocalizer<SharedResource> localizer)
		{
			_localizer = localizer;
			//var type = typeof(SharedResource);
			//var assemblyName = new System.Reflection.AssemblyName(type.GetType().Assembly.FullName);
			//_localizer = factory.Create("SharedResource", assemblyName.Name);
		}
		public string Get(string key)
		{
			return _localizer[key];
		}
	}
}
