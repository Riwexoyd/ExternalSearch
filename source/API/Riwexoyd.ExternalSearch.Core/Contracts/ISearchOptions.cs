namespace Riwexoyd.ExternalSearch.Core.Contracts
{
    public interface ISearchOptions
    {
        bool FilterProviders { get; }

        IReadOnlyCollection<Guid> Providers { get; }
    }
}
