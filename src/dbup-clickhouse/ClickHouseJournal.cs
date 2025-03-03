using System;
using System.Data;
using DbUp.Engine;
using DbUp.Engine.Output;
using DbUp.Engine.Transactions;

namespace DbUp.ClickHouse;

public class ClickHouseJournal(
// Remove pragma once implemented
#pragma warning disable CS9113 // Parameter is unread.
    Func<IConnectionManager> connectionManagerFactory,
    Func<IUpgradeLog> logFactory,
    string tableName
#pragma warning restore CS9113 // Parameter is unread.
    ) : IJournal
{
    public string[] GetExecutedScripts() => throw new NotImplementedException();

    public void StoreExecutedScript(SqlScript script, Func<IDbCommand> dbCommandFactory) => throw new NotImplementedException();

    public void EnsureTableExistsAndIsLatestVersion(Func<IDbCommand> dbCommandFactory) => throw new NotImplementedException();
}
