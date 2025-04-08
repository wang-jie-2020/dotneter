namespace Generator
{
    public class GeneratorOptions : NotifyPropertyChangedObject
    {
        private bool _isChooseAll;
        public bool IsChooseAll
        {
            get => _isChooseAll;
            set
            {
                _isChooseAll = value;
                SetAll();
            }
        }

        private bool _isOverride;
        public bool IsOverride
        {
            get => _isOverride;
            set
            {
                _isOverride = value;
                InvokePropertyChanged(nameof(IsOverride));
            }
        }

        private bool _useApplication;
        public bool UseApplication
        {
            get => _useApplication;
            set
            {
                _useApplication = value;
                CheckIsChooseAll();
                InvokePropertyChanged(nameof(UseApplication));
            }
        }

        private bool _useDomain;
        public bool UseDomain
        {
            get => _useDomain;
            set
            {
                _useDomain = value;
                CheckIsChooseAll();
                InvokePropertyChanged(nameof(UseDomain));
            }
        }

        private bool _useEntityFramework;
        public bool UseEntityFramework
        {
            get => _useEntityFramework;
            set
            {
                _useEntityFramework = value;
                CheckIsChooseAll();
                InvokePropertyChanged(nameof(UseEntityFramework));
            }
        }

        private bool _useXUnitTests;
        public bool UseXUnitTests
        {
            get => _useXUnitTests;
            set
            {
                _useXUnitTests = value;
                CheckIsChooseAll();
                InvokePropertyChanged(nameof(UseXUnitTests));
            }
        }

        public GeneratorOptions()
        {
            IsChooseAll = true;
            IsOverride = true;
        }

        private void SetAll()
        {
            _useApplication = _isChooseAll;
            _useDomain = _isChooseAll;
            _useEntityFramework = _isChooseAll;
            _useXUnitTests = _isChooseAll;
            InvokePropertyChanged(nameof(IsChooseAll));
            InvokePropertyChanged(nameof(UseApplication));
            InvokePropertyChanged(nameof(UseDomain));
            InvokePropertyChanged(nameof(UseEntityFramework));
            InvokePropertyChanged(nameof(UseXUnitTests));
        }

        private void CheckIsChooseAll()
        {
            _isChooseAll = _useApplication && _useDomain && _useEntityFramework && _useXUnitTests;
            InvokePropertyChanged(nameof(IsChooseAll));
        }

        public bool HasChecked()
        {
            return UseApplication || UseDomain || UseEntityFramework;
        }
    }
}
