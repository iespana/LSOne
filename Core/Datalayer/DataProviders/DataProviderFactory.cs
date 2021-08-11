using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using LSOne.DataLayer.BusinessObjects;
using LSOne.DataLayer.BusinessObjects.Sequencable;
using LSOne.DataLayer.DataProviders.Sequences;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities;
using LSOne.Utilities.DataTypes;
using System.Collections.Concurrent;

namespace LSOne.DataLayer.DataProviders
{
	/// <summary>
	/// This factory represents the entry to data providers for LS One
	/// </summary>
    public class DataProviderFactory : Singleton<DataProviderFactory>
	{
        private ConcurrentDictionary<Type, Type> providerTypes;
        private Dictionary<Type, object> providerInstances;
 
        // Singleton requires a private parameterless constructor !
	    private DataProviderFactory()
	    {
	        providerTypes = new ConcurrentDictionary<Type, Type>();
            providerInstances = new Dictionary<Type, object>();
	    }

        /// <summary>
        /// Register a data provider for a given interface
        /// </summary>
        /// <typeparam name="ProviderInterface">Provider interface</typeparam>
        /// <typeparam name="ProviderInstance">Provier instance class</typeparam>
        /// <typeparam name="BusinessObject">Business object class</typeparam>
        public void Register<ProviderInterface, ProviderInstance, BusinessObject>()
	        where ProviderInterface : class
	        where ProviderInstance : class, IDataProviderBase<BusinessObject>, new()
            where BusinessObject : class
        {
	        Register(typeof (ProviderInterface), typeof (ProviderInstance));
	    }

	    private void Register(Type providerInterface, Type provider)
	    {
            providerTypes[providerInterface] = provider;
	    }

        /// <summary>
        /// Register all data providers in the given assembly
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
	    public int RegisterProviders(Assembly assembly)
	    {
	        int cnt = 0;
	        foreach (var type in assembly.GetTypes())
	        {
	            if (IsImplementationOfInterface(type, typeof (IDataProviderBase<>)))
	            {
                    var interfaceList = type.GetInterfaces();

	                foreach (var iface in interfaceList)
	                {
	                    var ifaceType = (iface.IsGenericType ? iface.GetGenericTypeDefinition() : iface);
	                    if (ifaceType != typeof (ISequenceable) &&
	                        ifaceType != typeof (IRegistrar) &&
	                        ifaceType != typeof (IDataProviderBase<>) &&
	                        ifaceType != typeof (IDataProvider<>))
	                    {
	                        Register(ifaceType, type);
	                        cnt++;
	                    }
	                }
	            }
	        }
	        return cnt;
	    }

        /// <summary>
        /// Register via looking up an implementation of IRegistrar in the given assembly
        /// </summary>
        /// <param name="assemblyName">Assembly name</param>
        /// <returns>The number of data providers registered</returns>
	    public int RegisterViaRegistrar(string assemblyName)
	    {
	        try
	        {
	            var a = Assembly.LoadFrom(assemblyName);
	            foreach (var type in a.GetTypes())
	            {
	                if (IsImplementationOfInterface(type, typeof (IRegistrar)))
	                {
	                    var registrar = Activator.CreateInstance(type) as IRegistrar;
	                    return registrar.Register();
	                }
	            }
	        }
            catch
            { }

	        return 0;
	    }

        /// <summary>
        /// Registers a single dataprovider implementation. Delimited parameters should have the form AssemblyName,TypeInAssembly
        /// </summary>
        /// <param name="delimitedInterface">A delimited assembly and type of the interface</param>
        /// <param name="delimitedImplementation">A delimited assembly and type of the implementation</param>
        /// <returns>The number of data providers registered</returns>
	    public int RegisterDelimitedType(string delimitedInterface, string delimitedImplementation)
	    {
            try
            {
                var iface = GetFromDelimitedName(delimitedInterface);
                var type = GetFromDelimitedName(delimitedImplementation);

                Register(iface, type);
                return 1;
            }
            catch
            { }

            return 0;
	    }

