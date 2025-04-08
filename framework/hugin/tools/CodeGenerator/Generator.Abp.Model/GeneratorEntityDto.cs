using System.Collections.ObjectModel;

namespace Generator
{
    public class GeneratorEntityDto : NotifyPropertyChangedObject
    {
        private string _displayName;
        public string DisplayName
        {
            get => _displayName;
            set
            {
                _displayName = value;
                InvokePropertyChanged(nameof(DisplayName));
            }
        }

        private ObservableCollection<GeneratorEntityDtoProperty> _properties;
        public ObservableCollection<GeneratorEntityDtoProperty> Properties
        {
            get => _properties;
            set
            {
                _properties = value;
                InvokePropertyChanged(nameof(Properties));
            }
        }
    }
}
