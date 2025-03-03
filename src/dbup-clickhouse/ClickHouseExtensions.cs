using System;
using DbUp.Builder;

namespace DbUp.ClickHouse;

public static class ClickHouseExtensions
{
    public static UpgradeEngineBuilder ClickHouseDatabase(this SupportedDatabases supportedDatabases, string connectionString)
    {
        throw new NotImplementedException();
    }
}
