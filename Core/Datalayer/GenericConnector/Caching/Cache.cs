using System;
using System.Collections.Generic;
using System.Linq;

using LSOne.DataLayer.GenericConnector.DataEntities;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities.DataTypes;

namespace LSOne.DataLayer.GenericConnector.Caching
{
	public class Cache : ICache
	{
		List<string> namePrefixes;
		List<Country> countries;
		Dictionary<RecordIdentifier, List<IDataEntity>> states;
		Dictionary<string,string> countryMapper;
		Dictionary<string,DecimalLimit> decimals;
		WeakReference dataModel;
		ICacheProvider cacheProvider;

		Dictionary<Type,Dictionary<string,IDataEntity>>[] entityCache;

		public Cache()
		{
			countries = null;
			states = null;
			namePrefixes = null;
			decimals = null;

			countryMapper = null;

			entityCache = new Dictionary<Type, Dictionary<string, IDataEntity>>[2];

			entityCache[(int) CacheType.CacheTypeTransactionLifeTime] = new Dictionary<Type, Dictionary<string, IDataEntity>>();
			entityCache[(int) CacheType.CacheTypeApplicationLifeTime] = new Dictionary<Type, Dictionary<string, IDataEntity>>();
		}

		public void SetCacheProvider(ICacheProvider provider)
		{
			cacheProvider = provider;
		}

		public IConnectionManager DataModel
		{
			get
			{
				return dataModel != null && dataModel.IsAlive 
					? (IConnectionManager)dataModel.Target 
					: null;
			}
			set
			{
				dataModel = new WeakReference(value);
			}
		}
		

		#region ICache Members

        public void InvalidateEntityCache()
        {
            ClearEntityCache(CacheType.CacheTypeApplicationLifeTime);
            ClearEntityCache(CacheType.CacheTypeTransactionLifeTime);            
        }

        public void ClearEntityCache(CacheType cacheType)
		{
			if (cacheType != CacheType.CacheTypeNone)
			{
				entityCache[(int)cacheType].Clear();
			}
		}

		public IDataEntity GetEntityFromCache(Type type, RecordIdentifier id)
		{
			string key = id.GetSignature();

			if (entityCache[(int)CacheType.CacheTypeApplicationLifeTime].ContainsKey(type) && entityCache[(int)CacheType.CacheTypeApplicationLifeTime][type].ContainsKey(key))
			{
				return entityCache[(int)CacheType.CacheTypeApplicationLifeTime][type][key];
			}

			if (entityCache[(int)CacheType.CacheTypeTransactionLifeTime].ContainsKey(type) && entityCache[(int)CacheType.CacheTypeTransactionLifeTime][type].ContainsKey(key))
			{
				return entityCache[(int)CacheType.CacheTypeTransactionLifeTime][type][key];
			}

			return null;
		}

		public void AddEntityToCache(RecordIdentifier id, IDataEntity entity, CacheType cacheType)
		{
			if (cacheType != CacheType.CacheTypeNone)
			{
				Type type = entity.GetType();
				if (!entityCache[(int) cacheType].ContainsKey(type))
				{
					entityCache[(int) cacheType].Add(type, new Dictionary<string, IDataEntity>());
				}
				entityCache[(int)cacheType][type].Add(id.GetSignature(), entity);
			}
		}

		public void UpdateEntityInCache(IDataEntity entity)
		{
			Type type = entity.GetType();
			string key = entity.ID.GetSignature();

			if (entityCache[(int)CacheType.CacheTypeApplicationLifeTime].ContainsKey(type) && entityCache[(int) CacheType.CacheTypeApplicationLifeTime][type].ContainsKey(key))
			{
				entityCache[(int)CacheType.CacheTypeApplicationLifeTime][type][key] = entity;
			}

			if (entityCache[(int)CacheType.CacheTypeTransactionLifeTime].ContainsKey(type) && entityCache[(int)CacheType.CacheTypeTransactionLifeTime][type].ContainsKey(key))
			{
				entityCache[(int)CacheType.CacheTypeTransactionLifeTime][type][key] = entity;
			}
		}

		public void DeleteEntityFromCache(Type type, RecordIdentifier id)
		{
			string key = id.GetSignature();

			if (entityCache[(int) CacheType.CacheTypeApplicationLifeTime].ContainsKey(type) && entityCache[(int) CacheType.CacheTypeApplicationLifeTime][type].ContainsKey(key))
			{
				entityCache[(int) CacheType.CacheTypeApplicationLifeTime][type].Remove(key);
			}

			if (entityCache[(int)CacheType.CacheTypeTransactionLifeTime].ContainsKey(type) && entityCache[(int)CacheType.CacheTypeTransactionLifeTime][type].ContainsKey(key))
			{
				entityCache[(int)CacheType.CacheTypeTransactionLifeTime][type].Remove(key);
			}
		}

		public void DeleteTypeFromCache(Type type)
		{
			if (entityCache[(int) CacheType.CacheTypeApplicationLifeTime].ContainsKey(type))
			{
				entityCache[(int) CacheType.CacheTypeApplicationLifeTime].Remove(type);
			}

			if (entityCache[(int) CacheType.CacheTypeTransactionLifeTime].ContainsKey(type))
			{
				entityCache[(int) CacheType.CacheTypeTransactionLifeTime].Remove(type);
			}
		}

		public void InvalidateSystemDecimalCache()
		{
			decimals = null;
		}

