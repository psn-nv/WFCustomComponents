using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestApp
{
    public class ComboBoxCheckItem
    {
        public int Value { get; set; }        
        public string Name { get; set; }       

        public ComboBoxCheckItem()
        {
        }

        public ComboBoxCheckItem(string name, int val)
        {
            Name = name;
            Value = val;
        }

        public override string ToString()
        {
            return $"Name:{Name}, Value:{Value}";
        }

    }
}
