using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Entities.Events;
using Volo.Abp.EventBus;
using Yi.Infra.Rbac.Entities;

namespace Yi.Infra.Rbac.EventHandlers
{
    public class StudentEventHandler : ILocalEventHandler<EntityCreatedEventData<StudentEntity>>, ITransientDependency
    {
        public Task HandleEventAsync(EntityCreatedEventData<StudentEntity> eventData)
        {
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(eventData.Entity));
            return Task.CompletedTask;
        }
    }
}
