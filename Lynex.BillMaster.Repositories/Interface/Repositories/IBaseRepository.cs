namespace WCC.Repositories.Interface.Repositories
{
    public interface IBaseRepository
    {
        void BeginTrans();

        void CommitTrans();

        void RollBackTrans();
    }
}