	    private static Type GetFromDelimitedName(string delimited)
	    {
	        var items = delimited.Split(',');
	        var a = Assembly.LoadFrom(items[0].Trim());
	        return a.GetType(delimited);
	    }

	    /// <summary>
        /// Checks whether the specified type implements the specified interface
        /// </summary>
        /// <param name="type">Type to check for implementation</param>
        /// <param name="iface">Interface type</param>
        /// <returns>True if the type implements the interface</returns>
        private static bool IsImplementationOfInterface(Type type, Type iface)
        {
            if (!iface.IsInterface)
                throw new ArgumentException("iface should be an interface type", "iface");

            var interfaceList = type.GetInterfaces();
            foreach (Type interfaceType in interfaceList)
            {
                if (iface == interfaceType)
                    return true;
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == iface)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Look up a data provider by the interface and business object
        /// </summary>
        /// <typeparam name="ProviderInterface">Provider interface</typeparam>
        /// <typeparam name="BusinessObject">Business object class</typeparam>
        /// <returns></returns>
        public ProviderInterface Get<ProviderInterface,BusinessObject>() 
            where BusinessObject : class //, new() //IDataEntity,
            where ProviderInterface : IDataProviderBase<BusinessObject>
        {
            var key = typeof(ProviderInterface);
            if (!providerTypes.ContainsKey(key))
            {
                throw new ApplicationException("No data provider found for : '" + typeof(ProviderInterface).FullName + "'");
            }

            if (!providerInstances.ContainsKey(key))
                providerInstances[key] = Activator.CreateInstance(providerTypes[key]);

            return (ProviderInterface) providerInstances[key];
        }

        /// <summary>
        /// Generate a number from the specified number sequence
        /// </summary>
        /// <typeparam name="ProviderInterface">Provider interface</typeparam>
        /// <typeparam name="BusinessObject">Business object class</typeparam>
        /// <param name="entry">The database entry</param>
        /// <returns>New sequence id</returns>
	    public RecordIdentifier GenerateNumber<ProviderInterface,BusinessObject>(IConnectionManager entry) 
            where BusinessObject : class //, new() //IDataEntity,
            where ProviderInterface : IDataProviderBase<BusinessObject>, ISequenceable
	    {
	        return GenerateNumber(entry, Get<ProviderInterface, BusinessObject>());
	    }

        /// <summary>
        /// Generate a number from the specified number sequence
        /// </summary>
        /// <param name="entry">The database entry</param>
        /// <param name="sequence">ISequencable instance</param>
        /// <returns>New sequence id</returns>
        public RecordIdentifier GenerateNumber(IConnectionManager entry, ISequenceable sequence)
	    {
            return Get<INumberSequenceData, NumberSequence>().GenerateNumberFromSequence(entry, sequence);
	    }

        /// <summary>
        /// Generate a list of numbers from the specified number sequence
        /// </summary>
        /// <typeparam name="ProviderInterface">Provider interface</typeparam>
        /// <typeparam name="BusinessObject">Business object class</typeparam>
        /// <param name="entry">The database entry</param>
        /// <param name="noOfRecords">Number of records to return</param>
        /// <returns>A list of sequence IDs</returns>
	    public List<RecordIdentifier> GenerateNumbers<ProviderInterface, BusinessObject>(IConnectionManager entry, int noOfRecords)
            where BusinessObject : class 
            where ProviderInterface : IDataProviderBase<BusinessObject>, ISequenceable
        {
            return GenerateNumbers(entry, Get<ProviderInterface, BusinessObject>(), noOfRecords);
        }

        /// <summary>
        /// Generate a list of numbers from the specified number sequence
        /// </summary>
        /// <param name="entry">The database entry</param>
        /// <param name="sequence">ISequencable instance</param>
        /// <param name="noOfRecords">Number of records to return</param>
        /// <returns>New sequence id</returns>
        public List<RecordIdentifier> GenerateNumbers(IConnectionManager entry, ISequenceable sequence, int noOfRecords)
        {
            return Get<INumberSequenceData, NumberSequence>().GenerateNumbersFromSequence(entry, sequence, noOfRecords);
        }
    }	
}
