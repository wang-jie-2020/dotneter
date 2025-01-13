using System;
using Demo.Controllers;
using Newtonsoft.Json;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;

namespace Demo.Enrich
{
    public class CustomerEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (!logEvent.Properties.ContainsKey("OrderOperation"))
            {
                return;
            }

            var orderOperation = logEvent.Properties["OrderOperation"] as StructureValue;
            if (orderOperation == null)
            {
                return;
            }

            foreach (var t in orderOperation.Properties)
            {
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(t.Name, t.Value));
            }
        }
    }

    public class CustomerEnricher<T> : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var entryName = typeof(T).FullName;
            var aliasName = typeof(T).Name;

            if (!logEvent.Properties.ContainsKey(entryName))
            {
                return;
            }

            var obj = logEvent.Properties[entryName] as StructureValue;
            if (obj == null)
            {
                return;
            }

            foreach (var property in obj.Properties)
            {
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(aliasName + "_" + property.Name, property.Value));
            }
        }
    }
}