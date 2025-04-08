namespace Generator
{
    public static class Global
    {
        public static GeneratorSolution GeneratorSolution { get; set; }

        public static GeneratorOptions Options { get; set; }

        public static GeneratorFeature Features { get; set; }

        public static GeneratorEntity Entity { get; set; }

        public static GeneratorEntityDto EntityDto { get; set; }

        static Global()
        {
            Options = new GeneratorOptions();
            Features = new GeneratorFeature();
        }
    }
}
