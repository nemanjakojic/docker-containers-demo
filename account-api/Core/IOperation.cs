using System.Threading.Tasks;

namespace code.Core 
{
    public interface IOperation<TRequet, TResult> 
    {
        Task<TResult> Execute(TRequet request);
    }

}