
namespace DbUp.ClickHouse
{
    public static class ClickHouseExtensions
    {
        public static DbUp.Builder.UpgradeEngineBuilder ClickHouseDatabase(this DbUp.Builder.SupportedDatabases supportedDatabases, string connectionString) { }
    }
    public class ClickHouseJournal : DbUp.Engine.IJournal
    {
        public ClickHouseJournal(System.Func<DbUp.Engine.Transactions.IConnectionManager> connectionManagerFactory, System.Func<DbUp.Engine.Output.IUpgradeLog> logFactory, string tableName) { }
        public void EnsureTableExistsAndIsLatestVersion(System.Func<System.Data.IDbCommand> dbCommandFactory) { }
        public string[] GetExecutedScripts() { }
        public void StoreExecutedScript(DbUp.Engine.SqlScript script, System.Func<System.Data.IDbCommand> dbCommandFactory) { }
    }
}
