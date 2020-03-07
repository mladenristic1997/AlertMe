using AlertMe.Domain.Events;
using Newtonsoft.Json;
using Prism.Events;
using System;
using System.IO;

namespace AlertMe.Domain
{
    public interface ILocalDataStore
    {
        T GetObject<T>() where T : class;
        void StoreObject<T>(T obj) where T : class;
        T GetObject<T>(string fileName) where T : class;
        void StoreObject<T>(T obj, string fileName) where T : class;
        void RemoveDirectory(string fileName);
    }

    public class LocalDataStore : ILocalDataStore
    {
        readonly IEventAggregator EventAggregator;

        public LocalDataStore(IEventAggregator ea)
        {
            EventAggregator = ea;
        }

        public T GetObject<T>() where T : class
        {
            string path = GetFilePath<T>();
            if (!File.Exists(path))
                return default(T);
            string data = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(data);
        }

        public void StoreObject<T>(T config) where T : class
        {
            string path = GetFilePath<T>();
            string json = JsonConvert.SerializeObject(config);
            File.WriteAllText(path, json);
            EventAggregator.GetEvent<LocalStoreChanged>().Publish();
        }

        static string GetFilePath<T>()
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/AlertMe";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var filename = $"{typeof(T).Name}.json";
            string path = $"{folder}/{filename}";
            return path;
        }

        public T GetObject<T>(string fileName) where T : class
        {
            string path = GetFilePath(fileName);
            if (!File.Exists(path))
                return default(T);
            string data = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(data);
        }

        public void StoreObject<T>(T config, string fileName) where T : class
        {
            string path = GetFilePath(fileName);
            string json = JsonConvert.SerializeObject(config);
            File.WriteAllText(path, json);
            EventAggregator.GetEvent<LocalStoreChanged>().Publish();
        }

        public void RemoveDirectory(string directoryName)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/AlertMe/" + directoryName;
            var directory = new DirectoryInfo(path) { Attributes = FileAttributes.Normal };
            foreach (var childInfo in directory.GetFileSystemInfos())
                childInfo.Delete();
            directory.Delete();
            EventAggregator.GetEvent<LocalStoreChanged>().Publish();
        }

        static string GetFilePath(string fileName)
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/AlertMe/" + fileName;
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            var filename = $"{fileName}.json";
            string path = $"{folder}/{filename}";
            return path;
        }
    }
}