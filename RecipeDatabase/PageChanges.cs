using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeDatabase
{
    internal class PageChanges
    {
        public int selectedId = 0;
        public bool selectedIdChanged = false;

        public PageChanges(int selectedId=0,bool selectedIdChanged=false)
        {
            this.selectedId = selectedId;
            this.selectedIdChanged = selectedIdChanged;
        }

        public bool IsChanged(int newId)
        {
            bool result = false;
            if(selectedId!=newId)
                result = true;
            selectedId = newId;

            return result;
        }
    }
}
