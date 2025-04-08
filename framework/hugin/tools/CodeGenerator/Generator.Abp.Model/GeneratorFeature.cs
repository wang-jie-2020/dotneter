namespace Generator
{
    public class GeneratorFeature : NotifyPropertyChangedObject
    {
        private readonly bool v1 = true;
        public bool V1
        {
            get => v1;
            set
            {
                //v1 = value;
                //InvokePropertyChanged(nameof(V1));
            }
        }

        public GeneratorFeature()
        {
        }
    }
}
