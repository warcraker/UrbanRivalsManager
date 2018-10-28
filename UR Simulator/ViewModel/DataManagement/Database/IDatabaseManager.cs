using System;
using System.Collections.Generic;

using UrbanRivalsCore.Model;

namespace UrbanRivalsManager.ViewModel.DataManagement
{
    public interface IDatabaseManager
    {
        void purge();

        CardDefinition getCardDefinitionById(int id);
        IEnumerable<int> getAllCardDefinitionIds();
        void storeCardDefinition(CardDefinition card);
    }
}
