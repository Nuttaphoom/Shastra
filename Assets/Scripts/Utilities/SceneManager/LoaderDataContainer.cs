using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring 
{
    //public class LoaderDataContainer : MonoBehaviour
    //{
    //    private object _data;

    //    //public object UseDataInContainer()
    //    //{
    //    //    if (_data == null)
    //    //        throw new Exception("_data is null");

    //    //    StartCoroutine(DataUsedCompletion());
            
    //    //    return _data;
    //    //}

    //    public void TestStuff()
    //    {

    //    }
    //    private IEnumerator DataUsedCompletion()
    //    {
    //        yield return new WaitForSecondsRealtime(2.0f);

    //        Destroy(gameObject);
    //    }
    //    public void SetDataUser<T> (LoaderDataUser<T> loaderDataUser) 
    //    {
    //        _data = loaderDataUser;
    //        Debug.Log("Set data useer as " + typeof(T)); 

    //    }
    //}

    public class LoaderDataUser
    {

    }

    public class LoaderDataUser<T> : LoaderDataUser
    {
        private T _data; 
        public LoaderDataUser(T data) {
            _data = data; 
        } 

        public T GetData()
        {
            if (_data == null)
                throw new Exception("_Data is null");
            return _data; 
        }
    }

    
}
