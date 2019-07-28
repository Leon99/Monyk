using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Monyk.Lab.Main.Processors
{
    public class ResultProcessorFactory
    {
        private readonly IServiceProvider _svcProvider;
        private readonly IEnumerable<TypeInfo> _processors;

        public ResultProcessorFactory(IServiceProvider svcProvider)
        {
            _svcProvider = svcProvider;
            _processors = Assembly.GetExecutingAssembly().DefinedTypes.Where(t => t.ImplementedInterfaces.Contains(typeof(IResultProcessor)));
        }

        public IResultProcessor Create(string name, string settingsStr)
        {
            var resultProcessorType = _processors.FirstOrDefault(t => t.Name == name);
            var ctorParameters = resultProcessorType.DeclaredConstructors.First().GetParameters();
            if (ctorParameters.Any())
            {
                var settingsType = ctorParameters.First().ParameterType;
                object settings;
                if (settingsType != typeof(string))
                {
                    settings = JsonConvert.DeserializeObject(settingsStr, settingsType);
                    if (settingsType != typeof(object) && settings == null)
                    {
                        throw new ApplicationException("Settings object is expected but no valid object is provided.");
                    }
                }
                else
                {
                    settings = settingsStr;
                }

                return (IResultProcessor)ActivatorUtilities.CreateInstance(_svcProvider, resultProcessorType, settings);
            }
            else
            {
                return (IResultProcessor)ActivatorUtilities.CreateInstance(_svcProvider, resultProcessorType);
            }
        }
    }
}
