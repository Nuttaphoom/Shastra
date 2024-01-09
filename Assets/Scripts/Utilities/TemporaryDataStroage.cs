using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vanaring_Utility_Tool 
{
    public class TemporaryDataStroageTool<DataType>
    {
        private DataType _storedValue;

        private object _secondStoreValue;
        private Type _secondValueType;

        public TemporaryDataStroageTool(DataType storedValue)
        {
            _storedValue = storedValue;
        }

        public void AssignSecondStoreValue<SecondDataType>(SecondDataType v)
        {
            _secondStoreValue = v;
            _secondValueType = typeof(SecondDataType);
        }

        public DataType GetStoredValue
        {
            get
            {
                if (_storedValue == null)
                    throw new Exception("_storedValue has never been stored");

                return _storedValue;
            }
        }

        public SecondDataType GetSecondStoredValue<SecondDataType>()
        {
            if (_secondStoreValue == null)
                throw new Exception("_secondStoreValue has never been stored");

            if (typeof(SecondDataType) != _secondValueType)
                throw new InvalidOperationException("Stored type does not match requested type");

            return (SecondDataType)_secondStoreValue;
        }

        public Type GetSecondStoredValueType()
        {
            if (_secondValueType == null)
                throw new Exception("_secondValueType has never been stored");

            return _secondValueType;
        }
    }


}
