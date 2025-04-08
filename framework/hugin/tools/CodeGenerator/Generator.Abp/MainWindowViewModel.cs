using Generator;

namespace AbpGenerator
{
    public class MainWindowViewModel : NotifyPropertyChangedObject
    {
        private GeneratorOptions _options;
        public GeneratorOptions Options
        {
            get => _options;
            set
            {
                _options = value;
                InvokePropertyChanged(nameof(Options));
            }
        }

        private GeneratorFeature _features;
        public GeneratorFeature Features
        {
            get => _features;
            set
            {
                _features = value;
                InvokePropertyChanged(nameof(Features));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public MainWindowViewModel()
        {
            Options = Global.Options;
            Features = Global.Features;
        }
    }
}