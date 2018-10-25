using System;
using System.Collections.Generic;

using UrbanRivalsCore.Model;

namespace UrbanRivalsManager.ViewModel.DataManagement
{
    public interface IDatabaseManager
    {
        void purge();

        CardBase getCardBase(int id);
        IEnumerable<int> getAllCardBaseIds();
        void storeCardBase(CardBase card);
    }
}
