using System;
using System.IO;
using System.Reflection;
using LSOne.DataLayer.GenericConnector.Interfaces;

namespace LSOne.DataLayer.GenericConnector
{
    public class ConnectionManagerFactory
    {
        private static string connectionManager;

        public static string ConnectionManager
        {
            get
            {
                if (string.IsNullOrEmpty(connectionManager))
                    return DefaultManager;

                return connectionManager;
            }
            set { connectionManager = value; }
        }


        private static string DefaultManager
        {
            get
            {
                return "LSOne.DataLayer.SqlConnector,LSOne.DataLayer.SqlConnector.SqlServerConnectionManager";
            }
        }

        public static bool IsDefaultManager
        {
            get { return DefaultManager.Equals(connectionManager); }
        }

        public static IConnectionManager CreateConnectionManager()
        {
            var managerType = ConnectionManager.Split(',');
            if (managerType.Length != 2)
                throw new ApplicationException(string.Format("The ConnectionManager should be a reference to an assembly and a type, e.g. '{0}'",
                    DefaultManager));

            var assemblyFile = managerType[0];

            var assembly = TryLoad(assemblyFile + ".dll");

            if (assembly == null)
            {
                assembly = TryLoad(assemblyFile);
            }
            if (assembly == null)
                throw new ApplicationException(string.Format("Unable to load assembly '{0}' for Connection Manager", assemblyFile));

            var type = assembly.GetType(managerType[1]);
            if (type == null)
                throw new ApplicationException(string.Format("Unable to load type '{0}' from assembly '{1}' for Connection Manager",
                    managerType[1], managerType[0]));

            var mi = type.GetMethod("Create", BindingFlags.Static | BindingFlags.NonPublic);
            if (mi == null)
                throw new ApplicationException(string.Format("The connection manager '{0}, {1}', should have a method to initialize an instance of the manager\r\n\r\nprivate static IConnectionManager Create();",
                    managerType[0], managerType[1]));

            return mi.Invoke(null, BindingFlags.Static | BindingFlags.NonPublic, null, null, null) as IConnectionManager;
        }

        private static Assembly TryLoad(string assemblyFile)
        {
            try
            {
                return Assembly.LoadFrom(assemblyFile);
            }
            catch
            {
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                for (int i = 0; i < assemblies.Length; i++)
                {
                    if (assemblies[i].ManifestModule.Name.Equals(assemblyFile, StringComparison.OrdinalIgnoreCase))
                        return assemblies[i];
                }
                var fullPath = Path.Combine(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    assemblyFile);
                try
                {
                    return Assembly.LoadFrom(fullPath);
                }
                catch
                {
                    try
                    {
                        // Try Assembly.Load - the above LoadFrom will not work in a web environment (Integration Service)
                        // where assemblies are copied to the Temporary ASP.NET folder
                        var name = new AssemblyName(Path.GetFileNameWithoutExtension(fullPath));
                        return Assembly.Load(name);
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException("Unable to load " + fullPath, ex);
                    }
                }
            }
        }
    }
}
