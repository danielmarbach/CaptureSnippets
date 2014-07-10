using System.Collections.Generic;
using CaptureSnippets;
using NUnit.Framework;
using ObjectApproval;

[TestFixture]
public class StringExtensionTests
{
    [Test]
    public void TrimIndentation()
    {
        var input= new List<string>
        {
            "   Line1",
            "    Line2",
            "   Line2",
        };
        ObjectApprover.VerifyWithJson(input.TrimIndentation());
    }
    [Test]
    public void ExcludeEmptyPaddingLines()
    {
        var input= new List<string>
        {
            "   ",
            "    Line2",
            "   ",
        };
        ObjectApprover.VerifyWithJson(input.ExcludeEmptyPaddingLines());
    }
    [Test]
    public void ExcludeEmptyPaddingLines_empty_list()
    {
        var input= new List<string>();
        ObjectApprover.VerifyWithJson(input.ExcludeEmptyPaddingLines());
    }
    [Test]
    public void ExcludeEmptyPaddingLines_whitespace_list()
    {
        var input= new List<string>
        {"",  "  "
        };
        ObjectApprover.VerifyWithJson(input.ExcludeEmptyPaddingLines());
    }
    [Test]
    public void TrimIndentation_no_initial_padding()
    {
        var input= new List<string>
        {
            "Line1",
            "    Line2",
            "   Line2",
        };
        ObjectApprover.VerifyWithJson(input.TrimIndentation());
    }
}