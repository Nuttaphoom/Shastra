using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Vanaring
{
    public class PersistentAddressableResourceLoader : PersistentInstantiatedObject<PersistentAddressableResourceLoader>
    {
        public ResourcceType LoadResourceOperation<ResourcceType>(string address)
        {
            AsyncOperationHandle<ResourcceType> opHandler = Addressables.LoadAssetAsync<ResourcceType>(address);

            // Wait until the operation is done
            opHandler.WaitForCompletion();

            if (opHandler.Status != AsyncOperationStatus.Succeeded)
            {
                throw new Exception("Resource is NOT successfully loaded");
            }

            // Return the result
            return opHandler.Result;
        }

    }
}
