using System;
using System.Collections.Generic;
using System.Reflection;
using LSOne.DataLayer.GenericConnector.Enums;
using LSOne.DataLayer.GenericConnector.Interfaces;
using LSOne.Utilities;
using LSOne.Utilities.IO;

namespace LSOne.DataLayer.GenericConnector
{
    /// <summary>
    /// Handles creation and caching of LS One services
    /// </summary>
    public class ServiceFactory
    {

        private IDictionary<ServiceType, IService> services;
        private Dictionary<string, string> serviceOverrides;
        private Dictionary<ServiceType, bool> hasServiceCache;
        private FolderItem serviceBasePath;
        private object serviceLock;

        public event ResolveEventHandler ResolveServiceAssembly;

        public ServiceFactory()
        {
            hasServiceCache = new Dictionary<ServiceType, bool>();
            services = new Dictionary<ServiceType, IService>();
            serviceOverrides = new Dictionary<string, string>();
            serviceLock = new object();
        }

        /// <summary>
        /// Gets or sets the location of the service assemblies. This will be the path to the folder used when loading service instances.
        /// The default path returned is "(ApplicationStartup.BasePath)\Services"
        /// </summary>
        public FolderItem ServiceBasePath
        {
            get
            {
                if (serviceBasePath == null)
                {
                    serviceBasePath = (new FolderItem(ApplicationStartup.BasePath)).Child("Services");
                }

                return serviceBasePath;
            }
            set
            {
                serviceBasePath = value;
            }
        }

