using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChartTest.Common;

namespace ChartTest
{
    public class DomainObject : BindableBase
    {
        Double _value;
        public Double Value
        {
            get { return _value; }
            set { this.SetProperty(ref this._value, value); }
        }

        string _text;
        public string Text
        {
            get { return _text; }
            set { this.SetProperty(ref this._text, value); }
        }
    }
}
