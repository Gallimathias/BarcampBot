using BarcampBot.IoC.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BarcampBot.IoC.Locators
{
    public sealed class CoreServiceLocator : IServiceLocator
    {
        private static readonly CoreServiceLocator serviceLocator;

        private readonly Dictionary<Type, object> serviceInstances;
        private readonly Dictionary<string, Type> serviceRegister;

        static CoreServiceLocator()
        {
            serviceLocator = new CoreServiceLocator();
        }

        private CoreServiceLocator()
        {
            serviceInstances = new Dictionary<Type, object>();

            serviceRegister = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes().Where(t => typeof(IService).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface))
                .ToDictionary(t => t.FullName);

        }

        public object GetService(Type serviceType)
        {
            if (serviceType == null)
                return null;

            if (serviceType.IsAbstract || serviceType.IsInterface)
                serviceType = serviceRegister.Values.FirstOrDefault(t => serviceType.IsAssignableFrom(t));

            if (serviceType == null)
                return null;

            if (serviceInstances.TryGetValue(serviceType, out object instance))
                return instance;


            var tmpInstance = Activator.CreateInstance(serviceType);
            serviceInstances.Add(serviceType, tmpInstance);
            return tmpInstance;

        }
        public object GetService(string name) => GetService(serviceRegister[name]);


        public static IService GetServiceInstance(Type serviceType) => (IService)serviceLocator.GetService(serviceType);
        public static T GetServiceInstance<T>() where T : IService => (T)serviceLocator.GetService(typeof(T));

        public static void RegisterAssembly(string assemblyName)
        {
            List<Type> tmpList;
            var fileInfo = new FileInfo(assemblyName);

            try
            {
                tmpList = Assembly
                      .LoadFile(fileInfo.FullName)
                      .GetTypes()
                      .Where(t => typeof(IService).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
                      .ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return;
            }

            foreach (var item in tmpList)
                RegisterService(item);
        }

        public static void RegisterAssemblys(string folderPath, SearchOption option)
        {
            var folder = new DirectoryInfo(folderPath);
            var files = folder.GetFiles("*.dll", option);

            foreach (var file in files)
                RegisterAssembly(file.FullName);
        }
        public static void RegisterAssemblys(string folderPath)
            => RegisterAssemblys(folderPath, SearchOption.AllDirectories);

        public static void RegisterService(Type type)
        {
            if (!typeof(IService).IsAssignableFrom(type))
                throw new InvalidCastException($"Type {type.FullName} is not a valid Service");

            if (serviceLocator.serviceRegister.ContainsKey(type.FullName))
                return;

            serviceLocator.serviceRegister.Add(type.FullName, type);
        }
        public static void RegisterService<Type>() where Type : IService
            => RegisterService(typeof(Type));
    }
}
