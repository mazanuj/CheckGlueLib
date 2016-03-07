using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace CheckGlueLib
{
    internal static class ParseHtml
    {
        internal static async Task<List<GlueStruct>> Parse(byte[] respBytes)
        {
            return await Task.Run(() =>
            {
                var list = new List<GlueStruct>();
                try
                {
                    var resp = Encoding.UTF8.GetString(respBytes);

                    var doc = new HtmlDocument();
                    doc.LoadHtml(resp);

                    var rows =
                        doc.DocumentNode.Descendants("tr")
                            .Where(
                                x =>
                                    x.HasAttributes && x.Attributes.Contains("class") &&
                                    (x.Attributes["class"].Value == "cls81" || x.Attributes["class"].Value == "cls82"));

                    list.AddRange(rows.Select(x =>
                    {
                        var isGlue =
                            x.ChildNodes.ElementAt(2)
                                .Descendants("font")
                                .Last()
                                .GetAttributeValue("class", string.Empty) == "cls13";
                        var name = x.ChildNodes.ElementAt(1).FirstChild.InnerText;

                        return new GlueStruct {DomainName = name, IsGlue = isGlue};
                    }));
                }
                catch (Exception)
                {
                }

                return list;
            });
        }
    }
}