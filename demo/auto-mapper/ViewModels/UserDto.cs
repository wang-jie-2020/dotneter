using System.Collections.Generic;

namespace Demo.ViewModels
{
    public class UserDto
    {
        public string Name { get; set; }

        public List<UserExtendDto> UserExtends { get; set; }
    }
}
