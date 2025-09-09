using DbUp.Support;

namespace DbUp.ClickHouse;

/// <summary>
/// Parses SQL Objects and performs quoting functions for ClickHouse.
/// </summary>
public class ClickHouseObjectParser() : SqlObjectParser("`", "`");

