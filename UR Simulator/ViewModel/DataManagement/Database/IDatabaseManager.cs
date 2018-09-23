using System;
using System.Collections.Generic;

using UrbanRivalsCore.Model;

namespace UrbanRivalsManager.ViewModel.DataManagement
{
    public interface IDatabaseManager
    {
        void Purge();

        CardBase GetCardBase(int id);
        IEnumerable<int> GetAllCardBaseIds();
        bool StoreCardBase(CardBase card);
        bool StoreCardBase(List<CardBase> list);
    }
}
