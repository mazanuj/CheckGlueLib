using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CheckGlueLib
{
    public static class Check
    {
        private static readonly object Locker = new object();

        public static async Task<List<GlueStruct>> CheckIsGlue(List<string> domainNames, int chunk = 50, int dop = 20)
        {
            var list = new List<GlueStruct>();
            var domainsList = domainNames.ToChunks(chunk);

            await domainsList.Distinct().ForEachAsync(dop, async domains =>
            {
                try
                {
                    var valueCol = new NameValueCollection
                    {
                        {"xgl", "1"},
                        {"xcy", "1"},
                        {"urls", string.Join("\r\n", domains)}
                    };
                    var respBytes =
                        await new WebClient().UploadValuesTaskAsync("http://xseo.in/mglue", "POST", valueCol);
                    var glueStructs = await ParseHtml.Parse(respBytes);

                    lock (Locker)
                        list.AddRange(glueStructs);
                }
                catch (Exception)
                {
                }
            });

            return list;
        }
    }
}