using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Vanaring
{
    public class PersistentSaveLoadManager : PersistentInstantiatedObject<PersistentSaveLoadManager>
    {
        [SerializeField]
        private List<string> listOfFileName = new List<string>(); // temp assign all save filename in the whole game

        private List<string> savePath = new List<string>();

        private Dictionary<string, Dictionary<string, object>> temporaryLoader = new Dictionary<string, Dictionary<string, object>>();

        [ContextMenu("Save to Disc")]
        public void SaveToDisc()
        {
            LoadToTemp();
            foreach (string path in savePath)
            {
                //var state = LoadFile(path);

                //CaptureState(state, path);
                SaveFile(temporaryLoader[path], path);
            }
        }

        public void CaptureToTemp()
        {
            //GetFilesPath();
            foreach (string path in savePath)
            {
                CaptureState(path);
            }
        }

        private void Awake()
        {
            GetFilesPath();
            LoadToTemp();
        }

        //public void Save(string filepath)
        //{
        //    var state = LoadFile(filepath);

        //    CaptureState(state, filepath);
        //    SaveFile(state, filepath);
        //}

        [ContextMenu("Load to Temp")]
        public void LoadToTemp()
        {
            //GetFilesPath();
            foreach (string path in savePath)
            {
                Dictionary<string, object> state = LoadFile(path);
                AddDataToTemp(path, state);
            }
        }

        [ContextMenu("Restore From Temp")]
        public void RestoreFromTemp()
        {
            //GetFilesPath();
            foreach (string path in savePath)
            {
                RestoreState(path);
            }
        }

        //public void Load(string filepath)
        //{
        //    var state = LoadFile(filepath);

        //    RestoreState(state, filepath);
        //}

        public void AddDataToTemp(string path, Dictionary<string, object> state)
        {
            if (temporaryLoader.ContainsKey(path) == true)
            {
                temporaryLoader[path] = state;
            }
            else
            {
                temporaryLoader.Add(path, state);
            }
        }

        private Dictionary<string, object> LoadFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new Dictionary<string, object>();
            }

            using (FileStream stream = File.Open(filePath, FileMode.Open))
            {
                var formatter = new BinaryFormatter();
                return (Dictionary<string, object>)formatter.Deserialize(stream);
            }
        }

        private void SaveFile(object state, string filePath)
        {
            using (var stream = File.Open(filePath, FileMode.Create))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            }
        }

        private void GetFilesPath()
        {
            foreach (string fileName in listOfFileName)
            {
                string path = FileNameToPath(fileName);
                if (savePath.Contains(path))
                    continue;

                savePath.Add(path);
            }
        }

        private void CaptureState(/*Dictionary<string, object> state,*/ string filepath)
        {
            foreach (var saveable in FindObjectsOfType<SaveableEntity>())
            {
                if (filepath != FileNameToPath(saveable.fileName))
                    continue;

                temporaryLoader[filepath][saveable.Id] = saveable.CaptureState();
                //state[saveable.Id] = saveable.CaptureState();
            }
        }

        private void RestoreState(/*Dictionary<string, object> state, */string filepath)
        {
            foreach (var saveable in FindObjectsOfType<SaveableEntity>())
            {
                if (filepath != FileNameToPath(saveable.fileName))
                    continue;

                if (temporaryLoader[filepath].TryGetValue(saveable.Id, out object value))
                {
                    saveable.RestoreState(value);
                }
            }
        }

        private string FileNameToPath(string fileName)
        {
            string path = Application.persistentDataPath + "/" + fileName + ".txt";
            return path;
        }
    }
}
