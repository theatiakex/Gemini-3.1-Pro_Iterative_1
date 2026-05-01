using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using SubtitleQc.Core.Models;

namespace SubtitleQc.Core.Parsers;

public class TtmlParser : ISubtitleParser
{
    public IReadOnlyList<Cue> Parse(string content)
    {
        var cues = new List<Cue>();
        if (string.IsNullOrWhiteSpace(content)) return cues;

        try
        {
            var doc = XDocument.Parse(content);
            XNamespace tt = "http://www.w3.org/ns/ttml";
            
            var pElements = doc.Descendants().Where(e => e.Name.LocalName == "p");

            int idCounter = 1;
            foreach (var p in pElements)
            {
                ParseCue(p, cues, ref idCounter);
            }
        }
        catch (Exception)
        {
            // Invalid XML
        }

        return cues;
    }

    private void ParseCue(XElement p, List<Cue> cues, ref int idCounter)
    {
        string? beginStr = p.Attribute("begin")?.Value;
        string? endStr = p.Attribute("end")?.Value;
        
        string id = p.Attribute(XNamespace.Xml + "id")?.Value ?? 
                    p.Attribute("id")?.Value ?? 
                    idCounter.ToString();

        if (beginStr != null && endStr != null && 
            TryParseTime(beginStr, out TimeSpan start) && 
            TryParseTime(endStr, out TimeSpan end))
        {
            var lines = new List<string>();
            ExtractTextAndBr(p, lines);
            
            cues.Add(new Cue(id, start, end, lines));
            idCounter++;
        }
    }

    private void ExtractTextAndBr(XElement element, List<string> lines)
    {
        string currentLine = "";
        foreach (var node in element.Nodes())
        {
            if (node is XText textNode)
            {
                currentLine += textNode.Value.Replace("\n", "").Replace("\r", "").Trim();
            }
            else if (node is XElement childElement)
            {
                if (childElement.Name.LocalName == "br")
                {
                    lines.Add(currentLine.Trim());
                    currentLine = "";
                }
                else
                {
                    var subLines = new List<string>();
                    ExtractTextAndBr(childElement, subLines);
                    if (subLines.Count > 0)
                    {
                        currentLine += subLines[0];
                        for (int i = 1; i < subLines.Count; i++)
                        {
                            lines.Add(currentLine.Trim());
                            currentLine = subLines[i];
                        }
                    }
                }
            }
        }
        
        if (!string.IsNullOrWhiteSpace(currentLine))
        {
            lines.Add(currentLine.Trim());
        }
        else if (lines.Count == 0)
        {
            lines.Add("");
        }
    }

    private bool TryParseTime(string timeStr, out TimeSpan time)
    {
        time = TimeSpan.Zero;
        if (string.IsNullOrWhiteSpace(timeStr)) return false;

        if (TimeSpan.TryParseExact(timeStr, @"hh\:mm\:ss\.fff", CultureInfo.InvariantCulture, out time)) return true;
        if (TimeSpan.TryParseExact(timeStr, @"hh\:mm\:ss", CultureInfo.InvariantCulture, out time)) return true;
        if (TimeSpan.TryParse(timeStr, CultureInfo.InvariantCulture, out time)) return true;

        return false;
    }
}
