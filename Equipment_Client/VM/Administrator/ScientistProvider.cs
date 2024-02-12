using AutoCompleteTextBox.Editors;
using Equipment_Client.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Equipment_Client.VM.Administrator
{
    public class ScientistProvider<T> : ISuggestionProvider where T : Scientist
    {
        private readonly ObservableCollection<T> scientists;

        public IEnumerable GetSuggestions(string filter)
        {
            return scientists.Where(x => x.FIO.StartsWith(filter, StringComparison.InvariantCultureIgnoreCase));
        }

        public ScientistProvider(List<T> scientists)
        {
            this.scientists = new ObservableCollection<T>(scientists);
        }
    }
}
