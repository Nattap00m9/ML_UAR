namespace ML_UAR.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void Complete();
    }
}
