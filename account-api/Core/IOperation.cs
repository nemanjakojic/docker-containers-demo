using System.Threading.Tasks;

namespace Array.Test.Core 
{
    // An abstraction of an application operation that encapsulates core application logic.
    public interface IOperation<TRequet, TResult> 
    {
        Task<TResult> Execute(TRequet request);
    }

}