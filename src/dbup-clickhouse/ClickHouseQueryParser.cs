#nullable enable
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DbUp.ClickHouse;

internal static class ClickHouseQueryParser
{
    public static IReadOnlyCollection<string> ParseRawQuery(string sql)
    {
        List<string> result = new();
        StringBuilder currentStatementBuilder = new();

        currentStatementBuilder.Clear();

        var currCharOfs = 0;
        var end = sql.Length;
        var ch = '\0';
        var currTokenBeg = 0;
        var blockCommentLevel = 0;
        var parenthesisLevel = 0;

        None:
        if (currCharOfs >= end)
            goto Finish;
        var lastChar = ch;
        ch = sql[currCharOfs++];
        NoneContinue:
        while (true)
        {
            switch (ch)
            {
                case '/':
                    goto BlockCommentBegin;
                case '-':
                    goto LineCommentBegin;
                case '\'':
                    goto Quoted;
                case '`':
                    goto Quoted;
                case ';':
                    if (parenthesisLevel == 0)
                        goto SemiColon;
                    break;
                case '(':
                    parenthesisLevel++;
                    break;
                case ')':
                    parenthesisLevel--;
                    break;
            }

            if (currCharOfs >= end)
                goto Finish;

            lastChar = ch;
            ch = sql[currCharOfs++];
        }

        Quoted:
        Debug.Assert(ch is '\'' or '`');
        while (currCharOfs < end && sql[currCharOfs] != ch)
        {
            currCharOfs++;
        }

        if (currCharOfs < end)
        {
            currCharOfs++;
            ch = '\0';
            goto None;
        }

        goto Finish;

        LineCommentBegin:
        if (currCharOfs < end)
        {
            ch = sql[currCharOfs++];
            if (ch == '-')
                goto LineComment;
            lastChar = '\0';
            goto NoneContinue;
        }

        goto Finish;

        LineComment:
        while (currCharOfs < end)
        {
            ch = sql[currCharOfs++];
            if (ch is '\r' or '\n')
                goto None;
        }

        goto Finish;

        BlockCommentBegin:
        while (currCharOfs < end)
        {
            ch = sql[currCharOfs++];
            switch (ch)
            {
                case '*':
                    blockCommentLevel++;
                    goto BlockComment;
                case '/':
                    continue;
            }

            if (blockCommentLevel > 0)
                goto BlockComment;
            lastChar = '\0';
            goto NoneContinue;
        }

        goto Finish;

        BlockComment:
        while (currCharOfs < end)
        {
            ch = sql[currCharOfs++];
            switch (ch)
            {
                case '*':
                    goto BlockCommentEnd;
                case '/':
                    goto BlockCommentBegin;
            }
        }

        goto Finish;

        BlockCommentEnd:
        while (currCharOfs < end)
        {
            ch = sql[currCharOfs++];
            if (ch == '/')
            {
                if (--blockCommentLevel > 0)
                    goto BlockComment;
                goto None;
            }

            if (ch != '*')
                goto BlockComment;
        }

        goto Finish;

        SemiColon:
        currentStatementBuilder.Append(sql, currTokenBeg, currCharOfs - currTokenBeg - 1);
        result.Add(currentStatementBuilder.ToString());
        while (currCharOfs < end)
        {
            ch = sql[currCharOfs];
            if (char.IsWhiteSpace(ch))
            {
                currCharOfs++;
                continue;
            }

            currentStatementBuilder.Clear();

            currTokenBeg = currCharOfs;
            goto None;
        }

        return result;

        Finish:
        currentStatementBuilder.Append(sql, currTokenBeg, end - currTokenBeg);
        result.Add(currentStatementBuilder.ToString());
        return result;
    }
}
