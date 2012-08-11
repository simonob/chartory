using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ChartTest.Common;

namespace ChartTest
{
    public class MainPageViewModel
    {
        public IEnumerable<Double> Doubles { get; set; }
        public ObservableCollection<DomainObject> Objects { get; set; }

        private ICommand _changeValueCommand;
        public ICommand ChangeValueCommand
        {
            get { return _changeValueCommand; }
        }

        private ICommand _addValueCommand;
        public ICommand AddValueCommand
        {
            get { return _addValueCommand; }
        }

        private ICommand _removeValueCommand;
        public ICommand RemoveValueCommand
        {
            get { return _removeValueCommand; }
        }

        public MainPageViewModel()
        {
            Doubles = new double[] { 30, 65, 34, 11, 50, 80, 90 };

            Objects = new ObservableCollection<DomainObject>()
            {
                new DomainObject() { Value = 1000, Text = "Item 1"},
                new DomainObject() { Value = 50, Text = "Item 2"},
                new DomainObject() { Value = 500, Text = "Item 3"},
                new DomainObject() { Value = 30, Text = "Item 4"},
                new DomainObject() { Value = 22, Text = "Item 5"},
                new DomainObject() { Value = 788, Text = "Item 6"},
                new DomainObject() { Value = 988, Text = "Item 7"},
                new DomainObject() { Value = 566, Text = "Item 8"},
            };

            _changeValueCommand = new DelegateCommand(updateExistingItems);
            _addValueCommand = new DelegateCommand(addItem);
            _removeValueCommand = new DelegateCommand(removeItem);
        }


        private void updateExistingItems(object obj)
        {
            Objects[getRandomItemIndex()].Value += 50;
        }

        private void addItem(object obj)
        {
            var dt = DateTime.Now;
            Double newValue = dt.Millisecond;

            if (Objects.Count >0 && newValue < Objects.Min((x) => x.Value))
                newValue += Objects.Min((x) => x.Value);

            Objects.Add(new DomainObject()
            { 
                Text = string.Format("Item created at {0:HH:mm:ss}", dt), 
                Value =newValue
            });
        }

        private void removeItem(object obj)
        {
            if (Objects.Count > 0)
                Objects.RemoveAt(getRandomItemIndex());
        }

        int getRandomItemIndex()
        {
            var listLength = Objects.Count;
            var sec = DateTime.Now.Millisecond;
            if (sec == 0)
                sec = 1;

            var randomItemIndex = sec % listLength;
            return randomItemIndex;
        }



    }
}
