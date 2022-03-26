using System.Threading;
using System.Threading.Tasks;
using RestEase;

namespace ClientClassLibrary
{
    public interface ISomethingApi
    {
        [Get("{id}")]
        [Header("Accept", "application/json")]
        public Task<SomeThing> GetSomethingAsync([Path] string id, CancellationToken cancellationToken = default);
    }
}