        /// <summary>
        /// Indicates wether an instance has been created/loaded for the given service type
        /// </summary>
        /// <param name="serviceType">The type of service to check for</param>
        /// <returns></returns>
        public bool ServiceIsLoaded(ServiceType serviceType)
        {
            lock (serviceLock)
            {
                if (services == null)
                {
                    return false;
                }

                if (services.ContainsKey(serviceType))
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Sets the service instance of the given <paramref name="serviceType"/> to the instance of <paramref name="service"/>"
        /// </summary>
        /// <param name="serviceType">The type of servie to set the instance for</param>
        /// <param name="service">The instance of the servie to set</param>
        public virtual void SetService(ServiceType serviceType, IService service)
        {
            lock (serviceLock)
            {
                if (services == null)
                {
                    services = new Dictionary<ServiceType, IService>();
                }

                services[serviceType] = service;
            }
        }

        /// <summary>
        /// Removes an instance of a service if it has been loaded
        /// </summary>
        /// <param name="serviceType">The type of service to remove</param>
        public void UnloadService(ServiceType serviceType)
        {
            lock (serviceLock)
            {
                if (services != null)
                {
                    if (services.ContainsKey(serviceType))
                    {
                        services.Remove(serviceType);
                    }                    
                }
            }
        }

        /// <summary>
        /// Indicates wether the factory has a loaded instance for the given service type or if the given service type
        /// can be loaded from an assembly
        /// </summary>
        /// <param name="serviceType">The type of service to check for</param>
        /// <returns></returns>
        public bool HasService(ServiceType serviceType)
        {
            lock (serviceLock)
            {
                FolderItem serviceItem = null;
                Assembly assembly = null;

                if (hasServiceCache.ContainsKey(serviceType))
                {
                    return hasServiceCache[serviceType];
                }

                if (services == null)
                {
                    services = new Dictionary<ServiceType, IService>();
                }

                if (services.ContainsKey(serviceType))
                {
                    hasServiceCache.Add(serviceType, true);
                    return true;
                }


                // We need to load the service
                FolderItem servicesDirectory = ServiceBasePath;


                string serviceEnumName = Enum.GetName(typeof (ServiceType), serviceType);
                string svcName = serviceEnumName + "@";
                string serviceFileName = "";

                if (serviceOverrides.ContainsKey(serviceEnumName))
                {
                    serviceFileName = serviceOverrides[serviceEnumName];

                    serviceItem = servicesDirectory.Child(serviceFileName);

                    if (!serviceItem.Exists)
                    {
                        serviceItem = null; // We abort using a override since the requested one was not found
                    }
                }

                // If we didnt use Service overload or the service overload was not found then we fallback to the default one.
                if (serviceItem == null || serviceFileName == "")
                {
                    serviceFileName = "LSOne.Services." + svcName.Replace("Service@", "") + ".dll";

                    serviceItem = servicesDirectory.Child(serviceFileName);
                }


                if (!servicesDirectory.Exists)
                {
                    assembly = DoResolveServiceAssembly(this, new ResolveEventArgs(serviceFileName));
                    if (assembly == null)
                    {
                        hasServiceCache.Add(serviceType, false);
                        return false;
                    }
                }
                if ((serviceItem == null || !serviceItem.Exists) && assembly == null)
                {
                    assembly = DoResolveServiceAssembly(this, new ResolveEventArgs(serviceFileName));
                    if (assembly == null)
                    {
                        hasServiceCache.Add(serviceType, false);
                        return false;
                    }
                }

                hasServiceCache.Add(serviceType, true);
                return true;
            }
        }

        /// <summary>
        /// Overrides or sets an instance of a service with the given type and name. If an existing override exists ofr the given service type it will be overridden.
        /// </summary>
        /// <param name="serviceType">The type of service to override</param>
        /// <param name="overrideFullName">The full assembly name of the override. I.e "LSOne.Services.EFT.SomeProcessor.dll"</param>
        public void SetServiceOverride(ServiceType serviceType, string overrideFullName)
        {
            lock (serviceLock)
            {
                string serviceEnumName = Enum.GetName(typeof (ServiceType), serviceType);

                if (services != null)
                {
                    if (services.ContainsKey(serviceType))
                    {
                        services.Remove(serviceType);
                    }
                }

                if (serviceOverrides.ContainsKey(serviceEnumName))
                {
                    serviceOverrides.Remove(serviceEnumName);
                }

                serviceOverrides.Add(serviceEnumName, overrideFullName);
            }
        }

        /// <summary>
        /// Removes the override for the given <see cref="ServiceType"/> and unloads the current service of that type. The next time the service is called it will then load the default implementation.
        /// </summary>
        /// <param name="serviceType">The type of service to remove the override for</param>
        public void RemoveServiceOverride(ServiceType serviceType)
        {
            lock(serviceLock)
            {
                string serviceEnumName = Enum.GetName(typeof(ServiceType), serviceType);

                if(serviceOverrides.ContainsKey(serviceEnumName))
                {
                    serviceOverrides.Remove(serviceEnumName);

                    if (services != null && services.ContainsKey(serviceType))
                    {
                        services.Remove(serviceType);
                    }
                }                
            }
        }

        /// <summary>
        /// Creates and returns an instance of a service based on the given <see cref="ServiceType"/>
        /// </summary>
        /// <param name="serviceType">The type of service to create</param>
        /// <param name="entry">The instance of <see cref="IConnectionManager"/> that this service is being loaded for. This instance will be passed down to the <see cref="IService.Init"/> function</param>
        /// <returns></returns>
        public IService Service(ServiceType serviceType, IConnectionManager entry)
        {
            lock (serviceLock)
            {
                FolderItem serviceItem = null;
                Assembly assembly = null;
                Module[] moduleArray;
                Type[] types;
                string serviceClassName;
                System.Runtime.Remoting.ObjectHandle serviceHandle;
                IService service;

                if (services == null)
                {
                    services = new Dictionary<ServiceType, IService>();
                }

                if (services.ContainsKey(serviceType))
                {
                    return services[serviceType];
                }

                // We need to load the service
                FolderItem servicesDirectory = ServiceBasePath;


                string serviceEnumName = Enum.GetName(typeof (ServiceType), serviceType);
                string svcName = serviceEnumName + "@";
                string serviceFileName = "";

                if (serviceOverrides.ContainsKey(serviceEnumName))
                {
                    serviceFileName = serviceOverrides[serviceEnumName];

                    serviceItem = servicesDirectory.Child(serviceFileName);

                    if (!serviceItem.Exists)
                    {
                        serviceItem = null; // We abort using a override since the requested one was not found
                    }
                }

                // If we didnt use Service overload or the service overload was not found then we fallback to the default one.
                if (serviceItem == null || serviceFileName == "")
                {
                    serviceFileName = "LSOne.Services." + svcName.Replace("Service@", "") + ".dll";

                    serviceItem = servicesDirectory.Child(serviceFileName);
                }



                if (!servicesDirectory.Exists)
                {
                    assembly = DoResolveServiceAssembly(this, new ResolveEventArgs(serviceFileName));
                    if (assembly == null)
                    {
                        return null;
                    }
                }
                if ((serviceItem == null || !serviceItem.Exists) && assembly == null)
                {
                    assembly = DoResolveServiceAssembly(this, new ResolveEventArgs(serviceFileName));
                    if (assembly == null)
                    {
                        //TODO: somehow return this message or at least log it down
                        //MessageBox.Show(Properties.Resources.ServiceNotFound.Replace("#1", serviceFileName).Replace("#2", servicesDirectory.AbsolutePath), Properties.Resources.LoadingServices, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Stop);
                        return null;
                    }
                }

                try
                {
                    if (assembly == null)
                    {
                        assembly = Assembly.LoadFrom(serviceItem.AbsolutePath);
                    }

                    moduleArray = assembly.GetModules();

                    foreach (Module module in moduleArray)
                    {
                        types = module.GetTypes();

                        foreach (Type type in types)
                        {
                            if (type.GetInterface(typeof (IService).FullName) != null)
                            {
                                serviceClassName = type.FullName;

                                serviceHandle = System.Activator.CreateInstanceFrom(assembly.Location, serviceClassName, null);
                                service = (IService) serviceHandle.Unwrap();
                                service.Init(entry);
                                services.Add(serviceType, service);

                                return service;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                    return null;
                }

                return null;
            }
        }

        private Assembly DoResolveServiceAssembly(object sender, ResolveEventArgs args)
        {
            if (ResolveServiceAssembly != null)
            {
                return ResolveServiceAssembly(sender, args);
            }
            return null;
        }
    }
}