		public Dictionary<string, DecimalLimit> GetSystemDecimals()
		{
			return GetSystemDecimals(DataModel);
		}

		public Dictionary<string, DecimalLimit> GetSystemDecimals(IConnectionManager entry)
		{
			if (decimals == null && cacheProvider != null)
			{
				decimals = LoadData(entry, cacheProvider.GetSystemDecimals);
			}
				
			return decimals;
		}

		public List<string> GetNamePrefixes()
		{
			return GetNamePrefixes(DataModel);
		}

		public List<string> GetNamePrefixes(IConnectionManager entry)
		{
			if (namePrefixes == null && cacheProvider != null)
			{
				namePrefixes = LoadData(entry, cacheProvider.GetNamePrefixes);
			}
			
			return namePrefixes;
		}


		public List<Country> GetCountries()
		{
			return GetCountries(DataModel);
		}

		public List<Country> GetCountries(IConnectionManager entry)
		{
			if (countries == null && cacheProvider != null)
			{
				countries = LoadData(entry, cacheProvider.GetCountries);
			}
			
			return countries;
		}

		public string GetCountryName(RecordIdentifier countryID)
		{
			List<Country> countryList;

			if (countryMapper == null)
			{
				countryMapper = new Dictionary<string, string>();
			}
			else
			{
				if (countryMapper.ContainsKey((string)countryID))
				{
					return countryMapper[(string)countryID];
				}
			}

			countryList = GetCountries();

			foreach (Country country in countryList)
			{
				if (country.ID == countryID)
				{
					countryMapper.Add((string)country.ID, country.Text);
					return country.Text;
				}
			}

			return string.Empty;
		}

		/// <summary>
		/// Returns the states of the given country using the existing <see cref="DataModel"/>.
		/// </summary>
		/// <param name="countryID">Country ID</param>
		/// <returns></returns>
		public List<IDataEntity> GetStates(RecordIdentifier countryID)
		{
			return GetStates(DataModel, countryID);
		}

		/// <summary>
		/// Returns the states of the given country. If they are not cached already they will be retrieved from database and cached.
		/// </summary>
		/// <param name="entry">Database connection</param>
		/// <param name="countryID"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">Country id is an invalid <see cref="RecordIdentifier"/>.</exception>
		public List<IDataEntity> GetStates(IConnectionManager entry, RecordIdentifier countryID)
		{
			if (RecordIdentifier.IsEmptyOrNull(countryID)) throw new ArgumentNullException(nameof(countryID));

			if(states == null)
			{
				states = new Dictionary<RecordIdentifier, List<IDataEntity>>();
			}

			if(!states.ContainsKey(countryID) && cacheProvider != null)
			{
				states.Add(countryID, LoadData(entry, cacheProvider.GetStates, countryID));
			}

			return states.ContainsKey(countryID) && states[countryID] != null
				? states[countryID]
				: new List<IDataEntity>();
		}

		/// <summary>
		/// Returns the state code (abbreviated name) for the given state name and country.
		/// </summary>
		/// <param name="countryID"></param>
		/// <param name="stateName"></param>
		/// <returns>A <see cref="RecordIdentifier"/> containing the state code (abbreviated name) or an empty <see cref="RecordIdentifier"/></returns>
		/// <exception cref="ArgumentNullException">Country id is invalid (null or empty <see cref="RecordIdentifier"/>).</exception>
		/// <exception cref="ArgumentNullException">State name is invalid (null or empty string).</exception>
		public RecordIdentifier GetStateCode(RecordIdentifier countryID, string stateName)
		{
			if (string.IsNullOrWhiteSpace(stateName)) throw new ArgumentNullException(nameof(stateName));

			List<IDataEntity> statesList = GetStates(countryID);
			IDataEntity foundState = null;
			if(statesList == null || statesList.Count > 0)
			{
				foundState = statesList.FirstOrDefault(sl => string.Compare(sl.Text, stateName, StringComparison.InvariantCultureIgnoreCase) == 0);
			}

			return foundState == null ? RecordIdentifier.Empty : foundState.ID;
		}

		#endregion
		
		#region Private methods

		/// <summary>
		/// Generic method for retrieving cached entities from data store on a temporary connection.
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="dataProvider"></param>
		/// <typeparam name="U">Type of the returned result.</typeparam>
		/// <returns></returns>
		private U LoadData<U>(IConnectionManager entry, Func<IConnectionManager, U> dataProvider)
		{
			return LoadData(entry, (connection, _) => dataProvider(connection), (object)null);
		}
		
		/// <summary>
		/// Generic method for retrieving cached entities from data store on a temporary connection.
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="dataProvider"></param>
		/// <param name="parameter1">Additional parameter to <paramref name="dataProvider"/> method.</param>
		/// <typeparam name="T1">Type of the additional parameter to <paramref name="dataProvider"/> method.</typeparam>
		/// <typeparam name="U">Type of the returned result.</typeparam>
		/// <returns></returns>
		private U LoadData<T1, U>(IConnectionManager entry,
							Func<IConnectionManager, T1, U> dataProvider, 
							T1 parameter1)
		{
			if (dataProvider == null)
			{
				return default(U);
			}

			IConnectionManagerTemporary tempEntry = null;
			try
			{
				tempEntry = entry.CreateTemporaryConnection();
				return dataProvider(tempEntry, parameter1);
			}
			finally
			{
				if (tempEntry != null)
				{
					tempEntry.Close();
					tempEntry = null;
				}
			}
		}
		
		#endregion
	}
}
