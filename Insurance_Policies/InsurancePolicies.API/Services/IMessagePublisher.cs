using System.Threading.Tasks;

namespace InsurancePolicies.API.Services
{
    public interface IMessagePublisher
    {
        Task Publish<T>(T obj);
    }
}