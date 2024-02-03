using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Vanaring
{
    public class PersistentSaveLoadManager : PersistentInstantiatedObject<PersistentSaveLoadManager>
    { 
        private List<string> savePath = new List<string>();

        [ContextMenu("Save All")]
        private void SaveAll()
        {
            GetFilesPath();
            foreach (string path in savePath)
            {
                var state = LoadFile(path);

                CaptureState(state, path);
                SaveFile(state, path);
            }
        }

        //private void Save()
        //{
        //    var state = LoadFile();

        //    CaptureState(state);
        //    SaveFile(state);
        //}

        [ContextMenu("Load All")]
        private void LoadAll()
        {
            GetFilesPath();
            foreach (string path in savePath)
            {
                var state = LoadFile(path);

                RestoreState(state, path);
            }
        }
        //private void Load()
        //{
        //    var state = LoadFile();

        //    RestoreState(state);
        //}

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
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                string path = FileNameToPath(saveable.fileName);
                if (savePath.Contains(path))
                    continue;

                savePath.Add(path);
            }
        }

        private void CaptureState(Dictionary<string, object> state, string filepath)
        {
            foreach (var saveable in FindObjectsOfType<SaveableEntity>())
            {
                if (filepath != FileNameToPath(saveable.fileName))
                    continue;

                state[saveable.Id] = saveable.CaptureState();
            }
        }

        private void RestoreState(Dictionary<string, object> state, string filepath)
        {
            foreach (var saveable in FindObjectsOfType<SaveableEntity>())
            {
                if (filepath != FileNameToPath(saveable.fileName))
                    continue;

                if (state.TryGetValue(saveable.Id, out object value))
                {
                    saveable.RestoreState(value);
                }
            }
        }

        private string FileNameToPath(string fileName)
        {
            string path = Application.persistentDataPath + "/" + fileName;
            return path;
        }
    }
}
