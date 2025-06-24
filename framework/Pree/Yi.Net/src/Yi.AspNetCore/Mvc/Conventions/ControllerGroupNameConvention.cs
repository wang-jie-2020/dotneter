using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Yi.AspNetCore.Mvc.Conventions;

public class ControllerGroupNameConvention: IControllerModelConvention
{
    private const string regex = @"(?<=Controllers\.)(.+)(?=\.)";
    
    public void Apply(ControllerModel controller)
    {
        var controllerNamespace = controller.ControllerType.Namespace;
        
        var groupName = string.Empty;
        try
        {
            groupName = Regex.Match(controllerNamespace, regex)?.Groups[1].Value;

            if (groupName.IsNullOrEmpty())
            {
                groupName = controllerNamespace.Split('.').LastOrDefault();
            }
        }
        catch
        {
            // ignored
        }
        
        controller.ApiExplorer.GroupName = groupName;
    }
}