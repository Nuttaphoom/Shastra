using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vanaring
{
    public class DatabaseAddressLocator
    {
        private const string SpellDatabaseAddress = "SpellDatabaseSOAddress";
        private const string InventoryDatabaseAddress = "InventoryDatabaseSOAddress";
        private const string CharacterSheetDatabaseSOAddress = "CharacterSheetDatabaseSOAddress";
        private const string TuitorialDatabaseSOAddress = "TuitorialDatabaseSOAddress"; 
        public static string GetSpellDatabaseAddress => SpellDatabaseAddress; 
        public static string GetInventoryDatabaseAddress => InventoryDatabaseAddress;
        public static string GetCharacterSheetDatabaseAddress => CharacterSheetDatabaseSOAddress;
        public static string GetTuitorialDatabaseSOAddress => TuitorialDatabaseSOAddress; 

        public static SpellDatabaseSO _spellDatabaseSO;
        public static SpellDatabaseSO GetSpellDatabase
        {
            get
            {
                if (_spellDatabaseSO == null)
                {
                    _spellDatabaseSO  = PersistentAddressableResourceLoader.Instance.LoadResourceOperation<SpellDatabaseSO>(SpellDatabaseAddress);
                }
                return _spellDatabaseSO;
            }
        }

    }
}
