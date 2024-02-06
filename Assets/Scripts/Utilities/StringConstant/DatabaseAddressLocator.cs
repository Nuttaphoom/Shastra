using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vanaring.Assets.Scripts.Utilities.StringConstant
{
    public class DatabaseAddressLocator
    {
        private const string SpellDatabaseAddress = "SpellDatabaseSOAddress";
        private const string InventoryDatabaseAddress = "InventoryDatabaseSOAddress";

        public static string GetSpellDatabaseAddress => SpellDatabaseAddress; 
        public static string GetInventoryDatabaseAddress => InventoryDatabaseAddress; 
    }
}
