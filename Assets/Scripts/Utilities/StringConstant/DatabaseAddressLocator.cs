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
        private const string CharacterSheetDatabaseSOAddress = "CharacterSheetDatabaseSOAddress";
        public static string GetSpellDatabaseAddress => SpellDatabaseAddress; 
        public static string GetInventoryDatabaseAddress => InventoryDatabaseAddress;
        public static string GetCharacterSheetDatabaseAddress => CharacterSheetDatabaseSOAddress;

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
