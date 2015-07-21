namespace Bezysoftware.Navigation
{
    using System.Threading.Tasks;

    public interface IDeactivateQuery
    {
        Task<bool> CanDeactivateAsync(NavigationType navigationType);
    }
}
