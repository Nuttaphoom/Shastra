using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring 
{
    public class LoaderDataContainer : MonoBehaviour
    {
        [SerializeField]
        private object _data;

        public object UseDataInContainer()
        {
            StartCoroutine(DataUsedCompletion());
            return _data;
        }

        private IEnumerator DataUsedCompletion()
        {
            yield return new WaitForSecondsRealtime(2.0f);

            Destroy(gameObject);
        }
        public void SetDataUser<T> (LoaderDataUser<T> loaderDataUser) 
        {
            _data = loaderDataUser; 
        }
    }

    public class LoaderDataUser
    {

    }

    public class LoaderDataUser<T>
    {
        private T _data; 
        public LoaderDataUser(T data) {
            _data = data; 
        } 

        public T GetData()
        {
            return _data; 
        }
    }

    
}
