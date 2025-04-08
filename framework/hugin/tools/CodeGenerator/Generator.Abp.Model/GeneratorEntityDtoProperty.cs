using System;
using System.Collections.Generic;
using System.Linq;

namespace Generator
{
    public class GeneratorEntityDtoProperty : NotifyPropertyChangedObject
    {
        public string Name { get; }

        public string Type { get; }

        private bool _checked;
        public bool Checked
        {
            get => _checked;
            set
            {
                _checked = value;
                InvokePropertyChanged(nameof(Checked));
            }
        }

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

        public List<string> Annotations { get; set; } = new List<string>();
        public string AnnotationString
        {
            get => string.Join("\r\n", Annotations);
            set
            {
                Annotations = value.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                InvokePropertyChanged(nameof(AnnotationString));
            }
        }

        public GeneratorEntityDtoProperty(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
}
