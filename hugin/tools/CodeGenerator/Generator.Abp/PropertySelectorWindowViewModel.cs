using Generator;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AbpGenerator
{
    public class PropertySelectorWindowViewModel : NotifyPropertyChangedObject
    {
        public GeneratorEntityDto Entity { get; set; }

        public PropertySelectorWindowViewModel()
        {
            if (Global.EntityDto == null)
            {
                Global.EntityDto = new GeneratorEntityDto
                {
                    DisplayName = Global.Entity.GetDisplayName(),
                    Properties = GetProperties(Global.Entity.Properties)
                };
            }

            Entity = Global.EntityDto;
        }

        private ObservableCollection<GeneratorEntityDtoProperty> GetProperties(List<GeneratorEntityProperty> properties)
        {
            var result = new ObservableCollection<GeneratorEntityDtoProperty>();

            foreach (var prop in properties)
            {
                var item = new GeneratorEntityDtoProperty(prop.Name, prop.Type)
                {
                    Checked = true,
                    DisplayName = prop.GetDisplayName(),
                    Annotations = prop.Annotations
                };

                result.Add(item);
            }

            return result;
        }
    }
}
