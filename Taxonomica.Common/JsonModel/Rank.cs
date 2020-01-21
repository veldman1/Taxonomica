using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taxonomica.Common
{
    public static class Rank
    {
        public static readonly string[] Ranks = new string[]
        {
                "Kingdom", "Subkingdom", "Infrakingdom", "Superdivision", "Division", "Subdivision", "Superphylum", "Phylum", "Subphylum",
                "Infraphylum", "Superclass", "Class", "Subclass", "Infraclass", "Superorder", "Order", "Suborder",
                "Infraorder", "Section", "Subsection", "Superfamily", "Family", "Subfamily", "Tribe", "Subtribe", "Genus",
                "Subgenus", "Species",
        };

        public static string Next(string rank)
        {
            var next = Ranks.SkipWhile(x => x != rank).Skip(1).FirstOrDefault();
            return next;
        }
    }
}
